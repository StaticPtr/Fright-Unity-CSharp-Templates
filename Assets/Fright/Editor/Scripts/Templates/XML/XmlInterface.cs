﻿using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Fright.Editor.Templates
{
	public class XmlInterace : XmlBase
	{
		public XmlInterace(XmlNode node, XmlDocument document)
		{
			ConstructFromXml(node, document);
		}

		public override void ToCSharp(StringBuilder stringBuilder, int indentationLevel)
		{
			throw new System.NotImplementedException();
		}
	}
}