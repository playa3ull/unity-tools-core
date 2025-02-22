namespace CocodriloDog.Core {

	using System.Collections;
	using System.Collections.Generic;
	using UnityEditor;
	using UnityEngine;

	/// <summary>
	/// Default editor for <see cref="ScriptableCompositeRoot"/> classes
	/// </summary>
	[CustomEditor(typeof(ScriptableCompositeRoot), true)]
	public class ScriptableCompositeRootEditor : CompositeRootEditor { }

}