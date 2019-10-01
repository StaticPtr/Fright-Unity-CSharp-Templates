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
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Fright.Editor.Templates
{
	public class TemplateSettings
	{
		private const string PERSISTENT_SETTING_LINE_ENDINGS = "lineendings";
		private const string PERSISTENT_SETTING_BUILD_OPTIONS = "buildoptions";
		private const string PERSISTENT_SETTING_OPTIONAL_USINGS = "optionusings";

		public TemplateBuilder.LineEndings lineEndings = TemplateBuilder.LineEndings.unix;
		public List<BuildOption> buildOptions = new List<BuildOption>();
		public List<OptionalUsing> optionalUsings = new List<OptionalUsing>();

		public string ApplyReplacementsToText(string text)
		{
			if (text != null)
			{
				foreach(var buildOption in buildOptions)
				{
					text = text.Replace("{" + buildOption.id + "}", buildOption.textValue);
				}
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

		/// Saves any persistent settings for the provided template
		public void SavePersistentSettings(XmlTemplate template)
		{
			EditorPrefs.SetInt(GetPersistentSettingKey(PERSISTENT_SETTING_LINE_ENDINGS, template), (int)lineEndings);
			EditorPrefs.SetString(GetPersistentSettingKey(PERSISTENT_SETTING_BUILD_OPTIONS, template), buildOptions != null ? JsonUtility.ToJson(new BuildOptionCollection(buildOptions), false) : null);
			EditorPrefs.SetString(GetPersistentSettingKey(PERSISTENT_SETTING_OPTIONAL_USINGS, template), optionalUsings != null ? JsonUtility.ToJson(new OptionalUsingCollection(optionalUsings), false) : null);
		}

		/// Recovers any persistent settings from the provided template
		public void RestorePeristentSettings(XmlTemplate template)
		{
			lineEndings = (TemplateBuilder.LineEndings)EditorPrefs.GetInt(GetPersistentSettingKey(PERSISTENT_SETTING_LINE_ENDINGS, template), (int)TemplateBuilder.LineEndings.unix);

			//Build options
			string buildOptionsString = EditorPrefs.GetString(GetPersistentSettingKey(PERSISTENT_SETTING_BUILD_OPTIONS, template));

			if (!string.IsNullOrEmpty(buildOptionsString))
			{
				try
				{
					BuildOptionCollection collection = JsonUtility.FromJson<BuildOptionCollection>(buildOptionsString);
					MergeInPersistentBuildOptions(collection);
				}
				catch {}
			}

			//Optional Usings
			string optionalUsingsString = EditorPrefs.GetString(GetPersistentSettingKey(PERSISTENT_SETTING_OPTIONAL_USINGS, template));

			if (!string.IsNullOrEmpty(optionalUsingsString))
			{
				try
				{
					OptionalUsingCollection collection = JsonUtility.FromJson<OptionalUsingCollection>(optionalUsingsString);
					MergeInPersistentOptionalUsings(collection);
				}
				catch {}
			}
		}

		private void MergeInPersistentBuildOptions(BuildOptionCollection collection)
		{
			foreach(BuildOption savedOption in collection.buildOptions)
			{
				if (savedOption.textValue != null)
				{
					BuildOption existingOption = this.buildOptions.FirstOrDefault(_opt => _opt.id == savedOption.id);

					if (existingOption != null)
					{
						existingOption.SetTextValue(savedOption.textValue);
					}
				}
			}
		}

		private void MergeInPersistentOptionalUsings(OptionalUsingCollection collection)
		{
			foreach(OptionalUsing savedOption in collection.optionalUsings)
			{
				OptionalUsing existingOption = this.optionalUsings.FirstOrDefault(_opt => _opt.id == savedOption.id);

				if (existingOption != null)
				{
					existingOption.isEnabled = savedOption.isEnabled;
				}
				else if (savedOption.isCustom)
				{
					this.optionalUsings.Add(savedOption);
				}
			}
		}

		#region Constructor
		public TemplateSettings()
		{
			//...
		}

		public TemplateSettings(XmlTemplate template)
		{
			buildOptions = new List<BuildOption>();
			optionalUsings = new List<OptionalUsing>();

			if (template != null)
			{
				buildOptions.AddRange(ConstructBuildOptionsForTemplate(template));
				optionalUsings.AddRange(ConstructOptionalUsingsForTemplate(template));
			}
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

		public static string GetPersistentSettingKey(string settingID, XmlTemplate template)
		{
			return string.Format("com.fright.templatebuilder.{0}.{1}", template.id, settingID);
		}
		#endregion

		#region Embedded Types
		[System.Serializable]
		public class OptionalUsing
		{
			public string id;
			public bool isEnabled;
			public bool isCustom;
		}

		/// This type is necessary because the Unity JsonUtility can't serialize collections directly
		[System.Serializable]
		private class BuildOptionCollection
		{
			public List<BuildOption> buildOptions;

			public BuildOptionCollection(IEnumerable<BuildOption> buildOptions)
			{
				this.buildOptions = new List<BuildOption>(buildOptions);
			}
		}

		/// This type is necessary because the Unity JsonUtility can't serialize collections directly
		[System.Serializable]
		private class OptionalUsingCollection
		{
			public List<OptionalUsing> optionalUsings;

			public OptionalUsingCollection(IEnumerable<OptionalUsing> optionalUsings)
			{
				this.optionalUsings = new List<OptionalUsing>(optionalUsings);
			}
		}
		#endregion
	}
}