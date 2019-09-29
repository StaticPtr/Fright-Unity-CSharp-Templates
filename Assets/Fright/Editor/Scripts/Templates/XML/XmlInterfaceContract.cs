using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Fright.Editor.Templates
{
	public class XmlInterfaceContract : XmlBase
	{
		public override string xmlType
		{
			get { return "interface-contract"; }
		}

		public override void ToCSharp(StringBuilder stringBuilder, int indentationLevel, TemplateSettings settings)
		{
			throw new System.NotImplementedException();
		}
	}
}