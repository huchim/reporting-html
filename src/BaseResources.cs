namespace Jaguar.Reporting.Html
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;

    public abstract class BaseResources : IResource
    {
        /// <inheritdoc/>
        public string WorkingDirectory { get; set; } = string.Empty;

        /// <summary>
        /// Especifica el tipo de filtro a usar.
        /// </summary>
        protected string FilterExpression { get; set; } = string.Empty;        

        /// <inheritdoc/>
        public virtual List<byte[]> Extract(string content)
        {
            var resourceList = new List<byte[]>();
            var externalResourceList = GetExtenalResources(content);

            foreach (var externalResource in externalResourceList)
            {
                var fileData = GetFile(externalResource);
                resourceList.Add(fileData);
            }

            return resourceList;
        }

        /// <inheritdoc/>
        public Stream GetStream(string content)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public byte[] MergeResources(List<byte[]> resources)
        {
            byte[] result = new byte[resources.Sum(a => a.Length)];

            using (var stream = new MemoryStream(result))
            {
                foreach (byte[] bytes in resources)
                {
                    stream.Write(bytes, 0, bytes.Length);
                }
            }

            return result;
        }

        protected string[] GetEmbedResources(string content, string tagName)
        {
            var filter = $"<\\s*?{tagName}\\b[^>]*>([\\s\\S]*)<\\/{tagName}\\b[^>]*>";
            var regExpression = new Regex(filter);
            var results = new List<string>();
            
            foreach (Match match in regExpression.Matches(content))
            {
                var blockContent = match.Groups[1].Value as string;
                
                if (!string.IsNullOrEmpty(blockContent))
                {
                    results.Add(blockContent);
                }
            }

            return results.ToArray();
        }

        private string[] GetExtenalResources(string content)
        {
            var regExpression = new Regex(this.FilterExpression);
            var results = new List<string>();

            foreach (Match match in regExpression.Matches(content))
            {
                var file = match.Groups[1].Value.ToString();

                if (!isRemoteResource(file))
                {
                    file = Path.Combine(this.WorkingDirectory, file);
                }

                results.Add(file);
            }

            return results.ToArray();
        }

        private byte[] GetFile(string filePath)
        {
            if (!isRemoteResource(filePath))
            {
                return File.ReadAllBytes(filePath);
            }
            else
            {
                throw new NotImplementedException("No se pueden descargar archivos remotos aún.");
            }
        }

        private bool isRemoteResource(string url)
        {
            url = url.ToUpperInvariant();
            var isHttp = url.StartsWith("http://") || url.StartsWith("https://") || url.StartsWith("//") || url.StartsWith("://");
            var isFtp = url.StartsWith("ftp://");
            return isHttp || isFtp;
        }
    }
}