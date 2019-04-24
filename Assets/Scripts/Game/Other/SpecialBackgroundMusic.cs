using UnityEngine;
using System.Collections;

public class SpecialBackgroundMusic : SoundObject {

	void Awake() {
		this.GetSound().time = GameSettings.SAVEDBACKGROUNDMUSICTIME;
		Play ();
	}

	public void SaveBackgroundMusicTime() {
		GameSettings.SAVEDBACKGROUNDMUSICTIME = GetSound().time;
	}
}
