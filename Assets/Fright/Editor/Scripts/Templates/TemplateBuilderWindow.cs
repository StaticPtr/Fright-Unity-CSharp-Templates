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
		private const float MIN_WIDTH = 400.0f;
		private const float MIN_HEIGHT = 400.0f;

		[SerializeField] private Rect sharedWindowPosition = new Rect(Vector2.zero, new Vector2(800.0f, 600.0f));
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
					result = Path.GetDirectoryName(selectedObjectPath);	
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
					template = templates[0];
				}
			}
		}

		#region Drawing
		/// Draws the UI of the template builder window
		private void OnGUI()
		{
			RebuildTemplateListIfNecessary();
			ApplyMinimumsToWindow();
			sharedWindowPosition = position;

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
					EditorGUILayout.BeginVertical(GUILayout.Width(200.0f), GUILayout.ExpandHeight(true));
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
			EditorGUILayout.Space();
			EditorGUILayout.BeginVertical(EditorStyles.helpBox);
			{
				EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);

				if (EditorGUILayout.DropdownButton(new GUIContent(template != null ? template.id : "<select template>"), FocusType.Keyboard))
				{
					GenericMenu menu = new GenericMenu();

					foreach(var template in templates)
					{
						menu.AddItem(new GUIContent(template.id), false, () => this.template = template);
					}

					menu.ShowAsContext();
				}

				templateSettings.lineEndings = (TemplateBuilder.LineEndings)EditorGUILayout.EnumPopup("Line Endings", templateSettings.lineEndings);
			}
			EditorGUILayout.EndVertical();
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
				EditorGUILayout.LabelField(templateCreatePath ?? "select a folder in the project view", EditorStyles.miniLabel);
				EditorGUILayout.Space();
			}
			EditorGUILayout.EndHorizontal();
		}
		#endregion
	}
}