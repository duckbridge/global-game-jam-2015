using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextExplanation : DispatchBehaviour {

	public Camera textExplanationCamera;
	public List<string> content;
	private int currentIndex = 0;
	public TextMesh textOutput;
	public bool isEnabled = false;

	public void Enable(bool showInitialLine) {
		this.isEnabled = true;
		this.textExplanationCamera.active = true;
		if(showInitialLine) {
			ShowLine();
		}
	}

	public void Disable() {
		this.isEnabled = false;
		this.textExplanationCamera.active = false;
	}

	void Start () {
		if(!isEnabled) {
			Disable();
		}
	}
	
	public void ShowLine() {
		if(currentIndex < content.Count) {
			textOutput.text = content[currentIndex];
		} else {
			Disable();
			DispatchMessage("OnTextFinished", this);
		}
	}

	public void GoToNextLine() {
		currentIndex++;
	}

	void Update () {
		if(isEnabled) {
			if(Input.GetKeyDown(KeyCode.Return)) {
				GoToNextLine();
				ShowLine();
			}
		}
	}
}
