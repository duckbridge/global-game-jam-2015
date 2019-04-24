using UnityEngine;
using System.Collections;

public class BulkyGuyCharacterControl : TransformerCharacterControl {

	private bool isCarryingKing = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetCarryingKing() {
		isCarryingKing = true;
		EnableAnimationSwitching();

		GetComponent<BulkyGuyTransformComponent>().TransformBack();
		StandStill();
		GetComponent<PlayerInputComponent>().enabled = true;
	}

	protected override string DecidePrefix () {
		string decidedPrefix = base.DecidePrefix();

		if(isCarryingKing) { 
			decidedPrefix += "-CarryingKing"; 
		}

		return decidedPrefix;
	}

	protected override void PlayAnimationForState (State newState) {
		base.PlayAnimationForState (newState);

		if(isCarryingKing) {
		
			switch(newState) {
				case State.JUMPING:
					animationManager.PlayAnimationByName("Jumping-CarryingKing-Idle", true);
				break;
			
				case State.FALLING:
					animationManager.PlayAnimationByName("Falling-CarryingKing-Idle", true);
				break;
			}
		}
	}

	public bool IsCarryingKing() {
		return this.isCarryingKing;
	}
}
