using UnityEngine;
using System.Collections;

public class TransformComponent : MonoBehaviour {

	public Collider normalCollider, transformedCollider;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public virtual void OnTransform() {
		normalCollider.enabled = false;
		transformedCollider.enabled = true;

		GetComponent<Ground>().enabled = true;
		this.transform.Find("FeetCollider").collider.enabled = false;
	}

}
