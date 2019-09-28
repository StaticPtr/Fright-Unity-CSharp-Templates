using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Fright.Editor.Templates
{
	public class XmlCodeblock : XmlBase
	{
		public string body;

		public override string xmlType
		{
			get { return "codeblock"; }
		}

		public override void ConstructFromXml(XmlNode node, XmlDocument document)
		{
			base.ConstructFromXml(node, document);
			
			if (node is XmlText)
			{
				body = node.InnerText.Trim();
			}
			else
			{
				foreach(XmlNode child in node.ChildNodes)
				{
					if (child is XmlText)
					{
						body = child.InnerText.Trim();
					}
				}
			}
		}

		public override void ToCSharp(StringBuilder stringBuilder, int indentationLevel)
		{
			stringBuilder.AppendWithIndentation(body, indentationLevel);
		}
	}
}