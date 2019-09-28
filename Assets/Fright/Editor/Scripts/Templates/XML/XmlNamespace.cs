using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Fright.Editor.Templates
{
	public class XmlNamespace : XmlBase
	{
		public List<XmlBase> children = new List<XmlBase>();

		public override string xmlType
		{
			get { return "namespace"; }
		}

		public override void ConstructFromXml(XmlNode node, XmlDocument document)
		{
			base.ConstructFromXml(node, document);

			//Children
			foreach (XmlNode child in node.ChildNodes)
			{
				XmlBase xmlBase = XmlTemplate.CreateXmlObjectFromNode(child, document);

				if (xmlBase != null)
				{
					children.Add(xmlBase);
				}
			}
		}

		public override void ToCSharp(StringBuilder stringBuilder, int indentationLevel)
		{
			//Start
			stringBuilder.AppendIndentations(indentationLevel);
			stringBuilder.Append("namespace ");
			stringBuilder.Append(id);
			stringBuilder.Append("\n");
			stringBuilder.AppendIndentations(indentationLevel);
			stringBuilder.Append("{\n");

			//Body
			XmlTemplate.ChildrenToCSharp(stringBuilder, indentationLevel + 1, children);

			//End
			stringBuilder.Append("\n");
			stringBuilder.AppendIndentations(indentationLevel);
			stringBuilder.Append("}");
		}
	}
}