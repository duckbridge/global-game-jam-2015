using UnityEngine;
using System.Collections;

public class WizardKingSolo : Player {

	public float maximumTimeThatHeCanStayOnTheGround = 5f;
	private bool isOnGround = false;
	private bool canDie = true;

	// Update is called once per frame
	void FixedUpdate () {
		if(isOnGround) {
			if(this.rigidbody.velocity == Vector3.zero) {
				isOnGround = false;
				CancelInvoke("OnDie");
				Invoke ("OnDie", maximumTimeThatHeCanStayOnTheGround);
			}
		}
	}
		
	public void CannotDie() {
		CancelInvoke("OnDie");
		canDie = false;
	}

	public override void OnDie () {
		if(!canDie) {
			return;
		}

		base.OnDie ();
		bloodSpawner = GetComponentInChildren<BloodSpawner>();
		bloodSpawner.SpawnBlood();
		DispatchMessage("OnWizardSoloDied", this);
	}

	public void OnThrown() {
		Invoke ("ResetCollider", .5f);
	}

	private void ResetCollider() {
		this.collider.isTrigger = false;
	}

	public void OnCollisionEnter(Collision coll) {
		Ground ground = coll.gameObject.GetComponent<Ground>();
		if(ground) {
			isOnGround = true;
		}
	}
}
