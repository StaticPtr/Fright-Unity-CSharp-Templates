using System.IO;
using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fright.Editor.Templates
{
	public static class TemplateBuilder
	{
		/// 4 Kibibytes (4096 bytes)
		public const int _4KiB = 4096;
		public const string WINDOWS_LINE_ENDINGS = "\r\n";
		public const string UNIX_LINE_ENDINGS = "\n";

		public static string BuildCodeFromTemplate(XmlTemplate template, TemplateBuilderSettings settings)
		{
			//Create the code
			StringBuilder codeBuilder = new StringBuilder(_4KiB);
			template.ToCSharp(codeBuilder, 0, settings);

			//Transform the code
			string code = NormalizeLineEndings(codeBuilder.ToString(), settings.lineEndings);

			foreach(var xmlBuildOption in template.buildOptions)
			{
				string value = settings.GetBuildOptionValue(xmlBuildOption.id) ?? xmlBuildOption.@default ?? "(null)";
				code = code.Replace("{" + xmlBuildOption.id + "}", value);
			}

			//Return the result
			return code;
		}

		public static IEnumerable<XmlTemplate> FindAllTemplatesInProject()
		{
			const string TEMPLATE_EXTENSION = ".xtemplate";
			var files = Directory.GetFiles(Application.dataPath + "/", "*" + TEMPLATE_EXTENSION, SearchOption.AllDirectories);

			foreach(string filePath in files)
			{
				if (Path.GetExtension(filePath) == TEMPLATE_EXTENSION)
				{
					yield return XmlTemplate.FromFile(filePath);
				}
			}
		}

		/// Changes all of the line endings in the provided text into the specified format
		public static string NormalizeLineEndings(string textToFix, LineEndings lineEndings)
		{
			//Transform all the line endings into Unix so it's easier to manage in the next step
			textToFix =  textToFix.Replace(WINDOWS_LINE_ENDINGS, UNIX_LINE_ENDINGS);

			//Perform the final transformation
			switch(lineEndings)
			{
				case LineEndings.windows:
					textToFix = textToFix.Replace(UNIX_LINE_ENDINGS, WINDOWS_LINE_ENDINGS);
					break;

				case LineEndings.unix:
					//Do nothing
					break;
			}

			//Return the result
			return textToFix;
		}

		#region Embedded Types
		public enum LineEndings
		{
			/// Microsoft Windows \r\n
			windows,
			/// Unix and macOS \n
			unix,
		}
		#endregion
	}
}