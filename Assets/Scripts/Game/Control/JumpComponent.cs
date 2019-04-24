using UnityEngine;
using System.Collections;

public class JumpComponent : DispatchBehaviour {

	public float jumpPowerDecrement;
	private float usedJumpPower;

	private enum InputState { HoldingJump, JumpReleased }
	private InputState inputState = InputState.JumpReleased;

	private bool requestOnGround = false;
	private Transform feetGround, centerFeetGround, frontFeetGround;

	public float jumpPower = 10f;
	public float onGroundRayCastLength = 2f;
	
	protected enum JumpState { ONGROUND, JUMPING, INAIR, FALLING }
	protected JumpState jumpState = JumpState.ONGROUND;

	void Start() {
		usedJumpPower = jumpPower;
		feetGround = transform.Find ("BodyContainer/FeetGround");
		centerFeetGround = this.transform.Find("BodyContainer/CenterFeetGround");
		frontFeetGround = transform.Find ("BodyContainer/FrontFeetGround");
	}

	public void OnJump() {
		if(jumpState != JumpState.JUMPING && jumpState != JumpState.FALLING 
		   && (inputState != InputState.HoldingJump && jumpState != JumpState.FALLING)) {
			inputState = InputState.HoldingJump;
			jumpState = JumpState.JUMPING;
			requestOnGround = false;
			DispatchMessage("OnJump", null);
		}
	}

	public void ReleaseJump() {
		inputState = InputState.JumpReleased;
	}

	public void OnJumpReleased() {
		inputState = InputState.JumpReleased;

		if(this.rigidbody.velocity.y > 0 && !this.rigidbody.isKinematic)
			this.rigidbody.velocity = new Vector3(this.rigidbody.velocity.x, 0f, this.rigidbody.velocity.z);
	
		if(requestOnGround) {
			jumpState = JumpState.ONGROUND;
			DispatchMessage("OnOnGround", null);
		} else if(jumpState != JumpState.ONGROUND) {
			jumpState = JumpState.FALLING;
			DispatchMessage("OnFalling", null);
			DispatchMessage("OnFallingFromJumping", null);
		}

	}

	public void FixedUpdate() {
		if(inputState == InputState.HoldingJump && jumpState == JumpState.JUMPING) {
			if(usedJumpPower > 0 && !this.rigidbody.isKinematic) {
				this.rigidbody.velocity = new Vector3(this.rigidbody.velocity.x, usedJumpPower, 0f);
				usedJumpPower -= jumpPowerDecrement;
			} else {
				jumpState = JumpState.FALLING;
				DispatchMessage("OnFalling", null);
			}
		}

		DoOnGroundCheck(feetGround, centerFeetGround, frontFeetGround);
	}

	private void DoOnGroundCheck(Transform usedTransform, Transform secondUsedTransform, Transform thirdUsedTransform) {
		RaycastHit firstHit;
		RaycastHit secondHit;
		RaycastHit thirdHit;

		if(usedTransform) {

			Physics.Raycast(usedTransform.position, (-this.transform.up), out firstHit, onGroundRayCastLength);
			Physics.Raycast(secondUsedTransform.position, (-this.transform.up), out secondHit, onGroundRayCastLength);
			Physics.Raycast(thirdUsedTransform.position, (-this.transform.up), out thirdHit, onGroundRayCastLength);

			Debug.DrawRay(usedTransform.position, -this.transform.up * onGroundRayCastLength);
			Debug.DrawRay(secondUsedTransform.position, -this.transform.up * onGroundRayCastLength);
			Debug.DrawRay(thirdUsedTransform.position, -this.transform.up * onGroundRayCastLength);

			if ((firstHit.collider) || (secondHit.collider) || (thirdHit.collider)) {
				if(jumpState == JumpState.FALLING || jumpState == JumpState.ONGROUND) {
					if(firstHit.collider) {
						OnObjectHitByRaycast(firstHit.collider);
					} else if(secondHit.collider) {
						OnObjectHitByRaycast(secondHit.collider);
					} else if(thirdHit.collider) {
						OnObjectHitByRaycast(thirdHit.collider);
					}
				}
			} else {
				if(jumpState == JumpState.ONGROUND) {
					jumpState = JumpState.FALLING;
					DispatchMessage("OnFalling", null);
				}
			}
		}
	}

	protected void OnObjectHitByRaycast(Collider coll) {

		if(coll.gameObject.GetComponent<Ground>()) {

			if(inputState == InputState.JumpReleased) {
				jumpState = JumpState.ONGROUND;
			} else {
				requestOnGround = true;
			}
			DispatchMessage("OnOnGround", null);
			usedJumpPower = jumpPower;
		}
	}

	public bool IsFalling() {
		return (jumpState == JumpState.FALLING && !requestOnGround);
	}

	public bool IsJumping() {
		return jumpState == JumpState.JUMPING;
	}
}
