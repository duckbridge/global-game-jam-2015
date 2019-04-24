using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextBoxManager : DispatchBehaviour {
	
	public SoundObject onShowSound;

	public List<TextBox> textBoxes;
	public List<SoundObject> talkSounds;

	public bool activateOnAwake = true;
	
	private int currentTextBox = 0;
	private bool spaceKeyDown = false;
	private bool textBoxIsDone = false;
	private bool isPaused = false;
	private bool isBusy = false;

	private SoundObject currentTalkSound;

	private Animation2D onShowAnimation;
	private Animation2D onHideAnimation;

	// Use this for initialization
	void Awake () {
		if(activateOnAwake)
			OnActivated();
	}

	public void OnActivated() {
		foreach(TextBox textBox in textBoxes) {
			textBox.active = false;
			SoundUtils.SetSoundVolumeToSavedValue(SoundType.FX);
		}

		Transform onShowAnimationsTransform = this.transform.Find("Animations/OnShowAnimation");
		Transform onHideAnimationsTransform = this.transform.Find("Animations/OnHideAnimation");

		if(onShowAnimationsTransform && onHideAnimationsTransform) {
		
			onShowAnimation = this.transform.Find("Animations/OnShowAnimation").GetComponent<Animation2D>();
			onHideAnimation = this.transform.Find("Animations/OnHideAnimation").GetComponent<Animation2D>();

			onShowAnimation.AddEventListener(this.gameObject);
			onHideAnimation.AddEventListener(this.gameObject);

			onHideAnimation.Stop ();
			onHideAnimation.Hide ();

			onShowAnimation.Show ();
			onShowAnimation.Awake ();
			onShowAnimation.Play (true);

			isBusy = true;

		} else {

			ShowNextTextBalloon();
		}

		if(onShowSound) {
			SoundUtils.SetSoundVolumeToSavedValue(SoundType.FX);
			onShowSound.Play();
		}
	}

	private void OnAnimationDone(Animation2D animation2D) {
		if(animation2D.name == "OnShowAnimation") {
			ShowNextTextBalloon();
			isBusy = false;
		}

		if(animation2D.name == "OnHideAnimation") {
			isBusy = false;
			this.active = false;
		}
	}
	void Update () {
        
		if(!isPaused && !isBusy) {
			if(Input.GetButtonDown("menu_select")) {

				if(!textBoxIsDone) {
					textBoxes[currentTextBox - 1].FinishTextBox();
					textBoxIsDone = true;

					return;
				}

				if(!spaceKeyDown && textBoxIsDone) {
					spaceKeyDown = true;
					textBoxIsDone = false;

					ShowNextTextBalloon();	
				}
			}

			if(spaceKeyDown) {
				if(Input.GetButtonUp("menu_select")) {
					spaceKeyDown = false;
				}
			}
		}
	}

	public void OnTextDone(TextBox textBox) {
		textBoxIsDone = true;
		//SceneUtils.FindObject<PressActionDisplay>().Show("Continue");
		DispatchMessage("OnTextPartDone", null);
	}

	public void OnShowNextWord() {
		if(talkSounds.Count > 0) {
			int randomTalkIndex = Random.Range(0, talkSounds.Count);
			if(currentTalkSound) {
				currentTalkSound.Stop();
			}
			currentTalkSound = talkSounds[randomTalkIndex];
			currentTalkSound.Play();
		}

		DispatchMessage("OnShowNextWord", null);
	}
	
	private void ShowNextTextBalloon() {
		
		if(currentTextBox < textBoxes.Count) {
			//SceneUtils.FindObject<PressActionDisplay>().Show("Skip");
			DispatchMessage("OnShowNextTextBalloon", null);

			if(currentTextBox > 0) {
				textBoxes[currentTextBox - 1].RemoveEventListener(this.gameObject);
				textBoxes[currentTextBox - 1].active = false;
			}

			textBoxes[currentTextBox].AddEventListener(this.gameObject);
			textBoxes[currentTextBox].active = true;
			textBoxes[currentTextBox].OnStart();

			currentTextBox++;

		} else {
			if(onHideAnimation) {
				isBusy = true;
				textBoxes[currentTextBox - 1].RemoveEventListener(this.gameObject);
				textBoxes[currentTextBox - 1].active = false;
			}

			DispatchMessage("OnTextDone", null);
			SceneUtils.FindObject<PressActionDisplay>().Hide ();

			Hide();
		}
	}

	public void Show() {
		this.active = true;
	}

	public void Hide() {
		if(this.onHideAnimation) {
			onShowAnimation.Hide ();
			onShowAnimation.Stop();

			onHideAnimation.Show ();
			onHideAnimation.Play(true);

		} else {
			this.active = false;
		}
	}

	public void ResetShowAndActivate() {
		Reset ();
		Show();
		OnActivated();
	}

	public void Reset() {
		foreach(TextBox textBox in textBoxes) {
			textBox.RemoveEventListener(this.gameObject);
			textBox.Reset ();
			textBox.active = false;
		}
		currentTextBox = 0;
	}

	public override void OnPauseGame() {
		isPaused = true;
	}
	public override void OnResumeGame() {
		isPaused = false;
	}

	public bool IsBusy() {
		return isBusy;
	}

}
