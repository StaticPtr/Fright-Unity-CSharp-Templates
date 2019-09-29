using System.IO;
using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Fright.Editor.Templates
{
	public class XmlBuildOption : XmlBase
	{
		public string name;
		public string type;
		public string @default;

		public override string xmlType
		{
			get { return "build-option"; }
		}

		public override bool shouldUse
		{
			get { return false; }
		}

		/// Converts the XML object into C# and adds it to the string builder
		public override void ToCSharp(StringBuilder stringBuilder, int indentationLevel, TemplateSettings settings)
		{
			throw new System.NotSupportedException();
		}

		/// Constructs the object from an Xml node and document
		public override void ConstructFromXml(XmlNode node, XmlDocument document)
		{
			base.ConstructFromXml(node, document);
			name = node.GetAttribute("name");
			type = node.GetAttribute("type", "string");
			id = node.GetAttribute("replacement", name.ToLower());
			@default = node.GetAttribute("default");
		}
	}
}