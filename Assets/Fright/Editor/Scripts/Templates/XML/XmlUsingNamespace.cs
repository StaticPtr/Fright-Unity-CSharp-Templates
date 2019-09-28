using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Fright.Editor.Templates
{
	public class XmlUsingNamespace : XmlBase
	{
		public bool isOnByDefault;
		public bool isOptional;

		public override bool shouldAddLeadingNewline
		{
			get { return false; }
		}

		public override void ConstructFromXml(XmlNode node, XmlDocument document)
		{
			base.ConstructFromXml(node, document);
			isOnByDefault = node.GetAttribute<bool>("onByDefault");
			isOptional = node.GetAttribute<bool>("optional");
		}

		public override void ToCSharp(StringBuilder stringBuilder, int indentationLevel)
		{
			stringBuilder.Append("using ");
			stringBuilder.Append(id);
			stringBuilder.Append(";");
		}
	}
}