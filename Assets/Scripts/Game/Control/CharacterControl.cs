using UnityEngine;
using System.Collections;

public class CharacterControl : DispatchBehaviour {
	
	protected BodyControl bodyControl;
	protected AnimationManager2D animationManager;
	private JumpComponent jumpComponent;

	public enum State { 
		MOVING,  
		JUMPING,
		FALLING, 
		TRANSFORMING,
		DEAD,
		NORMAL,
		IDLE,
		SELECTED,
		POINTINGSEMIFAR,
		SWITCHINGHAT,
		CASTINGMAGIC,
		TAKINGOFFHAT,
		POINTINGCLOSE,
		POINTINGFAR,
		POINTING,
		PRAISING
	}

	public State state = State.IDLE;

	private bool isOnGround = true;
	private bool canHoldJump = true;
	private bool isHoldingJump = false;

	private bool isAbleToSwitchAnimations = true;

	public virtual void Awake() {
		jumpComponent = GetComponent<JumpComponent>();
		jumpComponent.AddEventListener(this.gameObject);

		bodyControl = GetComponent<BodyControl>();
		bodyControl.AddEventListener(this.gameObject);

		animationManager = this.transform.Find ("BodyContainer/Animations").GetComponent<AnimationManager2D>();
	}

	public virtual void DoMove(float direction) {
		if(state != State.TRANSFORMING || state != State.DEAD) {
			bodyControl.Move(direction);

			if(isOnGround) {
				SwitchState(State.NORMAL);
			}
		}
	}

	public void DoPraise() {
		SwitchState(State.PRAISING);
		DisableAnimationSwitching();
	}

	public void FixBugJumpHeldAndReleasedWhenNotJumping() {
		isHoldingJump = false;
		
		if(isOnGround) {
			canHoldJump = true;
		}
		
		jumpComponent.ReleaseJump();
	}
	
	public void FixBugJumpHeldAndReleasedWhenMaybeJumping() {
		DoStopJump();
	}

	public void StandStill() {
		SwitchState(State.IDLE);
		DoMove(0f);
	}
	
	public void DoJump() {

		if(canHoldJump) {
			DispatchMessage("OnJumped", this);
			jumpComponent.OnJump();
		}
	}

	public void DoStopJump() {
		
		isHoldingJump = false;

		if(isOnGround) {
			canHoldJump = true;
		}
		jumpComponent.OnJumpReleased();
	}

	public void OnJump() {
		isOnGround = false;
		SwitchState(State.JUMPING);
	}

	public void OnFallingFromJumping() {
		canHoldJump = false;
	}

	public void OnFalling() {
		SwitchState(State.FALLING);
	}

	public void OnOnGround() {
		if(!isOnGround) {
			isOnGround = true;
		
			if(!isHoldingJump) {
				canHoldJump = true;
			}
			SwitchState(State.NORMAL);
		}
	}

	protected void SwitchState(State newState) {
		if(state != State.DEAD) {
			if(!isAbleToSwitchAnimations) {
				return;
			}

			PlayAnimationForState(newState);
			state = newState;
		}
	}
	
	protected virtual void PlayAnimationForState(State newState) {
		string newAnimationName = "";

		switch(newState) {

			case State.IDLE:
				newAnimationName = DecidePrefix() + "-Idle";
				animationManager.ResumeAnimationSynced(newAnimationName);
			break;
			
			case State.JUMPING:
				newAnimationName = "Jumping-Idle";
				animationManager.PlayAnimationByName(newAnimationName, true);
			break;

			case State.FALLING:
				newAnimationName = "Falling-Idle";
				animationManager.PlayAnimationByName(newAnimationName, true);
			break;

			case State.MOVING:
				newAnimationName = DecidePrefix() + "-Idle";
				animationManager.ResumeAnimationSynced(newAnimationName);
			break;

			case State.PRAISING:
				newAnimationName = "Praising";
				animationManager.PlayAnimationByName(newAnimationName, true);
			break;

			case State.NORMAL:
				if(IsMoving()) {
					SwitchState(State.MOVING);
				} else {
					SwitchState(State.IDLE);
				}
			break;

			case State.DEAD:
				newAnimationName = "DeathAnimation";
				animationManager.PlayAnimationByName(newAnimationName, true);
			break;
		}
	}

	protected virtual string DecidePrefix() {
		string prefix = "";

		if(IsMoving() && !jumpComponent.IsJumping() && !IsFalling()) {
			prefix = GameSettings.ANIMATION_RUNNINGPREFIX;

		} else if(jumpComponent.IsJumping()){
			prefix = GameSettings.ANIMATION_JUMPINGPREFIX;

		} else if(IsFalling()) { 
			prefix = GameSettings.ANIMATION_FALLINGPREFIX;
	
		} else {
			prefix = GameSettings.ANIMATION_STANDINGPREFIX;
		}

		return prefix;
	}

	public void EnableAnimationSwitching() {
		isAbleToSwitchAnimations = true;
	}

	public void DisableAnimationSwitching() {
		isAbleToSwitchAnimations = false;
	}

	public bool IsMoving() {
		return Mathf.Abs(rigidbody.velocity.x) > GameSettings.MINIMUM_VELOCITY_FOR_MOVE && Mathf.Abs((float)bodyControl.GetDirection()) > 0f;
	}

	private bool IsRunning() {
		return (Mathf.Abs(rigidbody.velocity.x) >= GameSettings.MINIMUM_RUNVELOCITY);
	}

	private bool IsFalling() {
		return jumpComponent.IsFalling();
	}

	public bool IsOnGround() {
		return this.isOnGround;
	}
}
