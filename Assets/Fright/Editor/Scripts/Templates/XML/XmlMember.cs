using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Fright.Editor.Templates
{
	/// Describes a type that can be used in an XML Template
	public class XmlMember : XmlCSharpBase
	{
		public string type;
		public bool isStatic;

		public override string xmlType
		{
			get { return "member"; }
		}

		public override bool shouldAddLeadingNewline
		{
			get { return false; }
		}

		public override void ConstructFromXml(XmlNode node, XmlDocument document)
		{
			base.ConstructFromXml(node, document);
			type = node.GetAttribute("type");
			isStatic = node.GetAttribute<bool>("static");
		}

		public override void ToCSharp(StringBuilder stringBuilder, int indentationLevel)
		{
			stringBuilder.AppendIndentations(indentationLevel);
			stringBuilder.Append(accessibility);
			stringBuilder.AppendSpace();
			stringBuilder.AppendIf("static ", isStatic);
			stringBuilder.Append(type ?? "?");
			stringBuilder.AppendSpace();
			stringBuilder.Append(id);
			stringBuilder.Append(';');
		}
	}
}