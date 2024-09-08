using Ganss.Xss;

namespace Portafolio.Servicios
{
    public interface IProyectoUtilidades
    {
        string LimpiarInputHTML(string inputHtml);
    }

    public class ProyectoUtilidades : IProyectoUtilidades
    {
        public string LimpiarInputHTML(string inputHtml)
        {
            if (string.IsNullOrWhiteSpace(inputHtml))
            {
                return string.Empty;
            }

            var sanitizer = new HtmlSanitizer();

            sanitizer.AllowedTags.Add("b");
            sanitizer.AllowedTags.Add("i");
            sanitizer.AllowedTags.Add("u");
            sanitizer.AllowedTags.Add("p");
            sanitizer.AllowedTags.Add("a");
            sanitizer.AllowedTags.Add("ul");
            sanitizer.AllowedTags.Add("li");
            sanitizer.AllowedAttributes.Add("href");

            var htmlLimpio = sanitizer.Sanitize(inputHtml);

            return htmlLimpio;
        }
    }
}
