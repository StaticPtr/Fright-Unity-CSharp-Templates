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
		/// The XML tag that this object comes from
		public abstract string xmlType { get; }

		/// Should an extra new-line be added before this object if there is another object
		public virtual bool shouldAddLeadingNewline
		{
			get { return true; }
		}

		/// Converts the XML object into C# and adds it to the string builder
		public abstract void ToCSharp(StringBuilder stringBuilder, int indentationLevel);

		/// Constructs the object from an Xml node and document
		public virtual void ConstructFromXml(XmlNode node, XmlDocument document)
		{
			id = node.GetAttribute("id");
		}
	}
}