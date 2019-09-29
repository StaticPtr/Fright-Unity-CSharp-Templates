using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Text;
using System.Xml;
using System.Collections;
using Fright.Editor.Templates;

namespace Fright.Editor.Tests
{
	public class Functions
	{
		[TestCase("VoidVoid")]
		[TestCase("VoidVoidWithBody")]
		[TestCase("Complex")]
		public void RunXmlToCodeTest(string id)
		{
			string xml = GetXml(id);
			XmlDocument document = new XmlDocument();
			document.LoadXml(xml);

			XmlFunction function = new XmlFunction();
			function.ConstructFromXml(document.GetFirstChild("function"), document);

			string generatedCode = SerializeToCode(function);
			string expectedCode = GetExpectedResult(id);
			Assert.AreEqual(expectedCode, generatedCode);
		}

		private string SerializeToCode(XmlCSharpBase xmlCSharp)
		{
			StringBuilder generatedCode = new StringBuilder();
			xmlCSharp.ToCSharp(generatedCode, 0, null);
			return generatedCode.ToString().Replace("\r\n", "\n");
		}

		private string GetExpectedResult(string id)
		{
			return System.IO.File.ReadAllText("Assets/Fright/Editor/Tests/Test Expectations/Function_" + id);
		}

		private string GetXml(string id)
		{
			return System.IO.File.ReadAllText("Assets/Fright/Editor/Tests/Test XML/Function_" + id + ".xml");
		}
	}
}