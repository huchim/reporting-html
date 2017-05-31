// -----------------------------------------------------------------------
// <copyright file="HtmlGenerator.cs" company="Carlos Huchim Ahumada">
// Este código se libera bajo los términos de licencia especificados.
// </copyright>
// -----------------------------------------------------------------------
namespace Jaguar.Reporting.Generators
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Jaguar.Reporting.Common;
    using Jaguar.Reporting.Html;
    using Mustache;

    /// <summary>
    /// Genera la información en formato CSV.
    /// </summary>
    public class HtmlGenerator : IGeneratorEngine
    {
        private ReportHandler report;
        private Dictionary<string, object> variables;

        /// <inheritdoc/>
        public string FileExtension => ".html";

        /// <inheritdoc/>
        public Guid Id => new Guid("eeb51011-ee09-425d-9473-3ee4192b9e2e");

        /// <inheritdoc/>
        public bool IsEmbed => true;

        /// <inheritdoc/>
        public string MimeType => "text/html";

        /// <inheritdoc/>
        public string Name => "Página HTML";

        /// <summary>
        /// Obtiene un valor que indica que las hojas de estilo serán incrustadas en el documento actual.
        /// </summary>
        public bool EmbedResources => false;

        /// <summary>
        /// Devuelve el contenido del documento.
        /// </summary>
        /// <param name="resourceHandle">Instancia que se encargará de obtener el tipo exacto de recursos.</param>
        /// <param name="content">Contenido HTML.</param>
        /// <returns>Contenido del recurso.</returns>
        public List<byte[]> GetResources(IResource resourceHandle, string content)
        {
            var resourceResult = resourceHandle.Extract(content);

            return resourceResult;
        }

        /// <summary>
        /// Devuelve todo el CSS utilizado en el documento incluyendo las hojas de estilo externas.
        /// </summary>
        /// <param name="content">Contenido HTML.</param>
        /// <returns>Contenido CSS del documento.</returns>
        public string GetAllCss(string content)
        {
            // Obtener la lista de documentos.
            var resoureceCss = GetResources(new CssResources { WorkingDirectory = this.report.WorkDirectory }, content);

            // Unirlos todos.
            var resourceCssMerged = this.MergeResources(resoureceCss);

            // Devolver todo el contenido.
            return Encoding.UTF8.GetString(resourceCssMerged);
        }

        /// <summary>
        /// Devuelve el contenido de los recursos una vez que han sido mezclados.
        /// </summary>
        /// <param name="resources">Lista de recursos.</param>
        /// <returns>Lista de recursos.</returns>
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

        private string TemplateFile
        {
            get
            {
                if (string.IsNullOrEmpty(this.report.Options["html.template"] as string))
                {
                    throw new ArgumentNullException(nameof(this.TemplateFile));
                }

                return Path.Combine(this.report.WorkDirectory, this.report.Options["html.template"].ToString());
            }
        }

        /// <inheritdoc/>
        public byte[] GetAllBytes(ReportHandler report, List<DataTable> data, Dictionary<string, object> variables)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public string GetString(ReportHandler report, List<DataTable> data, Dictionary<string, object> variables)
        {
            // Asignar variables de la clase.
            this.report = report;
            this.variables = variables;

            // Cargar el archivo en memoria.
            var htmlTemplate = this.LoadTemplateFile();

            // Reemplazar variables.
            htmlTemplate = this.ReplaceDataVariables(htmlTemplate, variables);

            // Crear el compilador y agregar una etiqueta personalizada.
            var compiler = new HtmlFormatCompiler();
            compiler.RegisterTag(new IfEqualTagDefinition(), true);

            // Compilar la plantilla.
            var generator = compiler.Compile(htmlTemplate);

            // Unificar los resultados.
            var mergedData = this.MergeData(data);

            // Generar los resultados.
            string result = generator.Render(mergedData);


            return result;
        }

        /// <summary>
        /// Carga el texto de la plantilla.
        /// </summary>
        /// <returns></returns>
        private string LoadTemplateFile()
        {
            var templateFile = Path.Combine(this.report.WorkDirectory, this.TemplateFile);
            return File.ReadAllText(templateFile, Encoding.UTF8);
        }

        private Dictionary<string, List<Dictionary<string, object>>> MergeData(List<DataTable> data)
        {
            var c = new Dictionary<string, List<Dictionary<string, object>>>();

            foreach (var table in data)
            {
                // Agregar la llave para poder usarla.
                c.Add(table.TableName, new List<Dictionary<string, object>>());

                // Hacer referencia al nodo.
                var m = c[table.TableName];

                foreach (var dataRow in table.Rows)
                {
                    var row = new Dictionary<string, object>();

                    foreach (var column in dataRow.Columns)
                    {
                        if (column.Value is string)
                        {
                            column.Value = column.Value.ToString();
                        }

                        if (column.Value is DBNull)
                        {
                            column.Value = null;
                        }

                        row.Add(column.Name, column.Value);
                    }

                    m.Add(row);
                }
            }

            return c;
        }

        private string ReplaceDataVariables(string input, Dictionary<string, object> variables)
        {
            foreach (var variable in variables)
            {
                input = input.Replace($"%{variable.Key}%", variable.Value.ToString());
            }

            return input;
        }
    }
}