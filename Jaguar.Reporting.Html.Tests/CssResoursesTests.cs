using System;
using System.IO;
using System.Text;
using Jaguar.Reporting.Generators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Jaguar.Reporting.Html.Tests
{
    [TestClass]
    public class CssResoursesTests
    {
        [TestMethod]
        public void TestMustBeOneCssContent()
        {
            var css = new CssResources()
            {
                WorkingDirectory = Directory.GetCurrentDirectory()
            };

            var html = "<span>before</span><link rel=\"stylesheet\" type=\"text/css\" href=\"style.css\" /><span>after</span>";
            var cssContent = css.Extract(html);
            Assert.AreEqual(1, cssContent.Count);
        }

        [TestMethod]
        public void TestMustBeTwoCssContent()
        {
            var css = new CssResources()
            {
                WorkingDirectory = Directory.GetCurrentDirectory()
            };

            var html = "<span>before</span><link rel=\"stylesheet\" type=\"text/css\" href=\"style.css\" /> between content <link rel=\"stylesheet\" type=\"text/css\" href=\"style.css\" /><span>after</span>";
            var cssContent = css.Extract(html);
            Assert.AreEqual(2, cssContent.Count);
        }

        [TestMethod]
        public void TestDetectEmbedCss()
        {
            var css = new CssResources()
            {
                WorkingDirectory = Directory.GetCurrentDirectory()
            };

            var html = "<span>before</span><link rel=\"stylesheet\" type=\"text/css\" href=\"style.css\" /> between content <style att=\"a\">\nbody { color: red; }\n</style>";
            var cssContent = css.Extract(html);
            Assert.AreEqual(2, cssContent.Count);
        }

        [TestMethod]
        public void TestParseExternalCss()
        {
            var css = new CssResources()
            {
                WorkingDirectory = Directory.GetCurrentDirectory()
            };

            var externalFile = Path.GetRandomFileName() + ".css";
            var cssTest = "body { color: blue; }";
            File.WriteAllText(externalFile, cssTest);
            var html = $"<span>before</span><link rel=\"stylesheet\" type=\"text/css\" href=\"{externalFile}\" />";

            // Obtener el contenido CSS en bytes como lo devuelve.
            var cssContentList = css.Extract(html);
            var cssContent = css.MergeResources(cssContentList);
            var cssOutput = Encoding.UTF8.GetString(cssContent);
            Assert.AreEqual(cssTest, cssOutput);

            if (File.Exists(externalFile))
            {
                File.Delete(externalFile);
            }
        }

        [TestMethod]
        public void TestParseExternalAndEmbedCss()
        {
            var css = new CssResources()
            {
                WorkingDirectory = Directory.GetCurrentDirectory()
            };

            var externalFile = Path.GetRandomFileName() + ".css";
            var cssTest = "body { color: blue; }";
            var cssTestEmbed = "p { color: red; }";
            File.WriteAllText(externalFile, cssTest);
            var html = $"<link rel=\"stylesheet\" type=\"text/css\" href=\"{externalFile}\" /><style>{cssTestEmbed}</style>";

            // Obtener el contenido CSS en bytes como lo devuelve.
            var cssContentList = css.Extract(html);
            var cssContent = css.MergeResources(cssContentList);
            var cssOutput = Encoding.UTF8.GetString(cssContent);
            Assert.AreEqual(cssTest + cssTestEmbed, cssOutput);

            if (File.Exists(externalFile))
            {
                File.Delete(externalFile);
            }
        }
    }
}
