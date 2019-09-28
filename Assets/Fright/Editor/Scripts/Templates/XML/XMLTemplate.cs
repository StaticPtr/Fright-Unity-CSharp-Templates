using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Fright.Editor.Templates
{
	/// Describes a template made from Xml that can be used to make a C# file
	public class XmlTemplate : XmlBase
	{
		private static Dictionary<string, System.Type> _xmlBaseTypes;

		public System.Version version;
		public List<XmlUsingNamespace> usings = new List<XmlUsingNamespace>();
		public List<XmlBase> children = new List<XmlBase>();

		private static Dictionary<string, System.Type> xmlBaseTypes
		{
			get
			{
				if (_xmlBaseTypes == null)
				{
					FindAllTagTypes();
				}
				return _xmlBaseTypes;
			}
		}

		public IEnumerable<XmlBase> allChildren
		{
			get
			{
				foreach(var @using in usings)
				{
					yield return @using;
				}

				foreach(var child in children)
				{
					yield return child;
				}
			}
		}

		public override string xmlType
		{
			get { return "template"; }
		}

		public override void ConstructFromXml(XmlNode node, XmlDocument document)
		{
			base.ConstructFromXml(node, document);

			//One offs
			version = new System.Version(node.GetAttribute("version", "1.0"));

			//Children
			foreach(XmlNode child in node.ChildNodes)
			{
				XmlBase xmlBase = CreateXmlObjectFromNode(child, document);

				if (xmlBase != null)
				{
					children.Add(xmlBase);
				}
			}
		}

		/// Converts the XML object into C# and adds it to the string builder
		public override void ToCSharp(StringBuilder stringBuilder, int indentationLevel)
		{
			ChildrenToCSharp(stringBuilder, indentationLevel, allChildren);
		}

		/// Converts multiple XmlBase objects into C#
		public static void ChildrenToCSharp(StringBuilder stringBuilder, int indentationLevel, IEnumerable<XmlBase> children)
		{
			bool isFirstChild = true;

			foreach (XmlBase child in children)
			{
				if (isFirstChild)
				{
					isFirstChild = false;
				}
				else
				{
					stringBuilder.Append(child.shouldAddLeadingNewline ? "\n\n" : "\n");
				}

				child.ToCSharp(stringBuilder, indentationLevel);
			}
		}

		/// Constructs an XmlBase from the node if it's type is known
		public static XmlBase CreateXmlObjectFromNode(XmlNode node, XmlDocument document)
		{
			XmlBase result = null;
			System.Type nodeType = null;

			//Instantiate the type if it's a known type
			if (xmlBaseTypes.TryGetValue(node.LocalName.ToLower(), out nodeType) && nodeType != null)
			{
				if (typeof(XmlBase).IsAssignableFrom(nodeType) && !nodeType.IsAbstract)
				{
					result = System.Activator.CreateInstance(nodeType) as XmlBase;
					result.ConstructFromXml(node, document);
				}
			}

			//Return the result
			return result;
		}

		private static void FindAllTagTypes()
		{
			_xmlBaseTypes = new Dictionary<string, System.Type>();

			//Find all the XmlBase types in any of the assemblies
			foreach(Assembly assembly in System.AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach(System.Type type in assembly.GetExportedTypes())
				{
					if (!type.IsAbstract && !type.IsGenericType && typeof(XmlBase).IsAssignableFrom(type))
					{
						try
						{
							XmlBase obj = System.Activator.CreateInstance(type) as XmlBase;
							xmlBaseTypes[obj.xmlType.ToLower()] = type;
						}
						catch (System.Exception e)
						{
							UnityEngine.Debug.LogException(e);
						}
					}
				}
			}
		}
	}
}