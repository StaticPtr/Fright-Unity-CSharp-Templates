using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Fright.Editor.Templates
{
	public class BuildOption
	{
		public string id;
		public string name;
		public string textValue;

		public BuildOption(XmlBuildOption xmlBuildOption)
		{
			id = xmlBuildOption.id;
			name = xmlBuildOption.name;
			textValue = xmlBuildOption.@default;
		}

		public virtual void DoLayout()
		{
			textValue = EditorGUILayout.TextField(name, textValue);
		}
	}

	public class IntBuildOption : BuildOption
	{
		public int intValue;

		public IntBuildOption(XmlBuildOption xmlBuildOption) : base(xmlBuildOption)
		{
			int.TryParse(textValue, out intValue);
		}

		public override void DoLayout()
		{
			EditorGUI.BeginChangeCheck();
			{
				intValue = EditorGUILayout.IntField(name, intValue);
			}
			if (EditorGUI.EndChangeCheck())
			{
				textValue = intValue.ToString();
			}
		}
	}

	public class FloatBuildOption : BuildOption
	{
		public float floatValue;

		public FloatBuildOption(XmlBuildOption xmlBuildOption) : base(xmlBuildOption)
		{
			float.TryParse(textValue, out floatValue);
		}

		public override void DoLayout()
		{
			EditorGUI.BeginChangeCheck();
			{
				floatValue = EditorGUILayout.FloatField(name, floatValue);
			}
			if (EditorGUI.EndChangeCheck())
			{
				textValue = floatValue.ToString();
			}
		}
	}

	public class BoolBuildOption : BuildOption
	{
		public bool boolValue;

		public BoolBuildOption(XmlBuildOption xmlBuildOption) : base(xmlBuildOption)
		{
			bool.TryParse(textValue, out boolValue);
		}

		public override void DoLayout()
		{
			EditorGUI.BeginChangeCheck();
			{
				boolValue = EditorGUILayout.Toggle(name, boolValue);
			}
			if (EditorGUI.EndChangeCheck())
			{
				textValue = boolValue.ToString();
			}
		}
	}
}