using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;
using HK.Framework.Text;

namespace HK.Framework.Editor.Text
{
	[CustomEditor(typeof(StringAsset))]
	public class StringAssetEditor : EditorScriptableObject<StringAsset>
	{
		private ReorderableList reorderableList;

		private string culture;

		private GUIContent currentCultureContent;

		private Rect valueRect = new Rect();

		void OnEnable()
		{
			this.culture = "ja";
			this.currentCultureContent = new GUIContent(this.HeaderName);
			reorderableList = new ReorderableList(serializedObject, serializedObject.FindProperty("database"));
			reorderableList.elementHeightCallback = (index) =>
			{
				return this.GetElementHeight(this.Target.database[index].value.Get(this.culture));
			};
			reorderableList.drawHeaderCallback = (Rect rect) =>
			{
				rect = EditorGUI.PrefixLabel(rect, this.currentCultureContent);
				this.CultureButton(rect, 0, "ja");
				this.CultureButton(rect, 1, "en");
			};
			reorderableList.drawElementCallback = (Rect rect, int index, bool selected, bool focused) =>
			{
				var data = this.Target.database[index];
				var currentValue = data.value.Get(this.culture);
				this.valueRect.Set(rect.x, rect.y, rect.width, GetElementHeight(currentValue));
				EditorGUI.BeginChangeCheck();
				var newValue = EditorGUI.TextArea(this.valueRect, currentValue);
				if(EditorGUI.EndChangeCheck())
				{
					Undo.RecordObject(this.target, "Changed StringAsset value");
					reorderableList.serializedProperty.GetArrayElementAtIndex(index)
						.FindPropertyRelative("value")
						.FindPropertyRelative(this.culture)
						.stringValue = newValue;
					StringAssetFinderDrawer.RemoveCachedDictionary(this.Target);
				}
			};
			reorderableList.onAddCallback = (ReorderableList list) =>
			{
				list.serializedProperty.InsertArrayElementAtIndex(list.serializedProperty.arraySize);
				var property = reorderableList.serializedProperty.GetArrayElementAtIndex(list.serializedProperty.arraySize - 1);
				var valueProperty = property.FindPropertyRelative("value");
				valueProperty.FindPropertyRelative("ja").stringValue = "";
				valueProperty.FindPropertyRelative("en").stringValue = "";
				property.FindPropertyRelative("guid").stringValue = System.Guid.NewGuid().ToString();
			};

		}

		public override void OnInspectorGUI()
		{
			reorderableList.DoLayoutList();
			serializedObject.ApplyModifiedProperties();
		}

		private void CultureButton(Rect origin, int index, string cultureIdentity)
		{
			const float width = 30;
			this.valueRect.Set(origin.x + index * width, origin.y, width, origin.height);
			if(GUI.Button(this.valueRect, cultureIdentity))
			{
				this.culture = cultureIdentity;
				this.currentCultureContent.text = this.HeaderName;
			}
		}

		private float GetElementHeight(string text)
		{
			return EditorGUIUtility.singleLineHeight + (text.Split('\n').Length - 1) * (EditorGUIUtility.singleLineHeight - 3);
		}

		private string HeaderName
		{
			get
			{
				return "Current Culture = " + this.culture;
			}
		}
	}
}
