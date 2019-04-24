using UnityEngine;
using System.Collections;

public class BodyContainer : MonoBehaviour {

	public float inFrontOfBodyRayCastLength = 3f;
	private Transform feet, head, center;

	void Awake() {
		feet = this.transform.Find ("Feet");
		center = this.transform.Find ("Center");
		head = this.transform.Find ("Head");
	}

	public void Start() {
	}
	
	public void Update() {
	}
	
	public bool AssertIfGroundIsInFrontOfBody(float direction) {
		if(AssertIfGroundIsInFrontOf(feet)) {
			return true;
		}

		if(AssertIfGroundIsInBetween(feet, head)) {
			return true;
		}

		if(AssertIfGroundIsInFrontOf(head)) {
			return true;
		}

		return false;
	}

	private bool AssertIfGroundIsInBetween(Transform firstTransform, Transform secondTransform) {
		bool isObjectinFrontOfBody = false;
		
		RaycastHit raycastHit;
		
		if(firstTransform && secondTransform) {
			Vector3 firstTransformRaycastEnd = firstTransform.position + (firstTransform.right * inFrontOfBodyRayCastLength);
			Vector3 secondTransformRaycastEnd = secondTransform.position + (secondTransform.right * inFrontOfBodyRayCastLength);

			Vector3 distanceBetweenFirstAndSecond = secondTransformRaycastEnd - firstTransformRaycastEnd;
			float distanceLength = distanceBetweenFirstAndSecond.magnitude;

			distanceBetweenFirstAndSecond.Normalize();

			Physics.Raycast(firstTransformRaycastEnd, distanceBetweenFirstAndSecond, out raycastHit, distanceLength);
			Debug.DrawRay(firstTransformRaycastEnd, distanceBetweenFirstAndSecond * distanceLength);
			
			if(raycastHit.collider) {
				if(raycastHit.collider.gameObject.GetComponent<Ground>()) {
					isObjectinFrontOfBody = true;
					OnObjectInFrontOfBody();
				}
			}
		}
		
		return isObjectinFrontOfBody;
	}

	private bool AssertIfGroundIsInFrontOf(Transform usedTransform) {
		bool isObjectinFrontOfBody = false;

		RaycastHit raycastHit;

		if(usedTransform) {
			Physics.Raycast(usedTransform.position, this.transform.right, out raycastHit, inFrontOfBodyRayCastLength);
			Debug.DrawRay(usedTransform.position, this.transform.right * inFrontOfBodyRayCastLength);

			if(raycastHit.collider) {
				if(raycastHit.collider.gameObject.GetComponent<Ground>()) {
					isObjectinFrontOfBody = true;
					OnObjectInFrontOfBody();
				}
			}
		}

		return isObjectinFrontOfBody;
	}

	protected virtual void OnObjectInFrontOfBody(){}
}
