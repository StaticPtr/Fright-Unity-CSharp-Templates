using System.Xml;
using System.Collections;
using System.Collections.Generic;

namespace Fright.Editor.Templates
{
	public abstract class XMLCSharpBase : XMLBase
	{
		public string accessibility;

		public XMLCSharpBase(string id, string accessibility = null) : base(id)
		{
			this.accessibility = accessibility ?? "private";
		}
	}
}