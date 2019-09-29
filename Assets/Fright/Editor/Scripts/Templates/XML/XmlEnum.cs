using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Fright.Editor.Templates
{
	/// Describes an enum that can be used in an XML Template
	public class XmlEnum : XmlType
	{
		public override string kind
		{
			get { return "enum"; }
		}

		public override string xmlType
		{
			get { return "enum"; }
		}

		public override void ConstructFromXml(XmlNode node, XmlDocument document)
		{
			base.ConstructFromXml(node, document);
			isSealed = false;
			isStatic = false;
			isAbstract = false;
			isPartial = false;
			interfaces.Clear();
		}
	}
}