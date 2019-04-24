using UnityEngine;
using System.Collections;

public class UIElement : DispatchBehaviour {
	
	public Transform hideTransform;
	public Transform showTransform;

	public bool isShown = true;
	public float hideShowTime;

	void Start () {}

	// Update is called once per frame
	void Update () {
	
	}

	public void Hide() {
		if(isShown && this.hideTransform) {
			iTween.StopByName(this.gameObject, "Showing");

			iTween.MoveTo(this.gameObject, new ITweenBuilder().SetName("Hiding").SetPosition(hideTransform.position).SetTime(hideShowTime).Build());
			this.isEnabled = false;
			isShown = false;
		}
	}

	public void Show(bool force = false) {
		if((!isShown || force) && this.showTransform) {
			iTween.StopByName(this.gameObject, "Hiding");

			iTween.MoveTo(this.gameObject, new ITweenBuilder().SetName("Showing").SetPosition(showTransform.position).SetTime(hideShowTime).Build());
			this.isEnabled = true;
			isShown = true;
		}
	}

}
