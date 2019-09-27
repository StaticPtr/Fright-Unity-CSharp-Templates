using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Fright.Editor.Templates
{
	/// An abstract class for all Template XML objects
	public abstract class XMLBase
	{
		/// The identifier for the XML object
		public string id;

		/// Constructs a new XMLBase
		public XMLBase(string id)
		{
			this.id = id;
		}

		/// Converts the XML object into C# and adds it to the string builder
		public abstract void ToCSharp(StringBuilder stringBuilder, ref int indentationLevel);
	}
}