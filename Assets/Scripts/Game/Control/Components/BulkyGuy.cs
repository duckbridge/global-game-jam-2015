using UnityEngine;
using System.Collections;

public class BulkyGuy : Transformer {

	public override void DoTransform() {

		if(!isTransformed && GetComponent<CharacterControl>().IsOnGround()) {
			GetTransformerCharacterControl().DoTransform();
			isTransformed = true;
			GetComponent<TransformComponent>().OnTransform();
			DispatchMessage("OnTransformed", this);

			GetComponent<PlayerInputComponent>().enabled = false;
		}
	}

	public override void DoPraise () {
	
	}
}
