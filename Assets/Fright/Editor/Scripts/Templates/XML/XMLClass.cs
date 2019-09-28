using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Fright.Editor.Templates
{
	/// Describes a class that can be used in an XML Template
	public class XmlClass : XmlType
	{
		public override string kind
		{
			get { return "class"; }
		}

		public override string xmlType
		{
			get { return "class"; }
		}
	}
}