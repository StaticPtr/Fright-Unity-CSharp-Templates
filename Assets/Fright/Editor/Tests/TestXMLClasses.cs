using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Text;
using System.Xml;
using System.Collections;
using Fright.Editor.Templates;

namespace Fright.Editor.Tests
{
	public class Classes
	{
		[TestCase("Empty")]
		[TestCase("Base")]
		[TestCase("OnlyInterfaces")]
		[TestCase("BaseAndInterfaces")]
		[TestCase("Complex")]
		public void RunXmlToCodeTest(string id)
		{
			string xml = GetXml(id);
			XmlDocument document = new XmlDocument();
			document.LoadXml(xml);

			XmlClass @class = new XmlClass();
			@class.ConstructFromXml(document.GetFirstChild("class"), document);

			string generatedCode = SerializeToCode(@class);
			string expectedCode = GetExpectedResult(id);
			Assert.AreEqual(expectedCode, generatedCode);
		}

		private string SerializeToCode(XmlCSharpBase xmlCSharp)
		{
			StringBuilder generatedCode = new StringBuilder();
			xmlCSharp.ToCSharp(generatedCode, 0);
			return generatedCode.ToString().Replace("\r\n", "\n");
		}

		private string GetExpectedResult(string id)
		{
			return System.IO.File.ReadAllText("Assets/Fright/Editor/Tests/Test Expectations/Class_" + id);
		}

		private string GetXml(string id)
		{
			return System.IO.File.ReadAllText("Assets/Fright/Editor/Tests/Test XML/Class_" + id + ".xml");
		}
	}
}