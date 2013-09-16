using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace RAI.Crawler
{
    public class GoogleCrawler : IDisposable
    {
        /// <summary>
        /// Sólo para depurar. Muestra el contenido de la base de datos.
        /// </summary>
        public static void ShowAllUris()
        {
            using (RAI.Crawler.Data.DataAdapterDataContext context = new RAI.Crawler.Data.DataAdapterDataContext()) {
                // Al usar using(..) se cierra la conexión a la BD al salir del bloque using(...){...}, aunque haya exception en medio
                foreach (RAI.Crawler.Data.DataUri row in context.DataUri) {
                    Console.WriteLine(row.Id + " " + row.AbsoluteUri + " " +
                        (row.Parent != null ? row.Parent.ToString() : "-") + " " +
                        (row.Cache != null ? row.Cache : "-") + " " +
                        (row.Status != null ? row.Status.ToString() : "-"));
                }
            }
        }

        /// <summary>
        /// Máximo número de intentos de descarga.
        /// </summary>
        protected const int MAX_ATTEMPTS = 3;
        /// <summary>
        /// Regex para extraer los enlaces de resultados.
        /// </summary>
        protected static readonly Regex _resultRegex = new Regex("<h3[^>]*><a href=\"(?<uri>[^\"]+)\"", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture | RegexOptions.Singleline);
        /// <summary>
        /// Regex para extraer los enlaces a página siguiente.
        /// </summary>
        protected static readonly Regex _nextRegex = new Regex("<a href=\"(?<uri>[^\"]+)\"([^>]*>){4}Next", RegexOptions.Compiled | RegexOptions.ExplicitCapture | RegexOptions.Singleline);
        
        /// <summary>
        /// Generador de números aleatorios para guardar los archivos descargados.
        /// </summary>
        protected Random _rand;
        /// <summary>
        /// Contexto para trabajar con la base de datos
        /// </summary>
        protected RAI.Crawler.Data.DataAdapterDataContext _context;
        /// <summary>
        /// Webclient para descargar contenido desde Internet.
        /// </summary>
        protected WebClient _webClient;

        public GoogleCrawler()
        {
            this._rand = new Random();
            this._context = new RAI.Crawler.Data.DataAdapterDataContext();
            this._webClient = new WebClient();
        }

        /// <summary>
        /// Añade una nueva semilla a la base de datos.
        /// </summary>
        /// <param name="uriString">La Uri de la semilla.</param>
        public void AddSeed(string uriString)
        {
            Uri uri = null;
            if (Uri.TryCreate(uriString, UriKind.Absolute, out uri)) {
                // Crear DataUri e insertar en la base de datos
                RAI.Crawler.Data.DataUri newUri = new Data.DataUri(uri.AbsoluteUri, null, null, null);
                this._context.DataUri.InsertOnSubmit(newUri);
                try{
					// Submit para la base de datos
                    this._context.SubmitChanges();
                }
                catch { }
            } else { 
                // La Uri proporcionada no está bien formada
            }
        }
        /// <summary>
        /// Arranca el crawler. No termina hasta que se hayan procesado todas las Uris pendientes.
        /// </summary>
        public void Run()
        {
            RAI.Crawler.Data.DataUri currentUri = null;
            while ((currentUri = this.GetPendingUri()) != null) {
                if (currentUri.IsSeed) {
                    currentUri.Status = this.ProcessSeed(currentUri);
                } else {
                    currentUri.Status = this.ProcessResult(currentUri);
                }
                try {
                    this._context.SubmitChanges(); // Actualizar status en la base de datos
                } catch (System.Data.SqlClient.SqlException) {
                }
            }
        }
        /// <summary>
        /// Devuelve una Uri pendiente de procesar.
        /// </summary>
        /// <returns>La Uri para procesar.</returns>
        protected RAI.Crawler.Data.DataUri GetPendingUri()
        {
            RAI.Crawler.Data.DataUri row = null;
            row = this._context.DataUri.FirstOrDefault(u => u.Status == null); // default sería null, cuando no hay Uris pendientes
            return row;
        }
        /// <summary>
        /// Procesa una Uri semilla. Navega por los resultados e inserta nuevas Uris de resultados en la base de datos.
        /// </summary>
        /// <param name="seed">La Uri semilla.</param>
        /// <returns>true si correcto, false si hay algún error</returns>
        protected bool ProcessSeed(RAI.Crawler.Data.DataUri seed)
        {
            Console.WriteLine("Processing Seed " + seed.Id + " : " + seed.AbsoluteUri);

            Uri uriPaginacion = new Uri(seed.AbsoluteUri);

            try
            {
                // Mientras haya más resultados y no haya error
                while (uriPaginacion != null)
                {
                    // Extraer Uris de resultados e insertar en la base de datos

                    // Descargamos el contenido de la Uri semilla
                    String content = Download(uriPaginacion.AbsoluteUri);
                    // Buscamos coincidencias en el contenido descargado con la expresion regular que busca uris de resultados
                    MatchCollection seedUris = GoogleCrawler._resultRegex.Matches(content);
                    // Para cada uri obtenida en el match insertamos un nuevo registro en la base de datos.
                    foreach (Match uri in seedUris)
                    {
                        // Insertamos la uri resultado nueva con el valor de padre inicializado a nuestra semilla
                        RAI.Crawler.Data.DataUri newUri = new Data.DataUri(uri.Groups["uri"].Value.Replace("&amp;", "&"), seed, null, null);
                        // Submit para la base de datos
                        this._context.DataUri.InsertOnSubmit(newUri);
                    }
                    // Extraer siguiente página de resultados
                    Match next = GoogleCrawler._nextRegex.Match(content);
                    // Si encuentra siguiente pagina aumentamos el contador de paginas en 1 y sustituimos uriPaginacion con la nueva
                    if (next.Success)
                    {
                        uriPaginacion = new Uri(uriPaginacion, next.Groups["uri"].Value.Replace("&amp;", "&"));
                    }
                    // Si no uriPaginacion toma valor null
                    else
                    {
                        uriPaginacion = null; // No hay más resultados
                    }

                }

                //Si todo es correcto
                return true;
            } 
            catch{
            }

            //Si se produce alguna excepcion en el try, error
            return false;
        }
        /// <summary>
        /// Procesa una Uri de resultados. Descarga el contenido y lo almacena localmente.
        /// </summary>
        /// <param name="result">La Uri de resultados.</param>
        /// <returns>true si correcto, false si hay algún error.</returns>
        protected bool ProcessResult(Data.DataUri result)
        {
            Console.WriteLine("Processing Result " + result.Id + "(" + result.Parent + ") : " + result.AbsoluteUri);
            
            try
            {
				// Utilizamos una estructura con StreamWriter para almacenar en ficheros los contenidos de las uris de resultados
                String file = CreateLocalFile();
                System.IO.StreamWriter fileSW = new StreamWriter(file);
				// Descargamos el uri resultado
				String uriData = Download(result.AbsoluteUri);
                fileSW.Write(uriData);
                fileSW.Close();
				//Para guardar el fichero se usa:
				result.Cache = System.IO.Path.GetFileNameWithoutExtension(file);
				
                // Si la descarga es correcta
                Console.WriteLine(" : " + result.Cache);
                return true;
            }
            catch { }

            // Si no es correcta (ha saltado excepcion en el try), error
            Console.WriteLine(" : failure");
            return false;
        }
        /// <summary>
        /// Descarga el contenido de la Uri especificada.
        /// </summary>
        /// <param name="uriString">La Uri del archivo a descargar.</param>
        /// <returns>El contenido descargado.</returns>
        protected string Download(string uriString)
        {
            string contents = null;
            int attempts = 0;
            while (contents == null && attempts < GoogleCrawler.MAX_ATTEMPTS) {
                attempts++;
                try {
                    // Dormir un tiempo aleatorio entre 1 y 3 segundos, para evitar bloqueos
                    System.Threading.Thread.Sleep(this._rand.Next(1000, 3000));
                    this._webClient.Headers.Add(System.Net.HttpRequestHeader.UserAgent, "Mozilla/5.0 (Windows; U; Windows NT 5.1; de; rv:1.9.2.3) Gecko/20100401 Firefox/3.6.3 (FM Scene 4.6.1)");
                    contents = this._webClient.DownloadString(uriString);
                } catch (WebException) {
                } catch {
                    attempts = GoogleCrawler.MAX_ATTEMPTS;
                }
            }
            return contents;
        }
        /// <summary>
        /// Crea un nuevo archivo local para almacenar un documento de resultado.
        /// El archivo se crea vacío y localizado de forma aleatoria.
        /// </summary>
        /// <returns>El path al archivo local.</returns>
        protected string CreateLocalFile()
        {
            string dirId = this._rand.Next(0,100).ToString("00"); // Directorio aleatorio, entre 00 y 99
            string dir = @".\Download\" + dirId;
            Directory.CreateDirectory(dir);
            string fileId = Directory.GetFiles(dir).Length.ToString("000"); // Dentro del directorio, se numeran sequencialmente
            string filename = Path.Combine(dir, "2012-" + dirId + "-" + fileId + ".html"); //Nombre final: 2012-<dir>-<seq>.html
            File.Create(filename).Close();
            return filename;
        }
        /// <summary>
        /// Cerrar conexiones a base de datos, etc.
        /// </summary>
        public void Dispose()
        {
            this._context.Dispose();
            this._webClient.Dispose();
        }
    }
}
