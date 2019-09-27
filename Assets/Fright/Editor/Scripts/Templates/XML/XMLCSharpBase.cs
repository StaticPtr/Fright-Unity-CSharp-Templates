using System.Xml;
using System.Collections;
using System.Collections.Generic;

namespace Fright.Editor.Templates
{
	public abstract class XmlCSharpBase : XmlBase
	{
		public string accessibility;

		public XmlCSharpBase(string id, string accessibility = null) : base(id)
		{
			this.accessibility = accessibility ?? "private";
		}
	}
}