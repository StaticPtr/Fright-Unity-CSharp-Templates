﻿using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Fright.Editor.Templates
{
	public struct XMLArgument
	{
		public string id;
		public string type;

		public XMLArgument(string id, string type)
		{
			this.id = id;
			this.type = type;
		}
	}
}