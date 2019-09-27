using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Text;
using System.Collections;
using Fright.Editor.Templates;

namespace Fright.Editor.Tests
{
	public class Functions
	{
		[Test]
		public void ToCSharp_VoidVoid()
		{
			XMLFunction function = new XMLFunction("TestFunction");

			string generatedCode = SerializeToCode(function);
			string expectedCode = GetExpectedResult("VoidVoid");
			Assert.AreEqual(expectedCode, generatedCode);
		}

		[Test]
		public void ToCSharp_VoidVoidWithBody()
		{
			XMLFunction function = new XMLFunction("TestFunction");
			function.body = "int x = 5;\nint y = 6;";

			string generatedCode = SerializeToCode(function);
			string expectedCode = GetExpectedResult("VoidVoidWithBody");
			Assert.AreEqual(expectedCode, generatedCode);
		}

		[Test]
		public void ToCSharp_Complex()
		{
			XMLFunction function = new XMLFunction("Add", "public");
			function.comment = "Adds two numbers together";
			function.body = "return lhs + rhs;";
			function.virtuality = XMLFunction.Virtuality.@override;
			function.returnType = "int";
			function.arguments.Add(new XMLArgument("lhs", "int"));
			function.arguments.Add(new XMLArgument("rhs", "int"));

			string generatedCode = SerializeToCode(function);
			string expectedCode = GetExpectedResult("Complex");
			Assert.AreEqual(expectedCode, generatedCode);
		}

		private string SerializeToCode(XMLCSharpBase xmlCSharp)
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
	}
}