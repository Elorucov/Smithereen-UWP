using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Windows.UI.Text;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

// Based on https://github.com/xleon/HTML2XAML/blob/master/XAMLHtml/XAMLHtml.cs
// but simplified and added executing action after clicking on links.

namespace SmithereenUWP.Helpers
{
    internal class HTMLTextParser
    {
        private static RichTextBlock _currentObject;

        public static void Parse(string xhtml, RichTextBlock rtb)
        {
            if (rtb == null) return;
            _currentObject = rtb;

            StringBuilder sb = new StringBuilder(xhtml.Length);
            sb.Append(xhtml);
            sb.Replace("\r", " ");
            sb.Replace("\n", "");
            var blocks = GenerateBlocksForHtml(sb.ToString());

            _currentObject = null;
            rtb.Blocks.Clear();

            foreach (var block in blocks)
            {
                rtb.Blocks.Add(block);
            }
        }

        private static List<Block> GenerateBlocksForHtml(string xhtml)
        {
            var blocks = new List<Block>();

            try
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(xhtml);

                foreach (var node in doc.DocumentNode.ChildNodes)
                {
                    if (node.Name == "p" || node.Name == "P")
                    {
                        var paragraph = GenerateParagraph(node);
                        blocks.Add(paragraph);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            return blocks;
        }

        private static string CleanText(string input)
        {
            var clean = Windows.Data.Html.HtmlUtilities.ConvertToText(input);
            if (clean == "\0")
                clean = "\n";
            return clean;
        }

        private static void AddChildren(Paragraph p, HtmlNode node)
        {
            var added = false;
            foreach (var child in node.ChildNodes)
            {
                var i = GenerateBlockForNode(child);
                if (i != null)
                {
                    p.Inlines.Add(i);
                    added = true;
                }
            }
            if (!added)
            {
                p.Inlines.Add(new Run { Text = CleanText(node.InnerText) });
            }
        }

        private static void AddChildren(Span s, HtmlNode node)
        {
            var added = false;

            foreach (var child in node.ChildNodes)
            {
                var i = GenerateBlockForNode(child);
                if (i != null)
                {
                    s.Inlines.Add(i);
                    added = true;
                }
            }
            if (!added)
            {
                s.Inlines.Add(new Run { Text = CleanText(node.InnerText) });
            }
        }

        private static Inline GenerateBlockForNode(HtmlNode node)
        {
            switch (node.Name)
            {
                case "p":
                case "P":
                    return GenerateInnerParagraph(node);
                case "a":
                case "A":
                    return GenerateHyperLink(node);
                case "b":
                case "B":
                case "strong":
                case "STRONG":
                    return GenerateBold(node);
                case "i":
                case "I":
                case "em":
                case "EM":
                    return GenerateItalic(node);
                case "u":
                case "U":
                    return GenerateUnderline(node);
                case "s":
                case "S":
                    return GenerateStrikethrough(node);
                case "code":
                case "CODE":
                    return GenerateMonospace(node);
                case "br":
                case "BR":
                    return new LineBreak();
                case "#text":
                    if (!string.IsNullOrEmpty(node.InnerText))
                        return new Run { Text = CleanText(node.InnerText) };
                    break;
                default:
                    return GenerateSpanWNewLine(node);
            }
            return null;
        }

        private static Inline GenerateHyperLink(HtmlNode node)
        {
            var link = new Hyperlink();
            link.NavigateUri = new Uri(node.Attributes["href"].Value, UriKind.Absolute);
            link.Inlines.Add(new Run { Text = CleanText(node.InnerText) });
            return link;
        }

        private static Inline GenerateBold(HtmlNode node)
        {
            var bold = new Bold();
            AddChildren(bold, node);
            return bold;
        }

        private static Inline GenerateUnderline(HtmlNode node)
        {
            var underline = new Underline();
            AddChildren(underline, node);
            return underline;
        }

        private static Inline GenerateStrikethrough(HtmlNode node)
        {
            var span = new Span();
            span.TextDecorations = TextDecorations.Strikethrough;
            AddChildren(span, node);
            return span;
        }

        private static Inline GenerateItalic(HtmlNode node)
        {
            var italic = new Italic();
            AddChildren(italic, node);
            return italic;
        }

        private static Inline GenerateMonospace(HtmlNode node)
        {
            var span = new Span();
            span.FontFamily = new FontFamily("Lucida Console");
            AddChildren(span, node);
            return span;
        }

        private static Block GenerateParagraph(HtmlNode node)
        {
            var paragraph = new Paragraph();
            AddChildren(paragraph, node);
            return paragraph;
        }

        private static Inline GenerateInnerParagraph(HtmlNode node)
        {
            var span = new Span();
            span.Inlines.Add(new LineBreak());
            AddChildren(span, node);
            span.Inlines.Add(new LineBreak());
            return span;
        }

        private static Inline GenerateSpanWNewLine(HtmlNode node)
        {
            var span = new Span();
            AddChildren(span, node);
            if (span.Inlines.Count > 0)
                span.Inlines.Add(new LineBreak());
            return span;
        }
    }
}