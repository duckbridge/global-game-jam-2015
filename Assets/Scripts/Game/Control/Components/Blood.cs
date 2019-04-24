using UnityEngine;
using System.Collections;

public class Blood : MonoBehaviour {

	public float colliderReEnableTimeout = .5f;
	public float destroyTimeout = 3f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnSpawn(Vector3 direction, float force) {
		this.transform.parent = null;
		this.collider.enabled = false;

		this.rigidbody.AddForce(direction * force, ForceMode.Impulse);
		Invoke ("ReEnableCollider", colliderReEnableTimeout);
		Invoke ("DoDestroy", destroyTimeout);
	}

	private void ReEnableCollider() {
		this.collider.enabled = true;
	}

	private void DoDestroy() {
		Destroy (this.gameObject);
	}
}
