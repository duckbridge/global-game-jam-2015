using UnityEngine;
using System.Collections;

public class SimpleUnityAnimationController : MonoBehaviour {

	public string clipName;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Play() {
		this.animation.Play(clipName);
	}

	public void DoPause() {
		this.animation[clipName].speed = 0f;
	}

	public void DoResume() {
		this.animation[clipName].speed = 1f;
	}
}
