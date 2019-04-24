using UnityEngine;
using System.Collections;

public class SimpleTutorialWithAnimation : MonoBehaviour {

	public SoundObject spraySound;
	public Animation2D animationToPlay, extraAnimationToPlay;

	public GameObject contentToShow;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void OnTriggerEnter(Collider coll) {
		Player player = coll.gameObject.GetComponent<Player>();
		if(player) {
			Show();
			this.collider.enabled = false;
		}
	}

	public void Show() {
		animationToPlay.AddEventListener(this.gameObject);
		animationToPlay.Play(true);
		
		if(extraAnimationToPlay) {
			extraAnimationToPlay.Play(true);
		}

		if(spraySound) {
			SoundUtils.SetSoundVolumeToSavedValue(SoundType.FX);
			spraySound.Play();
		}
	}

	public void OnAnimationDone(Animation2D animation2D) {
		if(animation2D.name == animationToPlay.name) {
			contentToShow.active = true;
		}
	}
}
