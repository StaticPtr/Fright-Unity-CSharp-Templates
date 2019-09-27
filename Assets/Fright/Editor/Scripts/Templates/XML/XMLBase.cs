using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Fright.Editor.Templates
{
	/// An abstract class for all Template XML objects
	public abstract class XmlBase
	{
		/// The identifier for the XML object
		public string id;

		/// Constructs a new XMLBase
		public XmlBase(string id)
		{
			this.id = id;
		}

		/// Constructs a new XMLBase from an XmlNode and XmlDocument
		public XmlBase(XmlNode xmlNode, XmlDocument xmlDocument)
		{

		}

		/// Converts the XML object into C# and adds it to the string builder
		public abstract void ToCSharp(StringBuilder stringBuilder, ref int indentationLevel);
	}
}