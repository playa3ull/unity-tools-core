namespace CocodriloDog.Core {

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;

	/// <summary>
	/// Triggers collision events when other <see cref="CollisionTrigger"/>s enter and exit this one and have
	/// <see cref="SelfTags"/> that match the <see cref="OtherTags"/>.
	/// </summary>
	public class CollisionTrigger : CollisionTriggerBase<CollisionTrigger, Collision> {


		#region Unity Methods

		private void OnTriggerEnter(Collider other) {
			var otherTrigger = other.GetComponentInParent<CollisionTrigger>();
			if (otherTrigger != null && DoOtherTagsMatch(otherTrigger) && EnterTrigger(otherTrigger)) {
				RaiseTriggerEnter(otherTrigger);
				m_OnTriggerEnter.Invoke(otherTrigger);
			}
		}

		private void OnTriggerExit(Collider other) {
			var otherTrigger = other.GetComponentInParent<CollisionTrigger>();
			if (otherTrigger != null && DoOtherTagsMatch(otherTrigger) && ExitTrigger(otherTrigger)) {
				RaiseTriggerExit(otherTrigger);
				m_OnTriggerExit.Invoke(otherTrigger);
			}
		}

		private void OnCollisionEnter(Collision collision) {
			var otherTrigger = collision.collider.GetComponentInParent<CollisionTrigger>();
			if (otherTrigger != null && DoOtherTagsMatch(otherTrigger) && EnterTrigger(otherTrigger)) {
				RaiseCollisionEnter(collision);
				m_OnCollisionEnter.Invoke(collision);
			}
		}

		private void OnCollisionExit(Collision collision) {
			var otherTrigger = collision.collider.GetComponentInParent<CollisionTrigger>();
			if (otherTrigger != null && DoOtherTagsMatch(otherTrigger) && ExitTrigger(otherTrigger)) {
				RaiseCollisionExit(collision);
				m_OnCollisionExit.Invoke(collision);
			}
		}

		#endregion


	}

}
