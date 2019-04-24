using UnityEngine;
using System.Collections;

public class MenuButton : DispatchBehaviour {

	public MenuButtonType menuButtonType;
	private bool isMovable = true;

	public virtual void Start() {
	}
	
	public void Update() {
	}
	
	public virtual void OnPressed() {
		DispatchMessage("OnMenuButtonPressed", this.menuButtonType);
	}

	public virtual void OnObjectClicked() {
		DispatchMessage("OnMenuButtonPressed", this.menuButtonType);
	}

	public virtual void Disable() {
		isMovable = false;
		GetComponent<TextMesh>().renderer.enabled = false;
	}

	public virtual void Enable() {
		isMovable = true;
		GetComponent<TextMesh>().renderer.enabled = true;
	}	

	public bool IsMovable() {
		return isMovable;
	}

	public virtual void OnSelected() {}
	public virtual void OnUnSelected() {}
}
