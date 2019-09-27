﻿using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Fright.Editor.Templates
{
	public static class XmlNodeExtensions
	{
		public static string GetAttribute(this XmlNode node, string attributeName, string fallback = null)
		{
			string result = fallback;

			//Check if this node even has attribute
			if (node.Attributes != null)
			{
				var attribute = node.Attributes.GetNamedItem(attributeName);
				result = attribute != null ? attribute.Value : fallback;
			}

			/// Return the result
			return result;
		}

		public static TEnum GetEnumAttribute<TEnum>(this XmlNode node, string attributeName, TEnum fallback = default(TEnum))
		{
			TEnum result = fallback;
			string stringValue = node.GetAttribute(attributeName);

			//Check if there is any value at all
			if (stringValue != null)
			{
				try
				{
					result = (TEnum)System.Enum.Parse(typeof(TEnum), stringValue, true);
				}
				catch
				{
					result = fallback;
				}
			}

			//Return the result
			return result;
		}

		public static XmlNode GetFirstChild(this XmlDocument document, string nodeName)
		{
			XmlNode result = null;

			foreach(XmlNode node in document.ChildNodes)
			{
				if (node.LocalName.Equals(nodeName, System.StringComparison.InvariantCultureIgnoreCase))
				{
					result = node;
					break;
				}
			}

			return result;
		}
	}
}