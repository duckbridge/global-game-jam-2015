using UnityEngine;
using System.Collections;

public class WizardKing : Player {

	public Vector3 throwPower;
	public WizardKingSolo wizardKingSoloPrefab;

	public float throwTextBoxHideTimeout = 4f;
	public float jumpTextBoxHidetimeout = .5f;
	public float onDoneTalkingToPeasantsTimeout = 2f;

	public GameObject onJumpCommand, onThrowCommand;
	public TextBoxManager onSelectedPeasantCommand;

	private SoundObject clapSound;

	private bool canShowCommandText = true;

	void Awake() {
		clapSound = this.transform.Find("Sounds/ClapSound").GetComponent<SoundObject>();
	}

	public void OnJumped(CharacterControl characterControl) {

		if(canShowCommandText) {
			if(characterControl is BulkyGuyCharacterControl) {
			
				BulkyGuyCharacterControl bulkyGuyCharacterControl = (BulkyGuyCharacterControl) characterControl;
				if(!bulkyGuyCharacterControl.IsCarryingKing()) {
					if(!onJumpCommand.active) {
						onJumpCommand.SetActive(true);
						onJumpCommand.GetComponentInChildren<SoundObject>().Play();

						CancelInvoke("HideOnJumpCommandTextBoxDelayed");
						Invoke ("HideOnJumpCommandTextBoxDelayed", jumpTextBoxHidetimeout);
					}
				}

			} else if(characterControl is TransformerCharacterControl) {
				if(!onJumpCommand.active) {
					onJumpCommand.SetActive(true);
					onJumpCommand.GetComponentInChildren<SoundObject>().Play();

					CancelInvoke("HideOnJumpCommandTextBoxDelayed");
					Invoke ("HideOnJumpCommandTextBoxDelayed", jumpTextBoxHidetimeout);
				}
			}
		}
	}

	public void OnThrowing() {
		if(canShowCommandText) {
			onThrowCommand.SetActive(true);
			onThrowCommand.GetComponentInChildren<SoundObject>().Play();

			CancelInvoke("HideOnThrowCommandTextBoxDelayed");
			Invoke ("HideOnThrowCommandTextBoxDelayed", throwTextBoxHideTimeout);
		}
	}

	private void HideSelectPeasantTextBoxDelayed() {
		onSelectedPeasantCommand.Hide ();
	}

	private void HideOnJumpCommandTextBoxDelayed() {
		onJumpCommand.SetActive(false);
	}

	private void HideOnThrowCommandTextBoxDelayed() {
		onThrowCommand.SetActive(false);
	}

	public void HideShoutCommands() {
		onJumpCommand.SetActive(false);
		onSelectedPeasantCommand.Hide();
		onThrowCommand.SetActive(false);
	}

	public void DoThrow() {
		OnThrowing();

		Direction direction = GetComponent<BodyControl>().GetDirection();

		WizardKingSolo wizardKingSolo = (WizardKingSolo) GameObject.Instantiate(wizardKingSoloPrefab, wizardKingSoloPrefab.transform.position, Quaternion.identity);

		GetComponent<KingCharacterControl>().SetNotCarryingKing();
		GetComponent<KingCharacterControl>().StandStill();
		GetComponent<KingCharacterControl>().DisableAnimationSwitching();

		PhysicsUtils.IgnoreCollisionBetween(wizardKingSolo.collider, this.collider);

		wizardKingSolo.gameObject.SetActive(true);
		wizardKingSolo.transform.parent = null;
		wizardKingSolo.rigidbody.AddForce(new Vector3(throwPower.x * (float)direction, throwPower.y), ForceMode.Impulse);
		wizardKingSolo.OnThrown();

		DispatchMessage("OnWizardThrown", wizardKingSolo);

		GetComponent<PlayerInputComponent>().enabled = false;
	}

	public void OnSelected() {
		canShowCommandText = false;

		onThrowCommand.SetActive(false);
		onJumpCommand.SetActive(false);
		onSelectedPeasantCommand.Hide ();
	}

	public void OnBeforeSelectingPeasant() {
		onSelectedPeasantCommand.ResetShowAndActivate();

		CancelInvoke("HideSelectPeasantTextBoxDelayed");
		Invoke ("HideSelectPeasantTextBoxDelayed", onDoneTalkingToPeasantsTimeout);

		CancelInvoke("OnDoneTalkingToPeasants");
		Invoke ("OnDoneTalkingToPeasants", onDoneTalkingToPeasantsTimeout);
	}

	private void OnDoneTalkingToPeasants() {
		DispatchMessage("OnWizardDoneTalking", null);
	}

	public void CastMagic() {
		GetKingCharacterControl().OnCastingMagic();
	}
	
	public void OnPointingAtPeasant(float distanceInPercentage) {
		GetKingCharacterControl().OnPointing(distanceInPercentage);
	}

	public override void OnDie () {
		base.OnDie();

		bloodSpawner.SpawnBlood();
		DispatchMessage("OnWizardKingDied", this);
	}

	public KingCharacterControl GetKingCharacterControl() {
		return (KingCharacterControl) GetCharacterControl();
	}
}
