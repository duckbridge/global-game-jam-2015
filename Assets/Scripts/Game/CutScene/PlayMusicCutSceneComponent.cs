using UnityEngine;
using System.Collections;

public class PlayMusicCutSceneComponent : CutSceneComponent {

	public SoundObject soundToPlay;
	public float timeToFinish = 5f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void OnActivated () {
		soundToPlay.Play();
		Invoke ("DeActivate", timeToFinish);
	}
}
