using UnityEngine;
using System.Collections;

public class TextContainer  {
	public TextMesh textMesh;
	public string[] words;
	private int currentWordIndex = 0;

	public TextContainer(TextMesh textMesh, bool resetText = false) {
		this.textMesh = textMesh;
		this.words = textMesh.text.Split(' ');

		if(resetText) textMesh.text = "";
	}

	public void AppendNextWord() {
		textMesh.text += words[currentWordIndex] + " ";
		currentWordIndex++;
	}

	public bool CanDisplayNextWord() {
		return (currentWordIndex < words.Length);
	}
}
