using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Fright.Editor.Templates
{
	/// Describes a function that can be used in an XML Template
	public class XmlFunction : XmlCSharpBase
	{
		public List<XmlArgument> arguments = new List<XmlArgument>();
		public Virtuality virtuality = Virtuality.none;
		public bool isStatic;
		public bool isSealed;
		public string returnType;
		public string body;
		public string comment;

		public override string xmlType
		{
			get { return "function"; }
		}

		public override void ConstructFromXml(XmlNode node, XmlDocument document)
		{
			base.ConstructFromXml(node, document);

			//One offs
			isSealed = node.GetAttribute("sealed", "false").Equals("true", System.StringComparison.InvariantCultureIgnoreCase);
			isStatic = node.GetAttribute("static", "false").Equals("true", System.StringComparison.InvariantCultureIgnoreCase);
			virtuality = node.GetEnumAttribute<Virtuality>("virtuality", Virtuality.none);
			returnType = node.GetAttribute("returnType", "void");
			comment = node.GetAttribute("comment");

			//Children
			foreach(XmlNode child in node.ChildNodes)
			{
				if (child is XmlText)
				{
					body = child.InnerText.Trim();
				}
				else if (child.LocalName.Equals("argument", System.StringComparison.InvariantCultureIgnoreCase))
				{
					arguments.Add(new XmlArgument(child, document));
				}
			}
		}

		/// Converts the XML object into C# and adds it to the string builder
		public override void ToCSharp(StringBuilder stringBuilder, int indentationLevel)
		{
			//Comment
			if (comment != null)
			{
				stringBuilder.AppendWithIndentation("/// " + comment, indentationLevel);
			}

			//Signature
			stringBuilder.AppendIndentations(indentationLevel);
			stringBuilder.Append(accessibility);
			stringBuilder.AppendSpace();
			stringBuilder.AppendIf("static ", isStatic);
			stringBuilder.AppendIf("sealed ", isSealed);

			if (virtuality != Virtuality.none)
			{
				stringBuilder.Append(virtuality.ToString().Replace("@", null));
				stringBuilder.AppendSpace();
			}

			stringBuilder.Append(returnType ?? "void");
			stringBuilder.AppendSpace();	
			stringBuilder.Append(id);

			//Arguments
			stringBuilder.Append("(");

			for(int i = 0; i < arguments.Count; ++i)
			{
				XmlArgument argument = arguments[i];

				stringBuilder.Append(argument.type ?? "?");
				stringBuilder.AppendSpace();
				stringBuilder.Append(argument.id ?? "?");

				if (i < arguments.Count - 1)
				{
					stringBuilder.Append(", ");
				}
			}

			stringBuilder.Append(")");


			//Body
			if (virtuality == Virtuality.@abstract)
			{
				stringBuilder.Append(";\n");
			}
			else
			{
				stringBuilder.Append("\n");
				stringBuilder.AppendIndentations(indentationLevel);
				stringBuilder.Append("{\n");
				stringBuilder.AppendWithIndentation(body, indentationLevel + 1);
				stringBuilder.AppendIndentations(indentationLevel);
				stringBuilder.Append("}");
			}
		}

		/// Describes how a function is virtualized
		public enum Virtuality
		{
			none,
			@abstract,
			@virtual,
			@override,
		}
	}
}