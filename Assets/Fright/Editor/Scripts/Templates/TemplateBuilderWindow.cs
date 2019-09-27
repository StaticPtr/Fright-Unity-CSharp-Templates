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
		[SerializeField] private Rect sharedWindowPosition = new Rect(Vector2.zero, new Vector2(800.0f, 600.0f));

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

		/// Draws the UI of the template builder window
		private void OnGUI()
		{
			sharedWindowPosition = position;
		}
	}
}