using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Jaguar.Reporting.Html
{
    public class CssResources : BaseResources
    {
        public CssResources()
        {
            // Filtro para recuperar los recursos CSS externos.
            FilterExpression = "<link\\s+(?:[^>]*?\\s+)?href=\"([^\"]*)\\\"";
        }

        /// <inheritdoc/>
        public override List<byte[]> Extract(string content)
        {
            // Obtener todos el contenido de todos los archivos vinculados a "link".
            var externalResources = base.Extract(content);

            // Obtiener todos los bloques de CSS dentro de la etiqueta "style".
            var embedResources = GetEmbedResources(content, "style");
            
            foreach (var embedResource in embedResources)
            {
                externalResources.Add(Encoding.UTF8.GetBytes(embedResource));
            }

            return externalResources;
        }        
    }
}
