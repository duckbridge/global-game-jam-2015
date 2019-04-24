using UnityEngine;
using System.Collections;

public class SimpleTutorialComponent : MonoBehaviour {

	public iTween.EaseType easeTypeToUse;
	public float showHideTime = .5f;
	public Transform showPosition;
	public Transform hidePosition;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Show() {

		this.active = true;

		if(this.transform.Find("XboxIcon") && this.transform.Find("KeyboardIcon")) {
			Transform xboxIcon = this.transform.Find("XboxIcon");
			Transform keyboardIcon = this.transform.Find("KeyboardIcon");


			xboxIcon.active = ControllerRumbleComponent.IsXboxControllerPluggedIn();
			keyboardIcon.active = !ControllerRumbleComponent.IsXboxControllerPluggedIn();
		}

		iTween.StopByName(this.gameObject, "Hiding");
		iTween.MoveTo(this.gameObject, new ITweenBuilder().SetPosition(showPosition.position).SetTime(showHideTime).SetName("Showing").SetEaseType(easeTypeToUse).Build());
	}

	public void ShowForTime(float time) {
		Show ();

		CancelInvoke("Hide");
		Invoke ("Hide", time);
	}

	public void Hide() {
		iTween.StopByName(this.gameObject, "Showing");
		iTween.MoveTo(this.gameObject, new ITweenBuilder().SetPosition(hidePosition.position).SetTime(showHideTime).SetName("Hiding").SetEaseType(easeTypeToUse).Build());
	}
}
