using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fright.Editor.Templates
{
	public class TemplateBuilderSettings
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
		public TemplateBuilderSettings()
		{
			//...
		}

		public TemplateBuilderSettings(XmlTemplate template)
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