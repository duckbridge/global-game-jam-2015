using UnityEngine;
using System.Collections;

public class Parallax2D : MonoBehaviour {

	public Transform targetTransform;
	public float parallaxSpeed;
	private float originalParallaxSpeed;

	public Transform spawnPosition;
	private Vector3 startPosition;
	private Vector3 targetPosition;
	
	void Awake () {
		FollowCamera2D followCamera2D = SceneUtils.FindObject<FollowCamera2D>();
		if(followCamera2D) {
			followCamera2D.AddEventListener(this.gameObject);
		}

		targetPosition = targetTransform.localPosition;
		startPosition = this.spawnPosition.transform.localPosition;
	}
	
	void Update () {
	}

	public void OnCameraMoved(Vector3 moveDifference) {
		
		float newXPosition = this.transform.localPosition.x;
		
		newXPosition += parallaxSpeed * -moveDifference.x;
		
		this.transform.localPosition = new Vector3(newXPosition, this.transform.localPosition.y, this.transform.localPosition.z);
		
		if((Mathf.Abs(targetPosition.x - this.transform.localPosition.x) < parallaxSpeed * 2f) && moveDifference.x > 0) {
			this.transform.localPosition = startPosition;
		}
		
		if((Mathf.Abs(startPosition.x - this.transform.localPosition.x) < parallaxSpeed * 2f) && moveDifference.x < 0) {
			this.transform.localPosition = targetPosition;
		}
	}
}
