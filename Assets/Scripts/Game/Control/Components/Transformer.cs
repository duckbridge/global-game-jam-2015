using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Transformer : Player {

	public int ID = 0;
	private GameObject shape;
	protected bool isTransformed = false;
	private Transform crackSoundsContainer;

	void Awake() {
		shape = this.transform.Find ("Shape").gameObject;
		crackSoundsContainer = this.transform.Find("Sounds/BoneCrackSounds");
	}

	public override void OnDie () {
		base.OnDie();

		bloodSpawner.SpawnBlood();
		DispatchMessage("OnTransformerDied", this);
	}

	public virtual void DoPraise() {
		GetComponent<CharacterControl>().DoPraise();
	}

	public void OnChosen() {
		OnUnSelected();
	}

	public void OnPushedForward() {
		OnActivate();
	}

	public void OnReachedEndOfLevel() {
		GetComponent<PlayerInputComponent>().enabled = false;
		GetCharacterControl().StandStill();
		Invoke ("DisableAllCollidersAndStuff", 1.5f);
		DispatchMessage("OnTransformed", this);
	}

	private void DisableAllCollidersAndStuff() {
		GetTransformerCharacterControl().OnSelected();
		
		this.rigidbody.isKinematic = true;
		this.rigidbody.useGravity = false;
		
		this.transform.Find("FeetCollider").collider.enabled = false;
		GetComponent<TransformComponent>().normalCollider.enabled = false;
	}

	public virtual void DoTransform() {

		SoundObject[] crackSounds = crackSoundsContainer.GetComponentsInChildren<SoundObject>();
		int randomChosenSound = Random.Range (0, crackSounds.Length);
		crackSounds[randomChosenSound].Play();

		GetTransformerCharacterControl().DoTransform();
		isTransformed = true;
		GetComponent<TransformComponent>().OnTransform();
		DispatchMessage("OnTransformed", this);

		GetComponent<PlayerInputComponent>().enabled = false;

	}

	public void OnSelected() {
		GetTransformerCharacterControl().OnSelected();
		shape.SetActive(true);
	}

	public void OnUnSelected() {
		GetTransformerCharacterControl().OnUnSelected();
		shape.SetActive(false);
	}

	public bool IsTranformed() {
		return this.isTransformed;
	}

	protected TransformerCharacterControl GetTransformerCharacterControl() {
		return (TransformerCharacterControl) GetCharacterControl();
	}

	public BloodSpawner GetBloodSpawner() {
		return this.bloodSpawner;
	}

	public bool IsTransformed() {
		return this.isTransformed;
	}
}
