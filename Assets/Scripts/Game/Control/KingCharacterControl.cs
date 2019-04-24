using UnityEngine;
using System.Collections;

public class KingCharacterControl : CharacterControl {

	private bool hasHatOn = false;
	private bool isCarryingKing = true;

	public void OnCastingMagic() {
		EnableAnimationSwitching();
		SwitchState(State.CASTINGMAGIC);
		DisableAnimationSwitching();
	}
	
	public void OnTakeOffHat() {
		hasHatOn = false;

		EnableAnimationSwitching();
		SwitchState(State.TAKINGOFFHAT);
		DisableAnimationSwitching();
	}
	
	public void OnPuttingOnHat() {
		hasHatOn = true;

		EnableAnimationSwitching();
		SwitchState(State.SWITCHINGHAT);
		DisableAnimationSwitching();
	}

	public void StartPointing() {
		EnableAnimationSwitching();
		SwitchState(State.POINTING);
		DisableAnimationSwitching();
	}

	public void OnPointing(float distanceInPercentage) {

		EnableAnimationSwitching();

		if(distanceInPercentage > .33f && distanceInPercentage < .77f) {
			SwitchState(State.POINTINGSEMIFAR);
		} else if(distanceInPercentage < .33) {
			SwitchState(State.POINTINGCLOSE);
		} else {
			SwitchState(State.POINTINGFAR);
		}

		DisableAnimationSwitching();
	}

	public void OnAnimationDone(Animation2D animation2D) {

		if(animation2D.name == "CastingMagic") {
			EnableAnimationSwitching();
			StandStill();
		}

		if(animation2D.name == "Pointing") {
			DispatchMessage("OnPointingDone", null);
		}
	}
	
	public void OnReverseAnimationDone(Animation2D animation2D) {
		if(animation2D.name == "SwitchingHat") {
			DispatchMessage("OnHatTakenOff", null);
		}
	}

	public void SetNotCarryingKing() {
		isCarryingKing = false;
	}

	protected override void PlayAnimationForState (State newState) {
		base.PlayAnimationForState (newState);

		switch(newState) {

			case State.POINTING:
				animationManager.PlayAnimationByName("Pointing", true);
				animationManager.AddEventListenerTo("Pointing", this.gameObject);
			break;

			case State.POINTINGSEMIFAR:
				animationManager.PlayAnimationByName("PointingSemiFar", true);
			break;

			case State.POINTINGFAR:
				animationManager.PlayAnimationByName("PointingFar", true);
			break;

			case State.POINTINGCLOSE:
				animationManager.PlayAnimationByName("PointingClose", true);
			break;
				
			case State.SWITCHINGHAT:
				animationManager.PlayAnimationByName("SwitchingHat", true);
				animationManager.AddEventListenerTo("SwitchingHat", this.gameObject);
			break;
			
			case State.TAKINGOFFHAT:
				animationManager.PlayAnimationByNameReversed("SwitchingHat", true);
				animationManager.AddEventListenerTo("SwitchingHat", this.gameObject);
			break;
				
			case State.CASTINGMAGIC:
				animationManager.PlayAnimationByName("CastingMagic", true);
				animationManager.AddEventListenerTo("CastingMagic", this.gameObject);
			break;
		}
	}

	protected override string DecidePrefix () {
		string fullyDecidedPrefix = base.DecidePrefix ();
		if(isCarryingKing) {
			fullyDecidedPrefix += hasHatOn ? "-HatOn" : "";
		} else {
			fullyDecidedPrefix += "-NotCarryingKing";
		} 

		return fullyDecidedPrefix;
	}

	public bool HasHatOn() {
		return this.hasHatOn;
	}

}
