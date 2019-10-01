//
// MIT License
// 
// Copyright (c) 2019 Brandon Dahn
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
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
		public override void ToCSharp(StringBuilder stringBuilder, int indentationLevel, TemplateSettings settings)
		{
			//Comment
			if (comment != null)
			{
				stringBuilder.AppendWithIndentation("/// " + comment, indentationLevel);
				stringBuilder.Append("\n");
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

				if (body != null)
				{
					stringBuilder.AppendWithIndentation(body, indentationLevel + 1);
					stringBuilder.Append("\n");
				}

				stringBuilder.AppendIndentations(indentationLevel);
				stringBuilder.Append("}");
			}
		}
	}
}