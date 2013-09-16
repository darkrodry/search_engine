using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using nFire.Core;
using nFire.Eirex;

namespace RAI.SearchEngine.Run
{
    class Program
    {
        static void Main(string[] args)
        {
            /* TO DO: actualizar el nombre del servicio y el path al archivo mdf */
            string dbConnectionString = @"Data Source=.\SQLEXPRESS"
                + @";AttachDbFilename=""C:\bbdd\BD2012C.mdf"""
                + @";Integrated Security=True;Connect Timeout=30;User Instance=True";

            /* TO DO: instanciar el motor de búsqueda */
            RAI.SearchEngine.ISearchEngine engine = new MotorOkapiBM25F(); 

            /* TO DO: cambiar rutas a colección y topics */
            DocumentCollection docCol = new DocumentCollection(@"C:\data\2012-documents.biased.tar");
            TopicCollection topCol = new TopicCollection(@"C:\data\2012-topics.xml");

            // 1. Indexar colección (SÓLO UNA VEZ!)
            //engine.BuildIndex(dbConnectionString, docCol);

            // 2. Hacer consultas
            List<IRun<IListResult>> runs = new List<IRun<IListResult>>();
            foreach (var topic in topCol) {
                runs.Add(engine.RunModulo1(dbConnectionString, docCol, topic));
                // 2.1. Opcional: mostrar resultados por pantalla, snippets, etc.
            }
            Formatter formatter = new Formatter();
            formatter.Write(runs, @"C:\data\2012_" + engine.Name + ".run");
        }
    }
}
