using UnityEngine;
using System.Collections;

public class FollowCamera2D : DispatchBehaviour {

	public GameObject gameObjectToFollow;
	private float minimumDistanceX = 0f;
	private float minimumDistanceY = 0f;

	private float cameraMoveSpeedX = 0.05f;
	private float cameraMoveSpeedY = 0.05f;


	private Vector3 oldPosition;

	public bool doStickyFollowing = true;

	public enum FollowType { ALL, HORIZONTAL, VERTICAL }
	public FollowType followType;

	private Vector3 positionFromTargetBeforeSwap;

	void Awake() {
		minimumDistanceX = GameSettings.CAMERA_MINIMUM_FOLLOW_DISTANCE_X;
		minimumDistanceY = GameSettings.CAMERA_MINIMUM_FOLLOW_DISTANCE_Y;

		cameraMoveSpeedX = GameSettings.CAMERA_FOLLOW_SPEED_X;
		cameraMoveSpeedY = GameSettings.CAMERA_FOLLOW_SPEED_Y;
	}

	void Start () {}
	
	void FixedUpdate () {
		oldPosition = this.transform.position;

		if(!doStickyFollowing) {
			MoveCameraToTarget();
		} else {
			this.transform.position = new Vector3(gameObjectToFollow.transform.position.x, this.transform.position.y, this.transform.position.z);
		}

		if((this.transform.position - oldPosition) != Vector3.zero) {
			DispatchMessage("OnCameraMoved", (this.transform.position - oldPosition));
		}
	}

	public void SetPositionAtFollowingTargetAndSpecialYPosition(Transform specialYPosition) {
		this.transform.position = new Vector3(gameObjectToFollow.transform.position.x, specialYPosition.position.y, this.transform.position.z);
	}

	public void SetPositionAtFollowingTarget() {
		this.transform.position = new Vector3(gameObjectToFollow.transform.position.x, this.transform.position.y, this.transform.position.z);
	}

	private void MoveCameraToTarget() {
		Vector2 distanceToTarget = GetDistanceToFollowingObject();
		if(Mathf.Abs(distanceToTarget.x) > minimumDistanceX || Mathf.Abs (distanceToTarget.y) > minimumDistanceY) {

			MoveCameraWithDistance(distanceToTarget);
		}
	}
	

	private Vector2 GetDistanceToFollowingObject() {
		Vector2 distanceToFollowingObject = Vector2.zero;

		switch(followType) {

			case FollowType.ALL:
				distanceToFollowingObject = 
					(gameObjectToFollow.transform.position - this.transform.position);
			break;
				
			case FollowType.HORIZONTAL: 
				distanceToFollowingObject.x = 
					gameObjectToFollow.transform.position.x - this.transform.position.x;
			break;
				
			case FollowType.VERTICAL:
				distanceToFollowingObject.y = 
					gameObjectToFollow.transform.position.y - this.transform.position.y;
			break;
		}

		return distanceToFollowingObject;
	}

	private void MoveCameraWithDistance(Vector2 distance) {

		switch(followType) {
			case FollowType.ALL: 
				this.transform.position = 
					new Vector3(this.transform.position.x + (distance.x * cameraMoveSpeedX), 
					            this.transform.position.y + (distance.y * cameraMoveSpeedY), this.transform.position.z);
				break;
				
			case FollowType.HORIZONTAL: 
				this.transform.position = 
					new Vector3(this.transform.position.x + (distance.x * cameraMoveSpeedX), 
					            this.transform.position.y, this.transform.position.z);
				
				break;
				
			case FollowType.VERTICAL:
				this.transform.position = 
					new Vector3(this.transform.position.x, 
					            this.transform.position.y + (distance.y * cameraMoveSpeedY), this.transform.position.z);
			
			break;
		}
	}

	public void SwapFollowType(FollowType newFollowType) {
		this.followType = newFollowType;
	}

	public override void OnPauseGame() {}
	
	public override void OnResumeGame() {}
}
