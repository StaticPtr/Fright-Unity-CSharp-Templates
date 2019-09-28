using System.IO;
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
		private const string LOREM_IPSUM = "<template id=\"My Test Template\" version=\"1.0.0.0\">\r\n    <using id=\"System.Collections\" optional=\"true\" onByDefault=\"true\" />\r\n    <using id=\"System.Collections.Generic\" optional=\"true\" onByDefault=\"true\" />\r\n\r\n    <namespace id=\"MyNamespace\">\r\n\r\n/*\r\nAn example of a codeblock\r\n*/\r\n\r\n        <!-- public class Test -->\r\n        <class id=\"Test\" base=\"Object\" access=\"public\">\r\n            <interface-contract id=\"Interface1\" />\r\n            <interface-contract id=\"Interface2\" />\r\n            <interface-contract id=\"Interface3\" />\r\n            \r\n            <codeblock>\r\npublic string myString\r\n{\r\n    get { return null; }\r\n}\r\n            </codeblock>\r\n\r\n            <!-- Add(float, float) : float -->\r\n            <function id=\"Add\" access=\"public\" returnType=\"float\">\r\n                <argument id=\"lhs\" type=\"float\" />\r\n                <argument id=\"rhs\" type=\"float\" />\r\nint result = lhs + rhs;\r\nDebug.Log(result);\r\nreturn result;\r\n            </function>\r\n\r\n            <!-- DoSomething() : void -->\r\n            <function id=\"DoSomething\" access=\"public\" static=\"true\" returnType=\"string\">\r\nreturn \"Test\";\r\n            </function>\r\n        </class>\r\n    </namespace>\r\n</template>";

		[SerializeField] private Rect sharedWindowPosition = new Rect(Vector2.zero, new Vector2(800.0f, 600.0f));
		[SerializeField] private Vector2 templatePreviewScrollPos = Vector2.zero;

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

		#region Drawing
		/// Draws the UI of the template builder window
		private void OnGUI()
		{
			//Position update
			ApplyMinimumsToWindow();
			sharedWindowPosition = position;

			//Drawing
			DrawToolbar();

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
		}

		private void DrawTemplateSettings()
		{
			EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
		}

		private void DrawTemplatePreview()
		{
			templatePreviewScrollPos = EditorGUILayout.BeginScrollView(templatePreviewScrollPos);
			GUI.enabled = false;
			{
				EditorGUILayout.TextArea(LOREM_IPSUM, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
			}
			GUI.enabled = true;
			EditorGUILayout.EndScrollView();
		}

		private void DrawToolbar()
		{
			EditorGUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.ExpandHeight(true));
			{
				EditorGUILayout.Space();
			}
			EditorGUILayout.EndHorizontal();
		}
		#endregion
	}
}