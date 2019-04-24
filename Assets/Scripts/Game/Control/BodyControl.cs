using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BodyControl : DispatchBehaviour {
	
	public float moveSpeed = 5f;

	protected BodyContainer bodyContainer;
	protected Direction currentDirection = Direction.RIGHT;

	private CharacterControl characterControl;
	private bool canMove = true;

	private bool isMovingAutomatic = false;
	private float automaticMoveSpeed = 0;

	public virtual void Awake() {
		characterControl = GetComponent<CharacterControl>();

		bodyContainer = this.transform.Find ("BodyContainer").GetComponent<BodyContainer>();
	}

	public void Update() {}
	public void Start() {}

	public void StopMoving() {
		this.rigidbody.velocity = new Vector3(0f , this.rigidbody.velocity.y, 0f);
	}

	private void FixedUpdate() {
		if(isMovingAutomatic) {
			this.transform.position = 
				new Vector3(this.transform.position.x + automaticMoveSpeed, this.transform.position.y, this.transform.position.z);
		}
	}

	public void Move(float direction) {
		Turn(direction);

		if(bodyContainer.AssertIfGroundIsInFrontOfBody(direction)) {
			this.rigidbody.velocity = new Vector3(0f, this.rigidbody.velocity.y, 0f);
		} else {
			if(!this.rigidbody.isKinematic && canMove) {

				if(direction <= -GameSettings.MINIMUM_VELOCITY_FOR_MOVE) {
					this.rigidbody.velocity = new Vector3(Mathf.FloorToInt(direction) * moveSpeed, this.rigidbody.velocity.y, 0f);
				} else if(direction >= GameSettings.MINIMUM_VELOCITY_FOR_MOVE) {
					this.rigidbody.velocity = new Vector3(Mathf.CeilToInt(direction) * moveSpeed, this.rigidbody.velocity.y, 0f);
				} else {
					this.rigidbody.velocity = new Vector3(0f, this.rigidbody.velocity.y, 0f);
					//misschien weg voor doorglijden
				}
			}
		}
	}

	public void MoveAutomaticKinematicForTime(float moveTime, float moveSpeed) {
		Invoke ("StopAutomaticMovement", moveTime);
		automaticMoveSpeed = moveSpeed;
		isMovingAutomatic = true;
	}

	private void StopAutomaticMovement() {
		isMovingAutomatic = false;
		automaticMoveSpeed = 0f;
		DispatchMessage("OnAutomaticMovementDone", null);
	}

	public virtual void Turn(float direction) {
		if(direction > 0) {
			bodyContainer.transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
			currentDirection = Direction.RIGHT;

		} else if(direction < 0) {
			bodyContainer.transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
			currentDirection = Direction.LEFT;

		} else {
			//currentDirection = Direction.NONE;
		}
	}

	public Direction GetDirection() {
		return this.currentDirection;
	}
}
