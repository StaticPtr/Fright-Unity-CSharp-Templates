using System.Xml;
using System.Collections;
using System.Collections.Generic;

namespace Fright.Editor.Templates
{
	public abstract class XmlCSharpBase : XmlBase
	{
		public string accessibility;

		public override void ConstructFromXml(XmlNode node, XmlDocument document)
		{
			base.ConstructFromXml(node, document);
			accessibility = node.GetAttribute("access", "private");
		}
	}
}