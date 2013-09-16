using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;
using System.Web;
using System.Text.RegularExpressions;

namespace RAI.SearchEngine
{
    public static class HTMLCleaner3
    {
        /**
         * Método para quitar las etiquetas HTML a un documento
         */
        public static Dictionary<string, string> StripHtml(string source)
        {
            //string result = null;
            Regex regLimpiar = new Regex("[^a-z0-9\r\n\t.,';: -]", RegexOptions.CultureInvariant | RegexOptions.Compiled);
            Dictionary<string, string> result = new Dictionary<string, string>();

            //Quitamos los caracteres especiales
            source = System.Web.HttpUtility.HtmlDecode(source);
           
            //Cargamos el documento con el HTMLAgilityPack
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(source);

            //Quitamos los comentarios del documento
            HtmlNodeCollection commentNodes = doc.DocumentNode.SelectNodes("//comment()");
            if(commentNodes!=null){
                foreach (HtmlNode comment in commentNodes)
                {
                    comment.ParentNode.RemoveChild(comment);
                }
            }
            //Quitamos las etiquetas <script> y su contenido
            foreach (var script in doc.DocumentNode.Descendants("script").ToArray())
            {
                script.Remove();
            }
            //Quitamos las etiquetas <style> y su contenido
            foreach (var style in doc.DocumentNode.Descendants("style").ToArray())
            {
                style.Remove();
            }

            string aux = "";
            //Procesamos el título
            foreach (var title in doc.DocumentNode.Descendants("title").ToArray())
            {
                aux += title.InnerText + " ";
            }
            //Console.WriteLine("TÍTULO: " + aux);
            result.Add("title", regLimpiar.Replace(aux, ""));

            aux = "";
            //Procesamos las negritas y strongs
            foreach (var title in doc.DocumentNode.Descendants("b").ToArray())
            {
                aux += title.InnerText + " ";
            }
            foreach (var title in doc.DocumentNode.Descendants("strong").ToArray())
            {
                aux += title.InnerText + " ";
            }
            //Console.WriteLine("NEGRITA: " + aux);
            result.Add("b", regLimpiar.Replace(aux, ""));

            aux = "";
            //Procesamos las cursivas
            foreach (var title in doc.DocumentNode.Descendants("i").ToArray())
            {
                aux += title.InnerText + " ";
            }
            //Console.WriteLine("CURSIVA: " + aux);
            result.Add("i", regLimpiar.Replace(aux, ""));

            aux = "";
            //Procesamos los títulos (1)
            foreach (var title in doc.DocumentNode.Descendants("h1").ToArray())
            {
                aux += title.InnerText + " ";
            }
            foreach (var title in doc.DocumentNode.Descendants("h2").ToArray())
            {
                aux += title.InnerText + " ";
            }
            foreach (var title in doc.DocumentNode.Descendants("h3").ToArray())
            {
                aux += title.InnerText + " ";
            }
            foreach (var title in doc.DocumentNode.Descendants("h4").ToArray())
            {
                aux += title.InnerText + " ";
            }
            foreach (var title in doc.DocumentNode.Descendants("h5").ToArray())
            {
                aux += title.InnerText + " ";
            }
            //Console.WriteLine("ENCABEZADOS: " + aux);
            result.Add("h", regLimpiar.Replace(aux, ""));

            aux = "";
            //Procesamos las keywords
            HtmlNode node = doc.DocumentNode.SelectSingleNode("//meta[@name='keywords']");
            if (node != null)
            {
                aux = node.GetAttributeValue("content", "");
            }
            //Console.WriteLine("KEYWORDS: " + aux);
            result.Add("keywords", regLimpiar.Replace(aux, ""));

            aux = "";
            //Procesamos las descripciones
            node = doc.DocumentNode.SelectSingleNode("//meta[@name='description']");
            if (node != null)
            {
                aux = node.GetAttributeValue("content", "");
            }
            //Console.WriteLine("DESCRIPTION: " + aux);
            result.Add("description", regLimpiar.Replace(aux, ""));


            //Sacamos el texto
            result.Add("content", regLimpiar.Replace(doc.DocumentNode.InnerText, ""));
            return result;
        }
    }
}