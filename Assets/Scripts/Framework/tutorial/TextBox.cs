using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextBox: DispatchBehaviour {

	private List<TextContainer> textContainers = new List<TextContainer>();
	public List<TextMesh> textMeshesSetInEditor;

	private TextMesh[] textMeshesUsed;
	private List<string> originalTextMeshText;

	public float textTimeout;
	private string totalString;

	private int currentTextMeshIndex = 0;
	public bool isInitializedManually = false;

	// Use this for initialization
	void Awake () {
		if(!isInitializedManually) {
			Initialize();
		}
	}

	public void Update() {

	}

	public void Start() {

	}

	public void Initialize() {
		textContainers = new List<TextContainer>();
		originalTextMeshText = new List<string>();

		if(textMeshesSetInEditor.Count == 0) {
			textMeshesUsed = this.GetComponentsInChildren<TextMesh>();
		} else {
			textMeshesUsed = textMeshesSetInEditor.ToArray();
		}
		
		for(int i = 0 ; i < textMeshesUsed.Length; i++) {
			originalTextMeshText.Add (textMeshesUsed[i].text);
			TextContainer textContainer = new TextContainer(textMeshesUsed[i], true);
			textContainers.Add(textContainer);
		}
	}

	public void OnStart() {
		ShowNextWord();
	}

	public void FinishTextBox() {
		CancelInvoke("ShowNextWord");

		for(int i = 0; i < textMeshesUsed.Length ;i++) {
			textMeshesUsed[i].text = originalTextMeshText[i];
		}
		DispatchMessage("OnTextDone", this);
	}

	private void ShowNextWord() {
		CancelInvoke("ShowNextWord");

		TextContainer currentTextContainer = textContainers[currentTextMeshIndex];

		if(currentTextContainer.CanDisplayNextWord()) {
			currentTextContainer.AppendNextWord();
			DispatchMessage("OnShowNextWord", null);
			Invoke("ShowNextWord", textTimeout);
		} else {
		
			if(currentTextMeshIndex < textContainers.Count - 1) {
				currentTextMeshIndex++;
				currentTextContainer = textContainers[currentTextMeshIndex];
				Invoke("ShowNextWord", textTimeout);
				return;
			}

			DispatchMessage("OnTextDone", this);
			return;
		}
	}

	public void Reset() {
		Initialize();
		currentTextMeshIndex = 0;
	}
}
