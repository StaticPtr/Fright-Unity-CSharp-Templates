using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Text;
using System.Xml;
using System.Collections;
using Fright.Editor.Templates;

namespace Fright.Editor.Tests
{
	public class Templates
	{
		[TestCase("Complex")]
		[TestCase("WithUsings")]
		public void RunXmlToCodeTest(string id)
		{
			string xml = GetXml(id);
			XmlDocument document = new XmlDocument();
			document.LoadXml(xml);

			XmlTemplate template = new XmlTemplate();
			template.ConstructFromXml(document.GetFirstChild("template"), document);

			string generatedCode = SerializeToCode(template);
			string expectedCode = GetExpectedResult(id);
			Assert.AreEqual(expectedCode, generatedCode);
		}

		private string SerializeToCode(XmlBase xmlBase)
		{
			StringBuilder generatedCode = new StringBuilder();
			xmlBase.ToCSharp(generatedCode, 0);
			return generatedCode.ToString().Replace("\r\n", "\n");
		}

		private string GetExpectedResult(string id)
		{
			return System.IO.File.ReadAllText("Assets/Fright/Editor/Tests/Test Expectations/Template_" + id);
		}

		private string GetXml(string id)
		{
			return System.IO.File.ReadAllText("Assets/Fright/Editor/Tests/Test XML/Template_" + id + ".xml");
		}
	}
}