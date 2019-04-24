using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CharacterPicker : DispatchBehaviour {

	public WizardKing wizardKing;

	public PressActionDisplay selectKingActionDisplay, selectPeasantActionDisplay, pressJumpActionDisplay, pressTransformActionDisplay;
	public float cursorMoveTimeout = .5f;
	public GameObject cursorContainer;

	private bool isActivated = false;
	private bool canMoveCursor = true;

	private int currentIndex = 0;
	private List<Transformer> allTransformers;
	private int originalTotalAmountOfTransformers;
	// Use this for initialization
	void Start () {
		wizardKing.AddEventListener(this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		if(isActivated) {
			if(canMoveCursor) {
				if(Input.GetAxis("Horizontal") > 0) {

					allTransformers[currentIndex].OnUnSelected();

					currentIndex++;

					if(currentIndex >= allTransformers.Count) {
						currentIndex = 0;
					}

					MoveCursorToCurrentIndex();
				}

				if(Input.GetAxis("Horizontal") < 0) {

					allTransformers[currentIndex].OnUnSelected();

					currentIndex--;
					
					if(currentIndex < 0) {
						currentIndex = allTransformers.Count - 1;
					}
					
					MoveCursorToCurrentIndex();
				}
			}

			if(Input.GetButtonDown("menu_select_transformer")) {
				OnTransformerChosen(allTransformers[currentIndex]);
			}

			if(Input.GetButtonDown("menu_select_king")) {
				DispatchMessage("OnKingSelected", null);
				OnSelectingDone();
				pressTransformActionDisplay.Show ("Throw King");
			}
		}
	}

	public void SetTotalAmountOfTransformers(int totalAmountOfTransformers) {
		this.originalTotalAmountOfTransformers = totalAmountOfTransformers;
	}

	public void Initialize(List<Transformer> transformers) {
		this.gameObject.SetActive(true);
		if(transformers.Count > 0) {
			this.allTransformers = transformers;
			wizardKing.OnBeforeSelectingPeasant();
		}
	}

	public void OnWizardDoneTalking() {
	
		wizardKing.GetKingCharacterControl().AddEventListener(this.gameObject);
		wizardKing.GetKingCharacterControl().StartPointing();
	}


	public void OnPointingDone() {

		currentIndex = 0;
		isActivated = true;
		
		selectKingActionDisplay.Show ("Select King");
		selectPeasantActionDisplay.Show ("Select Follower");

		cursorContainer.active = true;
		MoveCursorToCurrentIndex();
	}

	private void OnTransformerChosen(Transformer transformerChosen) {
		OnSelectingDone();
		wizardKing.GetKingCharacterControl().OnPuttingOnHat();
		DispatchMessage("OnTransformerChosen", transformerChosen);
	}

	private void OnSelectingDone() {
		cursorContainer.active = false;
		Transformer selectedTransformer = allTransformers[currentIndex];

		selectedTransformer.OnUnSelected();
		CancelInvoke("ResetCursorMovement");
		this.isActivated = false;

		selectKingActionDisplay.Hide ();
		selectPeasantActionDisplay.Hide ();

		pressJumpActionDisplay.Show ("Jump");
		if(selectedTransformer is BulkyGuy) {
			pressTransformActionDisplay.Show ("Prepare to Catch King");
		} else {
			pressTransformActionDisplay.Show ("Transform Follower");
		}

		this.gameObject.SetActive(false);
	}

	public void ShowJumpAndThrowCommands() {
		pressJumpActionDisplay.Show ("Jump");
		pressTransformActionDisplay.Show ("Throw King");
	}
	
	public void HideTransformerButtons() {
		pressJumpActionDisplay.Hide ();
		pressTransformActionDisplay.Hide ();
	}
	
	private void MoveCursorToCurrentIndex() {
		canMoveCursor = false;

		wizardKing.OnPointingAtPeasant((float)currentIndex/(float)originalTotalAmountOfTransformers);
		CancelInvoke("ResetCursorMovement");
		Invoke ("ResetCursorMovement", cursorMoveTimeout);

		cursorContainer.transform.position = 
			new Vector3(allTransformers[currentIndex].transform.position.x, cursorContainer.transform.position.y, cursorContainer.transform.position.z);
		allTransformers[currentIndex].OnSelected();
	}

	private void ResetCursorMovement() {
		this.canMoveCursor = true;
	}
}
