//
// MIT License
// 
// Copyright (c) 2019 Brandon Dahn
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fright.Editor.Templates
{
	public class TemplateSettings
	{
		public TemplateBuilder.LineEndings lineEndings = TemplateBuilder.LineEndings.unix;
		public List<BuildOption> buildOptions = new List<BuildOption>();
		public List<OptionalUsing> optionalUsings = new List<OptionalUsing>();

		public void ChangeForNewTemplate(XmlTemplate template)
		{
			buildOptions = new List<BuildOption>();
			optionalUsings = new List<OptionalUsing>();

			if (template != null)
			{
				buildOptions.AddRange(ConstructBuildOptionsForTemplate(template));
				optionalUsings.AddRange(ConstructOptionalUsingsForTemplate(template));
			}
		}

		public string ApplyReplacementsToText(string text)
		{
			foreach(var buildOption in buildOptions)
			{
				text = text.Replace("{" + buildOption.id + "}", buildOption.textValue);
			}

			return text;
		}

		public bool IsOptionalUsingEnabled(string optionalUsingID)
		{
			bool result = true;

			//Find the first optional using with the provided ID
			for(int i = 0; i < optionalUsings.Count; ++i)
			{
				OptionalUsing optionalUsing = optionalUsings[i];

				if (optionalUsing.id == optionalUsingID)
				{
					result = optionalUsing.isEnabled;
					break;
				}
			}

			//Return the result
			return result;
		}

		public string GetBuildOptionValue(string buildOptionID)
		{
			string result = null;

			//Find the first build option with the provided ID
			for(int i = 0; i < buildOptions.Count; ++i)
			{
				BuildOption buildOption = buildOptions[i];

				if (buildOption.id == buildOptionID)
				{
					result = buildOption.textValue;
					break;
				}
			}

			//Return the result
			return result;
		}

		#region Constructor
		public TemplateSettings()
		{
			//...
		}

		public TemplateSettings(XmlTemplate template)
		{
			ChangeForNewTemplate(template);
		}
		#endregion

		#region Statics
		public static IEnumerable<BuildOption> ConstructBuildOptionsForTemplate(XmlTemplate template)
		{
			foreach(XmlBuildOption xmlBuildOption in template.buildOptions)
			{
				switch(xmlBuildOption.type.ToLower())
				{
					case "string":
					case "text":
						yield return new BuildOption(xmlBuildOption);
						break;

					case "int":
						yield return new IntBuildOption(xmlBuildOption);
						break;

					case "float":
					case "double":
						yield return new FloatBuildOption(xmlBuildOption);
						break;

					case "bool":
					case "boolean":
						yield return new BoolBuildOption(xmlBuildOption);
						break;
				}
			}
		}

		public static IEnumerable<OptionalUsing> ConstructOptionalUsingsForTemplate(XmlTemplate template)
		{
			foreach(XmlUsingNamespace xmlUsing in template.usings)
			{
				if (xmlUsing.isOptional)
				{
					yield return new OptionalUsing()
					{
						id = xmlUsing.id,
						isEnabled = xmlUsing.isOnByDefault,
						isCustom = false,
					};
				}
			}
		}
		#endregion

		#region Embedded Types
		public class OptionalUsing
		{
			public string id;
			public bool isEnabled;
			public bool isCustom;
		}
		#endregion
	}
}