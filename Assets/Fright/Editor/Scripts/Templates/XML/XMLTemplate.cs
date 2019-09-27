using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Fright.Editor.Templates
{
	/// Describes a template made from Xml that can be used to make a C# file
	public class XmlTemplate : XmlBase
	{
		private static Dictionary<string, System.Type> xmlBaseTypes = new Dictionary<string, System.Type>()
		{
			{"function", typeof(XmlFunction)},
			{"class", typeof(XmlClass)},
		};

		public System.Version version;
		public List<XmlBase> children = new List<XmlBase>();

		public override void ConstructFromXml(XmlNode node, XmlDocument document)
		{
			base.ConstructFromXml(node, document);

			//One offs
			version = new System.Version(node.GetAttribute("version", "1.0"));

			//Children
			foreach(XmlNode child in node.ChildNodes)
			{
				XmlBase xmlBase = CreateXmlObjectFromNode(child, document);

				if (xmlBase != null)
				{
					children.Add(xmlBase);
				}
			}
		}

		/// Converts the XML object into C# and adds it to the string builder
		public override void ToCSharp(StringBuilder stringBuilder, int indentationLevel)
		{
			//Usings
			//...

			//Namespaces
			//...

			//Children
			bool isFirstChild = true;

			foreach(var child in children)
			{
				if (isFirstChild)
				{
					isFirstChild = false;
				}
				else
				{
					stringBuilder.Append("\n\n");
				}

				child.ToCSharp(stringBuilder, indentationLevel);
			}
		}

		/// Constructs an XmlBase from the node if it's type is known
		public static XmlBase CreateXmlObjectFromNode(XmlNode node, XmlDocument document)
		{
			XmlBase result = null;
			System.Type nodeType = null;

			//Instantiate the type if it's a known type
			if (xmlBaseTypes.TryGetValue(node.LocalName.ToLower(), out nodeType) && nodeType != null)
			{
				if (typeof(XmlBase).IsAssignableFrom(nodeType) && !nodeType.IsAbstract)
				{
					result = System.Activator.CreateInstance(nodeType) as XmlBase;
					result.ConstructFromXml(node, document);
				}
			}

			//Return the result
			return result;
		}
	}
}