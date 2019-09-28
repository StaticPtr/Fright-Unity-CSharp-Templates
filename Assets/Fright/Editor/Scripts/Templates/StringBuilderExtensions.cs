using System.Xml;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace Fright.Editor.Templates
{
	internal static class StringBuilderExtensions
	{
		private static readonly string[] lineEndings = new[] { "\r\n", "\n" };

		public static void AppendSpace(this StringBuilder stringBuilder)
		{
			stringBuilder.Append(' ');
		}

		public static void AppendIf(this StringBuilder stringBuilder, string text, bool condition)
		{
			if (condition)
			{
				stringBuilder.Append(text);
			}
		}

		public static void AppendWithIndentation(this StringBuilder stringBuilder, string text, int indentation)
		{
			if (!string.IsNullOrEmpty(text))
			{
				string[] lines = GetLines(text);

				for (int i = 0; i < lines.Length; ++i)
				{
					stringBuilder.AppendIndentations(indentation);
					stringBuilder.Append(lines[i]);

					if (i < lines.Length - 1)
					{
						stringBuilder.Append("\n");
					}
				}
			}
		}

		public static void AppendIndentations(this StringBuilder stringBuilder, int indentation)
		{
			//Append the indentation
			for (int j = 0; j < indentation; ++j)
			{
				stringBuilder.Append('\t');
			}
		}

		public static string[] GetLines(string text)
		{
			return text.Split(lineEndings, System.StringSplitOptions.None);
		}
	}
}