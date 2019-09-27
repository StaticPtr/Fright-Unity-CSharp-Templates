using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Fright.Editor.Templates
{
	/// Describes a structure that can be used in an XML Template
	public class XmlStruct : XmlType
	{
		public override string kind
		{
			get { return "struct"; }
		}
	}
}