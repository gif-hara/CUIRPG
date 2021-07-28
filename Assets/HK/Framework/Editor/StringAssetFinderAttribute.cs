#if UNITY_EDITOR
using UnityEngine;
using System.Collections.Generic;
using HK.Framework.Text;
using UnityEditor;

namespace HK.Framework.Editor
{
	[CustomPropertyDrawer(typeof(StringAsset.Finder))]
	public class StringAssetFinderDrawer : PropertyDrawer
	{
		private static Dictionary<StringAsset, string[]> cachedPopupList = new Dictionary<StringAsset, string[]>();

		private static string[] emptyStringAsset = new string[0];

		private static Rect rect = new Rect();

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

			var indentLevel = EditorGUI.indentLevel;
			EditorGUI.indentLevel = 0;

			var targetProperty = property.FindPropertyRelative("target");

			rect.Set(position.x, position.y, position.width / 2, position.height);
			EditorGUI.PropertyField(rect, targetProperty, GUIContent.none);

			var stringAsset = targetProperty.objectReferenceValue as StringAsset;
			rect.Set(rect.x + rect.width, position.y, position.width / 2, position.height);
			var finderValue = property.FindPropertyRelative("value");
			var finderGuid = property.FindPropertyRelative("guid");
			EditorGUI.BeginChangeCheck();
			var selectedIndex = EditorGUI.Popup(rect, GetCurrentSelectKeyIndex(stringAsset, finderGuid.stringValue), GetKeyAndDescriptionList(stringAsset));
			if(EditorGUI.EndChangeCheck() && stringAsset != null)
			{
				finderValue.stringValue = stringAsset.database[selectedIndex].value.Default;
				finderGuid.stringValue = stringAsset.database[selectedIndex].guid;
			}

			EditorGUI.indentLevel = indentLevel;
		}

		public static void RemoveCachedDictionary(StringAsset stringAsset)
		{
			cachedPopupList.Remove(stringAsset);
		}


		private string[] GetKeyAndDescriptionList(StringAsset stringAsset)
		{
			if(stringAsset == null)
			{
				return emptyStringAsset;
			}

			if(cachedPopupList.ContainsKey(stringAsset))
			{
				return cachedPopupList[stringAsset];
			}

			string[] list = new string[stringAsset.database.Count];
			for(var i = 0; i < list.Length; i++)
			{
				list[i] = stringAsset.database[i].value.Default.Split('\n')[0];
			}

			cachedPopupList.Add(stringAsset, list);

			return list;
		}

		private int GetCurrentSelectKeyIndex(StringAsset stringAsset, string finderGuid)
		{
			if(stringAsset == null)
			{
				return 0;
			}
			return stringAsset.database.FindIndex(d => d.guid.CompareTo(finderGuid) == 0);
		}
	}
}
#endif
