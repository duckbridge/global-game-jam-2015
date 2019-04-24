using UnityEngine;
using System.Collections;

public class TransformerCharacterControl : CharacterControl {

	public float velocityDivider = 2f;

	public void DoPushForward() {
	}

	public void DoTransform() {
		SwitchState(State.TRANSFORMING);
		if(Mathf.Abs (this.rigidbody.velocity.x) > 0) {
			this.rigidbody.velocity = new Vector3(this.rigidbody.velocity.x /  velocityDivider, 
			                                      this.rigidbody.velocity.y, this.rigidbody.velocity.z);
		}
		DisableAnimationSwitching();
	}

	public void OnSelected() {
		SwitchState(State.SELECTED);
		DisableAnimationSwitching();
	}

	public void OnUnSelected() {
		EnableAnimationSwitching();
		SwitchState(State.NORMAL);
	}

	protected override void PlayAnimationForState (State newState) {
		base.PlayAnimationForState (newState);

		switch(newState) {
			
			case State.SELECTED:
				animationManager.PlayAnimationByName("Selected", true);
			break;

			case State.TRANSFORMING:	
				animationManager.PlayAnimationByName("Transformed", true);
			break;
		}
	}


}
