using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using nFire.Core;
using nFire.Eirex;

namespace RAI.SearchEngine
{
    public interface ISearchEngine
    {
        /// <summary>
        /// Obtiene el nombre del motor de búsqueda, del tipo "2012-XX.Y"
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Construye un índice inverso en la base de datos especificada a partir de la colección de documentos especificada.
        /// </summary>
        /// <param name="dbConnectionString">Cadena de conexión a la base de datos.</param>
        /// <param name="docCollection">Colección de documentos.</param>
        void BuildIndex(string dbConnectionString, DocumentCollection docCollection);

        /// <summary>
        /// Ejecuta la consulta especificada y devuelve los resultados según el módulo 1 del motor.
        /// </summary>
        /// <param name="dbConnectionString">Cadena de conexión a la base de datos con el índice inverso.</param>
        /// <param name="docCol">La colección de documentos de la que se obtuvo el índice inverso.</param>
        /// <param name="topic">El topic para el que devolver resultados.</param>
        /// <returns>El run con los resultados para el topic.</returns>
        /// <remarks>La query de entrada al motor debe ser sólo la propiedad Title del topic.</remarks>
        IRun<IListResult> RunModulo1(string dbConnectionString, DocumentCollection docCol, Topic topic);
        /// <summary>
        /// Ejecuta la consulta especificada y devuelve los resultados según los módulos 1+2 del motor.
        /// </summary>
        /// <param name="dbConnectionString">Cadena de conexión a la base de datos con el índice inverso.</param>
        /// <param name="docCol">La colección de documentos de la que se obtuvo el índice inverso.</param>
        /// <param name="topic">El topic para el que devolver resultados.</param>
        /// <returns>El run con los resultados para el topic.</returns>
        /// <remarks>La query de entrada al motor debe ser sólo la propiedad Title del topic.</remarks>
        IRun<IListResult> RunModulo1y2(string dbConnectionString, DocumentCollection docCol, Topic topic);
        /// <summary>
        /// Ejecuta la consulta especificada y devuelve los resultados según los módulos 1+2+3 del motor.
        /// </summary>
        /// <param name="dbConnectionString">Cadena de conexión a la base de datos con el índice inverso.</param>
        /// <param name="docCol">La colección de documentos de la que se obtuvo el índice inverso.</param>
        /// <param name="topic">El topic para el que devolver resultados.</param>
        /// <returns>El run con los resultados para el topic.</returns>
        /// <remarks>La query de entrada al motor debe ser sólo la propiedad Title del topic.</remarks>
        IRun<IListResult> RunModulo1y2y3(string dbConnectionString, DocumentCollection docCol, Topic topic);
    }
}
