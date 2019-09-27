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

		/// Constructs a new XMLFunction
		public XmlFunction(string id, string accessibility = null) : base(id, accessibility)
		{
			//...
		}

		/// Converts the XML object into C# and adds it to the string builder
		public override void ToCSharp(StringBuilder stringBuilder, ref int indentationLevel)
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