using System;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace FakeNewsCovid.Domain.Helper
{
    public static class HtmlHelper
    {
        public static string FormatHtml(string document)
        {
            HtmlDocument htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(document);

            var nodes = htmlDocument.QuerySelectorAll("div").ToArray();
            if (nodes.Length == 0)
            {
                return string.Empty;
            }

            var text = new StringBuilder();
            for (int i = 0; i < nodes.Length; i++)
            {
                nodes[i].FilterOutTags();
                text.Append(nodes[i].InnerHtml);
            }

            return text.ToString().FilterOutHtml();
        }
    }
}
