using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Fright.Editor.Templates
{
	public struct XmlArgument
	{
		public string id;
		public string type;

		public XmlArgument(string id, string type)
		{
			this.id = id;
			this.type = type;
		}
	}
}