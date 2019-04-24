using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextCutSceneComponent : CutSceneComponent {

	public List<TextBox> textBoxes;
	private int currentTextBoxIndex = -1;

	public override void OnActivated() {
		ShowNextTextBox();
	}

	public override void OnDeActivated() {
		
	}

	public void OnTextDone() {
		ShowNextTextBox();
	}

	private void ShowNextTextBox() {
		textBoxes[currentTextBoxIndex].active = false;

		currentTextBoxIndex++;
		if(currentTextBoxIndex < textBoxes.Count) {
			textBoxes[currentTextBoxIndex].active = true;
		} else {
			OnFinished();
		}
	}
}
