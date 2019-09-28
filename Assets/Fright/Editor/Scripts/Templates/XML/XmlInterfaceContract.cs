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

		public XmlInterfaceContract(XmlNode node, XmlDocument document)
		{
			ConstructFromXml(node, document);
		}

		public override void ToCSharp(StringBuilder stringBuilder, int indentationLevel)
		{
			throw new System.NotImplementedException();
		}
	}
}