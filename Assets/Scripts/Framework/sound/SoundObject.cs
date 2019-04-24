using UnityEngine;
using System.Collections;

public class SoundObject : DispatchBehaviour {

	public SoundType soundType = SoundType.FX;
	public float soundVolumePercentage = 1f;

	protected float originalVolume;
	private float originalTimeScale;

	protected bool isMuted = false;
	protected bool destroyOnDone = false;
	private bool isPaused = false;

	public virtual void Start() {
	}
	
	public void FixedUpdate() {
		if(destroyOnDone && !GetSound().isPlaying) {
			Destroy (this.gameObject);
		}
	}
	
	// Use this for initialization
	public virtual void Awake () {
		originalVolume = GetSound().volume;
		originalTimeScale = GetSound().pitch;
	}

	public AudioSource GetSound() {
		return this.GetComponent<AudioSource>();
	}
	
	public void SetVolume(float newVolume) {
		if(newVolume > 0) {
			this.GetSound().volume = newVolume * soundVolumePercentage;
		} else {
			if(newVolume < 0) {
				this.GetSound().volume = 0;
			} else {
				this.GetSound().volume = newVolume;
			}
		}
	}

	public float GetVolume() {
		return GetSound().volume;
	}

	public void SetTimeScale(float newTimeScale) {
		this.GetSound().pitch = newTimeScale;
	}

	public void ResetTimeScale() {
		this.GetSound().pitch = originalTimeScale;
	}

	public void Play(bool force = true) {
		if(active) {
			if(force) {

				GetSound().Play();

			} else if(!GetSound().isPlaying) {

				GetSound().Play();
			
			}

		}
	}

	public void Resume() {
		if(isPaused) {
			isPaused = false;
			GetSound().Play ();
		}
	}

	public void Pause() {
		isPaused = true;
		GetSound().Pause();
	}

	public void PlayIndependent(bool destroyOnDone = true) {
		this.transform.parent = null;
		Play ();
		this.destroyOnDone = destroyOnDone;
	}


	public void Stop() {
		GetSound().Stop();
	}

	public void Mute() {
		GetSound().volume = 0f;
		isMuted = true;
	}

	public void UnMute() {
		GetSound().volume = originalVolume;
		isMuted = false;
	}
}
