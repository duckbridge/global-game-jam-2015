using UnityEngine;
using System.Collections;

public class BlockTransformComponent : TransformComponent {

	public int blockMass = 1000;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void OnTransform () {
		base.OnTransform ();
		this.rigidbody.mass = blockMass;
	}
}
