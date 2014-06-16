using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace QppFacade
{
    public static class XElementExtensions
    {
        public static XDocument ToXDocument(this string str)
        {
            return XDocument.Parse(str);
        }

        public static XDocument ToXDocument(this XElement xElement)
        {
            return XDocument.Parse(xElement.ToString());
        }

        /// <summary>
        ///     Get the absolute XPath to a given XElement
        ///     (e.g. "/people/person[6]/name[1]/last[1]").
        /// </summary>
        /// <param name="element">
        ///     The element to get the index of.
        /// </param>
        public static string AbsoluteXPath(this XElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            Func<XElement, string> relativeXPath = e =>
            {
                var index = e.IndexPosition();
                var name = e.Name.LocalName;

                // If the element is the root, no index is required

                return (index == -1)
                    ? "/" + String.Format("{0}[1]", name)
                    : String.Format
                        (
                            "/{0}[{1}]",
                            name,
                            index.ToString()
                        );
            };

            var ancestors = from e in element.Ancestors()
                            select relativeXPath(e);

            return String.Concat(ancestors.Reverse().ToArray()) +
                   relativeXPath(element);
        }

        /// <summary>
        ///     Get the index of the given XElement relative to its
        ///     siblings with identical names. If the given element is
        ///     the root, -1 is returned.
        /// </summary>
        /// <param name="element">
        ///     The element to get the index of.
        /// </param>
        public static int IndexPosition(this XElement element)
        {
            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            if (element.Parent == null)
            {
                return -1;
            }

            var i = 1; // Indexes for nodes start at 1, not 0

            foreach (var sibling in element.Parent.Elements(element.Name))
            {
                if (sibling == element)
                {
                    return i;
                }

                i++;
            }

            throw new InvalidOperationException
                ("element has been removed from its parent.");
        }

        public static bool AttributeValueIs(this XElement element, string attrName, string value)
        {
            var attribute = element.Attributes(attrName).ToList();
            return attribute.Any() && attribute.Single().Value == value;
        }

        public static string InnerXml(this XElement parentOfBr)
        {
            return String.Concat(parentOfBr.Nodes());
        }

        public static bool HasNoChildElements(this XElement e)
        {
            return !e.Elements().Any();
        }

        public static bool HasAttribute(this XElement t, string attribute)
        {
            return t.Attributes(attribute).Any();
        }

        public static IEnumerable<XAttribute> AllAttributes(this XDocument doc, params string[] attrNames)
        {
            return attrNames.SelectMany(attrName => doc.Descendants().Where(e => e.Attributes(attrName).Any()).Select(e => e.Attribute(attrName)));
        }

        public static int Depth(this XContainer currentTopicRefNode)
        {
            if (currentTopicRefNode.Parent == null)
                return 1;
            return 1 + currentTopicRefNode.Parent.Depth();
        }

        public static IEnumerable<XElement> SkipFirst(this IEnumerable<XElement> elements)
        {
            return elements.Skip(1);
        }
    }
}