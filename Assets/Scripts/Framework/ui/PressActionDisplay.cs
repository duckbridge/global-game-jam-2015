using UnityEngine;
using System.Collections;

public class PressActionDisplay : MonoBehaviour {

	public bool showOnAwake = false;

	private Transform xboxIcon, specialXboxIcon;
	private Transform keyboardIcon, specialKeyboardIcon;

	private bool isInitialized = false;
	private TextMesh interractText;
	private string originalInterractText;

	public void Awake() {
		Initialize();
		originalInterractText = interractText.text;

		if(showOnAwake) {
			Show ();
		} else {
			Hide ();
		}
	}

	public void Start() {}
	
	public void Update() {}

	public void Show(string newText = "", bool showSpecialActionIcon = false) {

		if(newText != "") {
			interractText.text = newText;
		} else {
			interractText.text = originalInterractText;
		}

		Initialize();

		interractText.active = true;

		if(showSpecialActionIcon && specialXboxIcon && specialKeyboardIcon) {

			xboxIcon.active = false;
			keyboardIcon.active = false;

			specialXboxIcon.active = ControllerRumbleComponent.IsXboxControllerPluggedIn();
			specialKeyboardIcon.active = !ControllerRumbleComponent.IsXboxControllerPluggedIn();
		
		} else {

			if(specialXboxIcon && specialKeyboardIcon) {
				specialXboxIcon.active = false;
				specialKeyboardIcon.active = false;
			}

			xboxIcon.active = ControllerRumbleComponent.IsXboxControllerPluggedIn();
			keyboardIcon.active = !ControllerRumbleComponent.IsXboxControllerPluggedIn();
		
		}
	}

	public void Hide() {

		Initialize();

		interractText.active = false;

		xboxIcon.active = false;
		keyboardIcon.active = false;

		if(specialXboxIcon && specialKeyboardIcon) {
			specialXboxIcon.active = false;
			specialKeyboardIcon.active = false;
		}
	}

	private void DoSpecialInitialize() {

	}

	private void Initialize() {
		if(!isInitialized) {
			isInitialized = true;

			interractText = GetComponentInChildren<TextMesh>();
			
			xboxIcon = this.transform.Find ("XboxIcon");
			keyboardIcon = this.transform.Find ("KeyboardIcon");

			specialXboxIcon = this.transform.Find ("SpecialXboxIcon");
			specialKeyboardIcon = this.transform.Find ("SpecialKeyboardIcon");
		}
	}
}
