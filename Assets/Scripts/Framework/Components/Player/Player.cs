using UnityEngine;
using System.Collections;

public class Player : DispatchBehaviour {

	protected BloodSpawner bloodSpawner;

	private bool isActivated = false;
	private CharacterControl characterControl;

	// Use this for initialization
	void Start () {
		bloodSpawner = GetComponentInChildren<BloodSpawner>();
		characterControl = GetComponent<CharacterControl>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnActivate() {

		this.isActivated = true;
		this.collider.enabled = true;
		this.transform.Find("FeetCollider").collider.enabled = true;

		this.rigidbody.isKinematic = false;
		this.rigidbody.useGravity = true;

		GetComponent<PlayerInputComponent>().enabled = true;
		
	}

	public CharacterControl GetCharacterControl() {
		return characterControl;
	}

	public virtual void OnDie() {
		if(this.transform.Find ("Sounds/OnDeathSound")) {
			this.transform.Find ("Sounds/OnDeathSound").GetComponent<SoundObject>().PlayIndependent(true);
		}
	}

	public bool IsActivated() {
		return this.isActivated;
	}
}
