namespace CocodriloDog.Core {
	
	using System;
	using UnityEngine;
	using UnityEngine.Serialization;


	#region Small Types

	/// <summary>
	/// Defines how to show the <see cref="CompositeObjectReference{T}"/> field.
	/// </summary>
	public enum CompositeObjectReferenceMode {
		
		/// <summary>
		/// Sets the field so that the user can choose a source.
		/// </summary>
		CanChooseSource,

		/// <summary>
		/// Sets the field so that the user can't choose a source, but can see a disabled field with the current source.
		/// </summary>
		CannotChooseSource_ShowSource,

		/// <summary>
		/// Sets the field so that the user can't choose a source, and the field with the current source is not visible.
		/// </summary>
		CannotChooseSource_HideSource,

	}

	#endregion


	/// <summary>
	/// An object that can search for and reference <see cref="CompositeObject"/>s from the <see cref="Source"/>
	/// root.
	/// </summary>
	/// 
	/// <remarks>
	/// For this system to work, the <see cref="CompositeObject"/> instance must be registered before
	/// <see cref="Value"/> is requested, this should be done normally on <c>Awake</c>. For the 
	/// <see cref="CompositeObject"/> to be garbage collected, the instance must be unregistered, 
	/// normally <c>OnDestroy</c>. For this purpose, you can use 
	/// <see cref="CompositeObject.RegisterAsReferenceable(UnityEngine.Object)"/> and
	/// <see cref="CompositeObject.UnregisterReferenceable(UnityEngine.Object)"/>
	/// </remarks>
	/// 
	/// <typeparam name="T">The type of <see cref="CompositeObject"/> or interface to look for</typeparam>
	[Serializable]
	public class CompositeObjectReference<T> : ISerializationCallbackReceiver where T : class {


		#region Public Properties

		/// <summary>
		/// The source of the <see cref="CompositeObject"/>.
		/// </summary>
		public UnityEngine.Object Source { 
			get => m_Source; 
			set => m_Source = value; 
		}

		/// <summary>
		/// The referenced <see cref="CompositeObject"/>.
		/// </summary>
		public T Value {
			get {
				if (!Application.isPlaying) {
					Debug.LogWarning("CompositeObjectReference<T>.Value only works at runtime.");
				}
				var compositeObject = ReferenceableCompositeObjects.GetById(m_Source, m_Id);
				if(compositeObject == null) {
					Debug.LogWarning("CompositeObject not found. Did you forget to register it with RegisterAsReferenceable()?");
				}
				return compositeObject as T;
			}
		}

		#endregion


		#region Public Methods

		/// <summary>
		/// Tries to resolve the referenced <see cref="CompositeObject"/> at runtime without logging when it is missing.
		/// Prefer this over <see cref="Value"/> when a missing reference is expected and should fall back gracefully.
		/// </summary>
		public bool TryGetValue(out T value) {
			value = null;
			if (!Application.isPlaying) {
				return false;
			}
			var compositeObject = ReferenceableCompositeObjects.GetById(m_Source, m_Id);
			value = compositeObject as T;
			return value != null;
		}

		public void OnAfterDeserialize() => Initialize();

		public void OnBeforeSerialize() { }

		/// <summary>
		/// Sets <see cref="ShowSourceField"/>, <see cref="EnableSourceField"/>, and 
		/// <see cref="ShowEnableSourceFieldToggle"/> according to the specified <paramref name="mode"/>.
		/// </summary>
		/// <param name="mode">The mode.</param>
		public void SetMode(CompositeObjectReferenceMode mode) {
			switch (mode) {

				case CompositeObjectReferenceMode.CanChooseSource:
					m_ShowSourceField = true;
					m_EnableSourceField = true;
					break;

				case CompositeObjectReferenceMode.CannotChooseSource_ShowSource:
					m_ShowSourceField = true;
					m_EnableSourceField = false;
					break;

				case CompositeObjectReferenceMode.CannotChooseSource_HideSource:
					m_ShowSourceField = false;
					m_EnableSourceField = false;
					break;

			}
		}

		/// <summary>
		/// Used to set the source to <c>null</c> so that the inspector resets it to the default root object
		/// when <see cref="m_EnableSourceField"/> is <c>false</c>.
		/// </summary>
		/// <remarks>
		/// Only has effect when not overriding the source, because otherwise the user may want to preserve 
		/// the overriding <see cref="m_Source"/>.
		/// </remarks>
		public void ClearSourceToDefault() {
			if (!m_EnableSourceField) {
				m_Source = null;
			}
		}

		#endregion


		#region Private Fields

		[HideInInspector]
		[SerializeField]
		private bool m_Initialized;

		[Tooltip("The root of the CompositeObject.")]
		[SerializeField]
		private UnityEngine.Object m_Source;

		[SerializeField]
		private bool m_ShowSourceField = true;

		[Tooltip("Allows to choose another object as the source root.")]
		[SerializeField]
		private bool m_EnableSourceField = true;

		[Tooltip("The unique Id of the CompositeObject.")]
		[SerializeField]
		private string m_Id;

		#endregion


		#region Private Methods

		private void Initialize() {
			if (!m_Initialized) {
				// Force default values
				// This is needed only in array elements
				m_ShowSourceField = true;
				m_EnableSourceField = true;
				m_Initialized = true;
			}
		}

		#endregion


	}

}