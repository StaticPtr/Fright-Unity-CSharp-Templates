using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Fright.Editor.Templates
{
	/// An editor window for creating C# scripts from template files
	public class TemplateBuilderWindow : EditorWindow
	{
		private const float MIN_WIDTH = 450.0f;
		private const float MIN_HEIGHT = 400.0f;
		private const float SETTINGS_PANEL_WIDTH = 300.0f;

		[SerializeField] private string lastKnownCreationPath;
		[SerializeField] private Rect sharedWindowPosition = new Rect(Vector2.zero, new Vector2(700.0f, 400.0f));
		[SerializeField] private Vector2 templatePreviewScrollPos = Vector2.zero;
		
		private TemplateBuilderSettings templateSettings = new TemplateBuilderSettings();
		private List<XmlTemplate> templates = null;
		private XmlTemplate template = null;
		private string codePreview = null;

		/// The path to the currently selected folder in the project view
		public static string templateCreatePath
		{
			get
			{
				string result = null;
				Object selectedObject = Selection.activeObject;
				string selectedObjectPath = selectedObject ? AssetDatabase.GetAssetPath(selectedObject) : null;

				//If there is a selected object then grab the folder that it's in
				if (!string.IsNullOrEmpty(selectedObjectPath))
				{
					if (Directory.Exists(selectedObjectPath))
					{
						result = selectedObjectPath;
					}
					else
					{
						result = Path.GetDirectoryName(selectedObjectPath);
					}
				}

				//Return the result
				return result;
			}
		}

		/// Checks if the template builder window can be opened
		[MenuItem("Assets/Template Window", true)]
		public static bool CanOpenTemplateBuilder()
		{
			return !string.IsNullOrEmpty(templateCreatePath);
		}

		/// Opens the template builder window
		[MenuItem("Assets/Template Window", false, -1000)]
		public static void OpenTemplateBuilder()
		{
			TemplateBuilderWindow window = EditorWindow.CreateInstance<TemplateBuilderWindow>();
			window.titleContent.text = "C# Templates";

			window.position = window.sharedWindowPosition;
			window.ShowUtility();
		}

		private void ApplyMinimumsToWindow()
		{
			Vector2 size = this.position.size;
			size.x = System.Math.Max(size.x, MIN_WIDTH);
			size.y = System.Math.Max(size.y, MIN_HEIGHT);

			Rect position = this.position;
			position.size = size;
			this.position = position;
		}

		private void RebuildTemplateListIfNecessary()
		{
			if (templates == null)
			{
				templates = new List<XmlTemplate>(TemplateBuilder.FindAllTemplatesInProject());

				if (templates.Count > 0)
				{
					SelectTemplate(templates[0]);
				}
			}
		}

		private void SelectTemplate(XmlTemplate template)
		{
			this.template = template;
			this.codePreview = null;
			this.templateSettings.ChangeForNewTemplate(template);
		}

		private void CreateFileFromTemplate()
		{
			//Create the file
			string sourceCode = TemplateBuilder.BuildCodeFromTemplate(template, templateSettings);
			string filePath = lastKnownCreationPath + "/" + (templateSettings.GetBuildOptionValue("filename") ?? template.id) + ".cs";
			File.WriteAllText(filePath, sourceCode);

			//Open the file
			AssetDatabase.Refresh();
			Object createdFile = AssetDatabase.LoadAssetAtPath<Object>(filePath);
			Selection.activeObject = createdFile;
			AssetDatabase.OpenAsset(createdFile);

			//Close this window
			Close();
		}

		#region Drawing
		/// Draws the UI of the template builder window
		private void OnGUI()
		{
			RebuildTemplateListIfNecessary();
			ApplyMinimumsToWindow();

			EditorGUIUtility.labelWidth = 100.0f;
			sharedWindowPosition = position;

			//Update creation path
			if (!string.IsNullOrEmpty(templateCreatePath))
			{
				lastKnownCreationPath = templateCreatePath;
			}

			//Update the preview
			if (codePreview == null && template != null)
			{
				codePreview = TemplateBuilder.BuildCodeFromTemplate(template, templateSettings);
				codePreview = codePreview.Replace("\t", "    "); // Unity doesn't render tabs very well in the editor
			}

			//Drawing
			EditorGUI.BeginChangeCheck();
			{
				EditorGUILayout.BeginHorizontal();
				{
					//Template Settings
					EditorGUILayout.BeginVertical(GUILayout.Width(SETTINGS_PANEL_WIDTH), GUILayout.ExpandHeight(true));
					{
						DrawTemplateSettings();
					}
					EditorGUILayout.EndVertical();

					//Template Preview
					EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
					{
						DrawTemplatePreview();
					}
					EditorGUILayout.EndVertical();
				}
				EditorGUILayout.EndHorizontal();

				DrawToolbar();
			}
			if (EditorGUI.EndChangeCheck())
			{
				codePreview = null;
			}
		}

		private void DrawTemplateSettings()
		{
			//Settings
			EditorGUILayout.Space();
			EditorGUILayout.BeginVertical(EditorStyles.helpBox);
			{
				EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
				DrawTemplatePicker();
				templateSettings.lineEndings = (TemplateBuilder.LineEndings)EditorGUILayout.EnumPopup("Line Endings", templateSettings.lineEndings);
				EditorGUILayout.Space();
			}
			EditorGUILayout.EndVertical();

			//Template Optional Values
			if (templateSettings.buildOptions.Count > 0)
			{
				EditorGUILayout.BeginVertical(EditorStyles.helpBox);
				{
					EditorGUILayout.LabelField("Template Values", EditorStyles.boldLabel);
					DrawBuildOptions();
				}
				EditorGUILayout.EndVertical();
			}

			//Optional Usings
			if (templateSettings.optionalUsings.Count > 0)
			{
				EditorGUILayout.BeginVertical(EditorStyles.helpBox);
				{
					EditorGUILayout.LabelField("Namespaces", EditorStyles.boldLabel);
					DrawOptionalUsings();
				}
				EditorGUILayout.EndVertical();
			}

			//Create button
			EditorGUILayout.BeginVertical(GUILayout.ExpandHeight(true));
			{
				EditorGUILayout.GetControlRect(GUILayout.ExpandHeight(true));

				GUI.enabled &= !string.IsNullOrEmpty(lastKnownCreationPath) && template != null;
				{
					if (GUILayout.Button("Create"))
					{
						CreateFileFromTemplate();
					}
				}
				GUI.enabled = true;
				EditorGUILayout.Space();
			}
			EditorGUILayout.EndVertical();
		}

		private void DrawTemplatePicker()
		{
			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.PrefixLabel("Template");

				if (EditorGUILayout.DropdownButton(new GUIContent(template != null ? template.id : "<select template>"), FocusType.Keyboard))
				{
					GenericMenu menu = new GenericMenu();

					foreach(var template in templates)
					{
						XmlTemplate _template = template;
						menu.AddItem(new GUIContent(template.id), false, () => SelectTemplate(_template));
					}

					menu.ShowAsContext();
				}
			}
			EditorGUILayout.EndHorizontal();
		}

		private void DrawTemplatePreview()
		{
			templatePreviewScrollPos = EditorGUILayout.BeginScrollView(templatePreviewScrollPos);
			GUI.enabled = false;
			{
				EditorGUILayout.TextArea(codePreview ?? string.Empty, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
			}
			GUI.enabled = true;
			EditorGUILayout.EndScrollView();
		}

		private void DrawToolbar()
		{
			EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.ExpandHeight(true));
			{
				EditorGUILayout.LabelField(lastKnownCreationPath ?? "select a folder in the project view", EditorStyles.miniLabel);
			}
			EditorGUILayout.EndHorizontal();
		}

		private void DrawBuildOptions()
		{
			foreach(var buildOption in templateSettings.buildOptions)
			{
				buildOption.DoLayout();
			}
		}

		private void DrawOptionalUsings()
		{
			foreach(var optionalUsing in templateSettings.optionalUsings)
			{
				optionalUsing.isEnabled = EditorGUILayout.ToggleLeft(optionalUsing.id, optionalUsing.isEnabled);
			}
		}
		#endregion
	}
}