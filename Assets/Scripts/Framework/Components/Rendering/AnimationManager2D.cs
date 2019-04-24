using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AnimationManager2D : MonoBehaviour {

	private List<Animation2D> animations;
	protected Dictionary<string, Animation2D> animationsByName = new Dictionary<string, Animation2D>();

	private Animation2D currentAnimation;

	public void Start () {
		animations = new List<Animation2D>(this.GetComponentsInChildren<Animation2D>());

		foreach(Animation2D animation in animations) {
			if(!animationsByName.ContainsKey(animation.name)) {
				animationsByName.Add(animation.name, animation);

				if(!animation.playOnStartup) {
					animation.Stop();
					animation.Hide();
				} else {
					currentAnimation = animation;
				}
			}
		}
	}

	public void ResumeAnimationByName(string animationName) {
		Animation2D foundAnimation;
		animationsByName.TryGetValue(animationName, out foundAnimation);

		if(foundAnimation && foundAnimation != currentAnimation && !foundAnimation.IsPlaying()) {
			PlayAnimationByName(animationName, false);
		}
	}

	public void ResumeAnimationSynced(string animationName,  bool useTimeOut = false) {
		Animation2D foundAnimation;
		animationsByName.TryGetValue(animationName, out foundAnimation);
		
		if(foundAnimation) {
			if(currentAnimation != foundAnimation) {
				int savedFrame = 0;
				if(currentAnimation) {
					savedFrame = currentAnimation.GetPreviousFrame();
					currentAnimation.Stop();
					currentAnimation.Hide ();
				}

				foundAnimation.SetCurrentFrame(savedFrame);
				foundAnimation.Show ();
				foundAnimation.Play(false, false, useTimeOut);

			} 
			currentAnimation = foundAnimation;
		}
	}

	public void StopHideAnimationByName(string animationName) {
		Animation2D foundAnimation;
		animationsByName.TryGetValue(animationName, out foundAnimation);
		if(foundAnimation) {
			foundAnimation.Stop();
			foundAnimation.Hide ();
		}
	}

	public void PlayAnimationByNameReversed(string animationName, bool reset = false) {
		Animation2D foundAnimation;
		animationsByName.TryGetValue(animationName, out foundAnimation);
		
		if(foundAnimation) {
			
			if(currentAnimation) {
				currentAnimation.Stop();
				currentAnimation.Hide ();
			}

			foundAnimation.Show ();
			foundAnimation.Play(reset, true, false);
			
			currentAnimation = foundAnimation;
		}
	}

	public void PlayAnimationByName(string animationName, bool reset = false, bool useTimeOut = false) {
		Animation2D foundAnimation;
		animationsByName.TryGetValue(animationName, out foundAnimation);

		if(foundAnimation) {

			if(currentAnimation) {
				currentAnimation.Stop();
				currentAnimation.Hide ();
			}

			foundAnimation.Show ();
			foundAnimation.Play(reset, false, useTimeOut);

			currentAnimation = foundAnimation;
		}
	}

	public void SetLastFrameForAnimation(string animationName, bool pauseAnimation = false) {
		Animation2D foundAnimation;
		animationsByName.TryGetValue(animationName, out foundAnimation);
		
		if(foundAnimation) {
			if(pauseAnimation) {
				foundAnimation.Pause ();
			}
			foundAnimation.SetCurrentFrame(foundAnimation.frames.Length - 1);
		}
	}

	public void SetFrameForAnimation(string animationName, int frame, bool pauseAnimation = false) {
		Animation2D foundAnimation;
		animationsByName.TryGetValue(animationName, out foundAnimation);
		
		if(foundAnimation) {
			if(pauseAnimation) {
				foundAnimation.Pause ();
			}
			foundAnimation.SetCurrentFrame(frame);
		}
	}

	public void StopHideIndependentAnimationByName(string animationName) {
		Animation2D foundAnimation;
		animationsByName.TryGetValue(animationName, out foundAnimation);
		
		if(foundAnimation) {
			foundAnimation.Hide();
			foundAnimation.Stop();
		}
	}

	public void PlayIndependentAnimationByName(string animationName, bool reset = false, bool reverse = false, bool useTimeOut = false) {
		Animation2D foundAnimation;
		animationsByName.TryGetValue(animationName, out foundAnimation);
		
		if(foundAnimation) {
			foundAnimation.Show ();
			foundAnimation.Play(reset, reverse, useTimeOut);
		}
	}

	public void AddEventListenerTo(string animationName, GameObject listener) {
		Animation2D foundAnimation;
		animationsByName.TryGetValue(animationName, out foundAnimation);
		
		if(foundAnimation) {
			foundAnimation.AddEventListener(listener);
		}
	}
	
	public void RemoveEventListenerFrom(string animationName, GameObject listener) {
		Animation2D foundAnimation;
		animationsByName.TryGetValue(animationName, out foundAnimation);
		
		if(foundAnimation) {
			foundAnimation.RemoveEventListener(listener);
		}
	}

	public Animation2D GetAnimationByName(string animationName) {
		Animation2D foundAnimation = null;
		animationsByName.TryGetValue(animationName, out foundAnimation);

		return foundAnimation;
	}

	public List<Animation2D> GetAnimations() {
		return this.animations;
	}
}
