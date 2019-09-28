using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fright.Editor.Templates
{
	[System.Serializable]
	public class TemplateBuilderSettings
	{
		public TemplateBuilder.LineEndings lineEndings = TemplateBuilder.LineEndings.unix;
		public List<BuildOption> buildOptions = new List<BuildOption>();

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
	}
}