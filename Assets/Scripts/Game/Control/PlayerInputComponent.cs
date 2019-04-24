using UnityEngine;
using System.Collections;

public class PlayerInputComponent : DispatchBehaviour {

	private CharacterControl playerControl;
	private BodyControl bodyControl;
	private Player player;

	private bool downPressedController = false;
	private bool downPressedKeyboard = false;

	public void Awake() {
		playerControl = GetComponent<CharacterControl>();
		bodyControl = GetComponent<BodyControl>();
		player = GetComponent<Player>();
	}

	public void Update() {
		playerControl.DoMove(Input.GetAxis("Horizontal"));
		
		if(Input.GetButton("Jump")) {
			playerControl.DoJump();
		}
		
		if(Input.GetButtonUp ("Jump")) {
			playerControl.DoStopJump();
		}

		if(Input.GetButtonDown("Transform")) {
			if(player is Transformer && !GetComponent<Transformer>().IsTransformed()) {
				SceneUtils.FindObject<WizardKing>().CastMagic();
				GetComponent<Transformer>().DoTransform();
			}

			if(player is WizardKing) {
				GetComponent<WizardKing>().DoThrow();
			}
		}

		if(Input.GetButtonDown("Down")) {
			downPressedKeyboard = true;
		} 
		
		if(Input.GetAxis("Down") > 0.6) {
			downPressedController = true;
		}
		
		if(downPressedKeyboard) {
			if(Input.GetButtonUp("Down")) {
				downPressedKeyboard = false;
			}
		}
		
		if(downPressedController) {
			if(Input.GetAxis("Down") <= 0) {
				downPressedController = false;
			}
		}
	}


	public override void OnPauseGame () {
		this.enabled = false;
	}

	public override void OnResumeGame() {
		this.enabled = true;
	}
}
