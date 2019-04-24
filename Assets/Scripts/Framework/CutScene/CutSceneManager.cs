using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CutSceneManager : DispatchBehaviour {

	public List<CutScene> cutScenes;

	private List<UIElement> uiElements;
	private int currentCutSceneIndex = 0;
	private TriggerListener cutSceneEnableTriggerListener;
	private Player playerUsed;
	private bool isInitialized = false;

	void Awake() {
		Initialize();
	}
	
	public void Update() {}

	public void OnListenerTrigger(Collider coll) {
		WizardKing wizardKing = coll.gameObject.GetComponent<WizardKing>();
		WizardKingSolo wizardKingSolo = coll.gameObject.GetComponent<WizardKingSolo>();
		BulkyGuy bulkyGuy = coll.gameObject.GetComponent<BulkyGuy>();

		if(wizardKingSolo) {
			StartCutScene(null, false);
		}

		if(bulkyGuy && bulkyGuy.GetComponent<BulkyGuyCharacterControl>().IsCarryingKing()){
			bulkyGuy.GetComponent<PlayerInputComponent>().enabled = false;
			StartCutScene(bulkyGuy, true);
			bulkyGuy.GetComponent<CharacterControl>().StandStill();
		}

		if(wizardKing) {
			StartCutScene(wizardKing, true);
			wizardKing.GetComponent<PlayerInputComponent>().enabled = false;
			wizardKing.GetComponent<CharacterControl>().StandStill();
		}
	}

	public void StartCutScene(bool disablePlayerInput = true) {
		StartCutScene(SceneUtils.FindObject<Player>(), disablePlayerInput);
	}

	public void StartCutScene(Player player, bool disablePlayerInput = true) {

		Initialize();

		this.playerUsed = player;

		uiElements.ForEach(uiElement => uiElement.Hide ());

		OnActivateCutScene();
		
		if(cutSceneEnableTriggerListener) {
			cutSceneEnableTriggerListener.RemoveEventListener(this.gameObject);
			cutSceneEnableTriggerListener.collider.enabled = false;
		}
	}

	public void OnActivateCutScene() {
		if(currentCutSceneIndex > 0) {
			cutScenes[currentCutSceneIndex - 1].OnDeActivate();
		}

		cutScenes[currentCutSceneIndex].OnActivate();
	}
	
	public void OnCutSceneDone(CutScene cutScene) {
		currentCutSceneIndex++;

		if(cutScenes.Count == currentCutSceneIndex) {

			DispatchMessage("OnCutSceneManagerDone", this);

			uiElements.ForEach(uiElement => uiElement.Show ());

			this.active = false;

		} else {
			OnActivateCutScene();
		}
	}

	private void Initialize() {
		if(!isInitialized) {
			isInitialized = true;
			cutScenes.ForEach(cutScene => cutScene.AddEventListener(this.gameObject));
			
			if(this.transform.Find ("CutSceneEnableCollider")) {
				cutSceneEnableTriggerListener = this.transform.Find ("CutSceneEnableCollider").GetComponent<TriggerListener>();
				cutSceneEnableTriggerListener.AddEventListener(this.gameObject);
			}
			
			uiElements = new List<UIElement>(SceneUtils.FindObjectsOfType<UIElement>());
		}
	}
}
