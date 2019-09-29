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

		public string GetModifiedID(TemplateSettings settings)
		{
			string result = id;

			if (settings != null)
			{
				result = settings.ApplyReplacementsToText(result);
			}

			//Return the result
			return result;
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

		public override void ToCSharp(StringBuilder stringBuilder, int indentationLevel, TemplateSettings settings)
		{
			string modifiedID = GetModifiedID(settings);

			if (string.IsNullOrEmpty(modifiedID))
			{
				XmlTemplate.ChildrenToCSharp(stringBuilder, indentationLevel, settings, children);
			}
			else
			{
				//Start
				stringBuilder.AppendIndentations(indentationLevel);
				stringBuilder.Append("namespace ");
				stringBuilder.Append(modifiedID);
				stringBuilder.Append("\n");
				stringBuilder.AppendIndentations(indentationLevel);
				stringBuilder.Append("{\n");

				//Body
				XmlTemplate.ChildrenToCSharp(stringBuilder, indentationLevel + 1, settings, children);

				//End
				stringBuilder.Append("\n");
				stringBuilder.AppendIndentations(indentationLevel);
				stringBuilder.Append("}");
			}
		}
	}
}