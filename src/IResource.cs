using System.Collections.Generic;
using System.IO;

namespace Jaguar.Reporting.Html
{
    public interface IResource
    {
        /// <summary>
        /// Extrae la información del recurso.
        /// </summary>
        /// <param name="content">Contenido a evaluar.</param>
        /// <returns>Información del recurso.</returns>
        List<byte[]> Extract(string content);

        /// <summary>
        /// Obtiene todo los recursos en un flujo de memoria.
        /// </summary>
        /// <param name="content">Contenido a evaluar.</param>
        /// <returns>Flujo de datos con el recurso.</returns>
        Stream GetStream(string content);

        /// <summary>
        /// Devuelve el contenido de los recursos una vez que han sido mezclados.
        /// </summary>
        /// <param name="resources">Lista de recursos.</param>
        /// <returns>Lista de recursos.</returns>
        byte[] MergeResources(List<byte[]> resources);

        /// <summary>
        /// Obtiene la ruta al directorio base del recurso.
        /// </summary>
        string WorkingDirectory { get; set; }
    }
}