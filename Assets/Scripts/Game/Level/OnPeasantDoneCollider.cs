using UnityEngine;
using System.Collections;

public class OnPeasantDoneCollider : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnTriggerEnter(Collider coll) {
		Transformer transformer = coll.gameObject.GetComponent<Transformer>();
		if(transformer) {
			if(transformer is BulkyGuy) {
				return;
			}
			transformer.OnReachedEndOfLevel();
		}
	}
}
