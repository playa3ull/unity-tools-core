namespace CocodriloDog.Core {

	using System.Collections;
	using System.Collections.Generic;
	using System.Reflection;
	using UnityEditor;
	using UnityEngine;

	[CustomPropertyDrawer(typeof(HideAttribute))]
	public class HidePropertyDrawer : PropertyDrawerBase {


		#region Unity Methods

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
			if (m_Hide) {
				return 0;
			} else {
				return base.GetPropertyHeight(property, label);
			}
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {

			base.OnGUI(position, property, label);

			Label = EditorGUI.BeginProperty(Position, Label, Property);

			object targetObject = null;
			MethodInfo method = null;
			var hideAttribute = attribute as HideAttribute;

			// Find the help method
			// First look in the parent property to support system objects

			// Try to get the parent property, which will be used to look for asset or method
			var parentProperty = CDEditorUtility.GetNonArrayOrListParentProperty(Property);

			if (parentProperty != null) {
				targetObject = CDEditorUtility.GetPropertyValue(parentProperty);
				method = CDEditorUtility.GetMethod(targetObject, hideAttribute.MethodName);
			}
			// Resort to the serializedObject.targetObject
			if (method == null) {
				targetObject = Property.serializedObject.targetObject;
				method = CDEditorUtility.GetMethod(targetObject, hideAttribute.MethodName);
			}

			// Get the results
			m_Hide = false;
			if (method != null) {
				m_Hide = (bool)method.Invoke(targetObject, null);
			}
			if (!m_Hide) {
				EditorGUI.indentLevel += hideAttribute.IndentDelta;
				EditorGUI.PropertyField(GetNextPosition(Property), Property); ;
				EditorGUI.indentLevel -= hideAttribute.IndentDelta;
			}

			EditorGUI.EndProperty();

		}

		#endregion


		#region Private Fields

		private bool m_Hide;

		#endregion


	}

}
