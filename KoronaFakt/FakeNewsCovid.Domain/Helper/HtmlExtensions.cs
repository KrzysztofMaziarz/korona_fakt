using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web;
using HtmlAgilityPack;

namespace FakeNewsCovid.Domain.Helper
{
    public static class HtmlExtensions
    {
        private static readonly Dictionary<int, Func<string, string>> filters = new Dictionary<int, Func<string, string>>
        {
            { 0, HttpUtility.HtmlDecode },
            { 1, text => Regex.Replace(text, @"(<(.*?)>)", string.Empty, RegexOptions.Compiled) }
        };

        public static void FilterOutTags(this HtmlNode node)
        {
            var tags = new[] { "script", "style", "object", "embed", "img", "audio", "video" };
            foreach (var tag in tags)
            {
                node.InnerHtml = Regex.Replace(node.InnerHtml, $"<\\s*{tag}[^>]*>(.*?)<\\s*\\/\\s*{tag}>", string.Empty, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            }
        }

        public static string FilterOutHtml(this string text)
        {
            if (text == null)
            {
                return string.Empty;
            }

            for (int i = 0; i < filters.Count; i++)
            {
                text = filters[i](text);
            }

            return text.Trim();
        }
    }
}
