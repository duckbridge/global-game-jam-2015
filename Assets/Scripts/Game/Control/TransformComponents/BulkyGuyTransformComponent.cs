using UnityEngine;
using System.Collections;

public class BulkyGuyTransformComponent : TransformComponent {

	public TriggerListener kingCatchTrigger;

	// Use this for initialization
	void Start () {
		kingCatchTrigger = GetComponentInChildren<TriggerListener>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnListenerTrigger(Collider coll) {
		WizardKingSolo wizardKingSolo = coll.gameObject.GetComponent<WizardKingSolo>();
		if(wizardKingSolo) {
			GetComponent<BulkyGuyCharacterControl>().SetCarryingKing();
			Destroy(wizardKingSolo.gameObject);
		}
	}

	public void TransformBack() {
		this.normalCollider.enabled = true;
		this.transform.Find ("FeetCollider").collider.enabled = true;

		this.rigidbody.isKinematic = false;
		this.rigidbody.useGravity = true;
		
	}

	public override void OnTransform () {

		this.rigidbody.isKinematic = true;
		this.rigidbody.useGravity = false;
		this.normalCollider.enabled = false;
		this.transformedCollider.enabled = false;

		this.transform.Find ("FeetCollider").collider.enabled = false;

		kingCatchTrigger.collider.enabled = true;
		kingCatchTrigger.AddEventListener(this.gameObject);

	}
}
