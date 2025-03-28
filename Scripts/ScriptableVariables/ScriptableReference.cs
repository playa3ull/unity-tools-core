namespace CocodriloDog.Core {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	[CreateAssetMenu(menuName = "Cocodrilo Dog/Core/Scriptable Reference")]
	public class ScriptableReference : ScriptableValue<UnityEngine.Object> {


		#region Public Properties

		/// <summary>
		/// The value that is stored/referenced by this asset.
		/// </summary>
		public override UnityEngine.Object Value {
			get => base.Value;
			set {

				var previousValue = base.Value;
				var valueChanges = value != previousValue;

				if (valueChanges && previousValue != null) {
					OnInstanceDiscard?.Invoke(previousValue);
				}

				if (valueChanges) {
					base.Value = value;
					if (Value != null) {
						m_OnInstanceReady?.Invoke(Value);
					}
				}

			}
		}

		#endregion


		#region Public Delegates

		public delegate void ReferenceChange(UnityEngine.Object unityObject);

		#endregion


		#region Public Events

		/// <summary>
		/// Raised every time a non-null reference value is set.
		/// </summary>
		/// 
		/// <remarks>
		/// Use this to initialize the new instance. E.G. adding event handlers, calling methods, etc.
		/// </remarks>
		public event ReferenceChange OnInstanceReady {
			add {
				lock (this) {
					m_OnInstanceReady += value;
				}
			}
			remove {
				lock (this) {
					m_OnInstanceReady -= value;
				}
			}
		}

		/// <summary>
		/// Raised when the <c>Value</c> is about to change and the current value is not null, and soon to be released.
		/// </summary>
		/// 
		/// <remarks>
		/// Use this to clean old references that won't be used anymore. E.G. removing event handlers,
		/// disposing objects, etc.
		/// </remarks>
		public event ReferenceChange OnInstanceDiscard;

		#endregion


		#region Private Fields

		[NonSerialized]
		private ReferenceChange m_OnInstanceReady;

		#endregion


	}

}
