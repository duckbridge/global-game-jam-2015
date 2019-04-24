using UnityEngine;
using System.Collections;

public class DoSpecialEffectsCutScene : CutSceneComponent {

	public Animation animationToPlay;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void OnActivated () {
		animationToPlay.Play ();

		DeActivate();
	}
}
