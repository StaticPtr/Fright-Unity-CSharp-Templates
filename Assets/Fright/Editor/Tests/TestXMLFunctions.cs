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
		[Test]
		public void ToCSharp_VoidVoid()
		{
			RunXmlToCodeTest("VoidVoid");
		}

		[Test]
		public void ToCSharp_VoidVoidWithBody()
		{
			RunXmlToCodeTest("VoidVoidWithBody");
		}

		[Test]
		public void ToCSharp_Complex()
		{
			RunXmlToCodeTest("Complex");
		}

		private void RunXmlToCodeTest(string id)
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
			int indentation = 0;
			xmlCSharp.ToCSharp(generatedCode, ref indentation);
			return generatedCode.ToString().Replace("\r\n", "\n");
		}

		private string GetExpectedResult(string id)
		{
			return System.IO.File.ReadAllText("Assets/Fright/Editor/Tests/Test Expectations/" + id);
		}

		private string GetXml(string id)
		{
			return System.IO.File.ReadAllText("Assets/Fright/Editor/Tests/Test XML/" + id + ".xml");
		}
	}
}