using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Menu : DispatchBehaviour {

	public List<MenuButton> menuButtons;
	public bool isActive = true;
	public Scene sceneAfterStartPressed;
	public float menuStateResetTime = .5f;

	public SoundObject onMoveDownSound, onMoveUpSound;

	protected MenuButton currentMenuButton;
	protected int currentIndex = 0;

	private enum MenuState { MOVING, IDLE, BUSY }
	private MenuState menuState = MenuState.IDLE;

	private bool hasUsedKeyboardToNavigate = false;
	private bool hasUsedGamePadToNavigate = false;

	// Use this for initialization
	public virtual void Start () {
		menuButtons.ForEach(button => button.AddEventListener(this.gameObject));

		currentMenuButton = menuButtons[0];
		currentMenuButton.OnSelected();
	}


	private void OnMoveToPreviousButton() {
		if(onMoveUpSound) {
			onMoveUpSound.Play();
		}
		menuButtons[currentIndex].OnUnSelected();
		--currentIndex;
		if(currentIndex < 0) {
			currentIndex = menuButtons.Count - 1;
		}

		if(!menuButtons[currentIndex].IsMovable()) {
			OnMoveToPreviousButton();
		}

		menuButtons[currentIndex].OnSelected();
	}

	private void OnMoveToNextButton() {
		if(onMoveDownSound) {
			onMoveDownSound.Play();
		}
		menuButtons[currentIndex].OnUnSelected();
		++currentIndex;

		if(currentIndex >= menuButtons.Count) {
			currentIndex = 0;
		}

		if(!menuButtons[currentIndex].IsMovable()) {
			OnMoveToNextButton();
		}

		menuButtons[currentIndex].OnSelected();
	}

	public virtual void OnMenuButtonPressed(MenuButtonType menuButtonType) {
		if(!this.isActive) {
			return;
		}

		switch(menuButtonType) {
			case MenuButtonType.EXIT:
				Application.Quit();
			break;
			case MenuButtonType.STARTGAME:
				OnStartNewGame();
			break;
			case MenuButtonType.LOADGAME:
				OnLoadGame();
			break;
		}
		this.isActive = false;
	}

	protected virtual void OnStartNewGame() {}
	protected virtual void OnLoadGame() {}

	public void SetActive() {

		hasUsedGamePadToNavigate = false;
		hasUsedKeyboardToNavigate = false;

		ResetMenuState();

		this.isActive = true;

	}

	void Update () {

		if(!isActive) {
			return;
		}

		if(menuState != MenuState.MOVING) {

			if(Input.GetButtonDown("menu_down_keyboard")) {
				
				hasUsedGamePadToNavigate = false;
				hasUsedKeyboardToNavigate = true;

				OnMoveToNextButton();
				menuState = MenuState.MOVING;
			
			} else if(Input.GetButtonDown("menu_up_keyboard")) {
			
				hasUsedGamePadToNavigate = false;
				hasUsedKeyboardToNavigate = true;

				OnMoveToPreviousButton();
				menuState = MenuState.MOVING;
			}
			
			if(Input.GetAxis("menu_down_controller") > 0) {
				hasUsedKeyboardToNavigate = false;
				hasUsedGamePadToNavigate = true;

				OnMoveToNextButton();
				menuState = MenuState.MOVING;

			} else if(Input.GetAxis("menu_down_controller") < 0) {

				hasUsedKeyboardToNavigate = false;
				hasUsedGamePadToNavigate = true;

				OnMoveToPreviousButton();
				menuState = MenuState.MOVING;
			}

		}

		if(menuState == MenuState.MOVING) {
			if(hasUsedGamePadToNavigate) {
				if(Mathf.Abs(Input.GetAxis("menu_down_controller")) == 0) {
					ResetMenuState();
				}
			} else if(hasUsedKeyboardToNavigate) {
				if(!Input.GetButtonDown("menu_down_keyboard") && !Input.GetButtonDown("menu_up_keyboard")) {
					ResetMenuState();
				}
			}
		}

		if(Input.GetButtonDown("menu_select") && menuState != MenuState.BUSY) {
			menuState = MenuState.BUSY;

			menuButtons[currentIndex].OnPressed();
			menuState = MenuState.MOVING;
		}
	}

	public void ResetMenuState() {
		menuState = MenuState.IDLE;
	}
}
