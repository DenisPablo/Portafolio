using HtmlAgilityPack;
using System.Text;

namespace Portafolio.Servicios
{

    public interface IHtmlHelper
    {
        string TruncateHtml(string html, int maxLength);
    }

    public class HtmlHelper : IHtmlHelper
    {

        public string TruncateHtml(string html, int maxLength)
        {
            if (string.IsNullOrEmpty(html) || maxLength <= 0)
                return string.Empty;

            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            int currentLength = 0;

            // Método recursivo que recorre y reconstruye el HTML
            string ProcessNode(HtmlNode node)
            {
                if (node.NodeType == HtmlNodeType.Text)
                {
                    string text = node.InnerText;
                    if (currentLength + text.Length > maxLength)
                    {
                        int remaining = maxLength - currentLength;
                        currentLength = maxLength;
                        return System.Net.WebUtility.HtmlEncode(text.Substring(0, remaining));
                    }
                    else
                    {
                        currentLength += text.Length;
                        return node.OuterHtml;
                    }
                }
                else if (node.NodeType == HtmlNodeType.Element)
                {
                    var sb = new StringBuilder();
                    sb.Append($"<{node.Name}{GetAttributes(node)}>");
                    foreach (var child in node.ChildNodes)
                    {
                        if (currentLength >= maxLength)
                            break;
                        sb.Append(ProcessNode(child));
                    }
                    sb.Append($"</{node.Name}>");
                    return sb.ToString();
                }
                return string.Empty;
            }

            // Método auxiliar para reconstruir los atributos
            string GetAttributes(HtmlNode node)
            {
                if (!node.HasAttributes)
                    return string.Empty;
                var sb = new StringBuilder();
                foreach (var attr in node.Attributes)
                {
                    sb.Append($" {attr.Name}=\"{attr.Value}\"");
                }
                return sb.ToString();
            }

            var result = new StringBuilder();
            foreach (var node in doc.DocumentNode.ChildNodes)
            {
                if (currentLength >= maxLength)
                    break;
                result.Append(ProcessNode(node));
            }
            if (currentLength >= maxLength)
                result.Append("...");

            return result.ToString();
        }


    }

}

