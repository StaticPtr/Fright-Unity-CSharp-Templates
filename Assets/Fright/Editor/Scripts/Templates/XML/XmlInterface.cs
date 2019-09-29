using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Fright.Editor.Templates
{
	/// Describes an interface that can be used in an XML Template
	public class XmlInterface : XmlType
	{
		public override string kind
		{
			get { return "interface"; }
		}

		public override string xmlType
		{
			get { return "interface"; }
		}

		public override void ConstructFromXml(XmlNode node, XmlDocument document)
		{
			base.ConstructFromXml(node, document);
			isSealed = false;
			isStatic = false;
			isAbstract = false;
			isPartial = false;
		}
	}
}