using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class Animation2D : DispatchBehaviour {
	
	public float FPS;
	public bool Loop;
	public Sprite[] frames;
	public bool playOnStartup = false;
	public float animationPlayTimeout = 0f;

	public bool isPlayingReverse = false;

	public SpriteRenderer outputRenderer;
	public bool hideWhenDone = false;

	protected float secondsToWait;
	protected bool stopped = false;
	protected bool paused = false;
	protected float animationSpeed = 1f;

	protected int currentFrame;

	// Use this for initialization
	public void Awake () {
		if(!outputRenderer)
			outputRenderer = this.GetComponent<SpriteRenderer>();
			
		currentFrame = 0;
		if(FPS > 0) 
			secondsToWait = 1/FPS;
		else 
			secondsToWait = 0f;

		if(playOnStartup)
			Play(true, isPlayingReverse);

		OnAwake();
	}

	public void Start() {
		OnStart();
	}

	public void SetOutputRenderer(SpriteRenderer outputRenderer) {
		this.outputRenderer = outputRenderer;
	}
	
	public void SetColor(Color newColor) {
		outputRenderer.color = newColor;
	}

	public Color GetColor() {
		return outputRenderer.color;
	}

	public virtual void Animate() {
		CancelInvoke("Animate");
		if(!isPlayingReverse) {
			if(currentFrame >= frames.Length) {
				if(!Loop) {
					stopped = true;
					if(hideWhenDone)
						Hide ();
					OnAnimationDone();
				} else {
					currentFrame = 0;
					DispatchMessage("OnLoopingAnimationDone", this);
				}
			}
		} else {
			if(currentFrame <= -1) {
				if(!Loop) {
					stopped = true;

					if(hideWhenDone)
						Hide ();

					DispatchMessage("OnReverseAnimationDone", this);
				} else {
					currentFrame = frames.Length - 1;
				}
			}
		}
	
		if(outputRenderer.enabled && !stopped) {
			if(!isPlayingReverse)
				OnFrameExited(currentFrame - 1);
			else
				OnFrameExited(currentFrame);

			outputRenderer.sprite = frames[currentFrame];

			if(!isPlayingReverse)
				OnFrameEntered(currentFrame);
			else
				OnFrameEntered(currentFrame - 1);
		}

		if(!stopped) {
			if(!isPlayingReverse) {
				currentFrame++;
			} else {
				currentFrame--;
			}
		}

		if(!stopped && secondsToWait > 0)
			Invoke("Animate", secondsToWait/animationSpeed);
	}

	public void Play(bool reset = false, bool reverse = false, bool useTimeOut = false) {
		this.isPlayingReverse = reverse;
		PlayWithReset(reset);

		if(useTimeOut && animationPlayTimeout > 0) {
			Invoke ("OnAnimationDone", animationPlayTimeout);
		}
	}

	protected virtual void OnAnimationDone() {
		CancelInvoke("OnAnimationDone");
		DispatchMessage("OnAnimationDone", this);
	}
	
	private void PlayWithReset(bool reset) {
		if(reset) {
			if(!isPlayingReverse) {
				currentFrame = 0;
			} else {
				currentFrame = frames.Length - 1;
			}
		}
		
		paused = false;
		stopped = false;
		outputRenderer.enabled = true;
		if(frames.Length > 1)
			Animate();
		else if(frames.Length > 0) {
			if(!isPlayingReverse)
				outputRenderer.sprite = frames[0];
			else 
				outputRenderer.sprite = frames[frames.Length - 1];
		}

		OnPlay();
	}

	public virtual void OnPlay(){}
	public virtual void OnAnimationStopped(){}

	protected virtual void OnStart(){}
	protected virtual void OnAwake() {}

	public virtual void OnFrameEntered(int enteredFrame) {}
	public virtual void OnFrameExited(int exitedFrame) {}

	public void Pause() {
		stopped = true;
		paused = true;
		CancelInvoke("Animate");

	}

	public void Resume() {
		stopped = false;
		paused = false;

		Animate ();
	}

	public void Hide() {
		this.outputRenderer.enabled = false;
	}

	public void Show() {
		this.outputRenderer.enabled = true;
	}

	public void Stop() {
		Pause();
		if(!isPlayingReverse)
			currentFrame = 0;
		else
			currentFrame = frames.Length - 1;

		OnAnimationStopped();
	}

	public void SetSpeed(float newSpeed) {
		if(newSpeed > 0) {
			this.animationSpeed = newSpeed;
		}
	}

	public float GetSpeed() {
		return this.animationSpeed;
	}

	public void SetCurrentFrame(int newFrame) {
		if(newFrame > -1 && newFrame < frames.Length) {
			currentFrame = newFrame;
			outputRenderer.sprite = frames[currentFrame];
		}
	}

	public int GetPreviousFrame() {
		return currentFrame - 1;
	}

	public int GetCurrentFrame() {
		return currentFrame;
	}

	public bool IsPlaying() {
		return !this.stopped;
	}

	public override void OnPauseGame() {}

	public override void OnResumeGame() {}
}