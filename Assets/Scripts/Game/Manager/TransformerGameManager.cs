using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TransformerGameManager : GameManager {

	public TextMesh livesOutput;
	public CutSceneManager cutSceneManagerToPlayAtStart;
	public List<Transformer> transformersInGame;
	private List<int> deActivatedTransformerIDs;
	private WizardKing wizardKing;
	private WizardKingSolo wizardKingSolo;

	private Transformer pickedTransformer;
	private CharacterPicker characterPicker;

	private bool wizardKingSoloIsUsed = false;
	private int livesSaved = 0;

	public void Awake() {
		if(GameSettings.ERASE_SAVED_DATA) {
			PlayerPrefs.DeleteAll();
		}
	}

	public override void OnStart ()  {
		wizardKing = GetComponentInChildren<WizardKing>();
		
		transformersInGame.ForEach(transformerInGame => transformerInGame.AddEventListener(this.gameObject));
		wizardKing.AddEventListener(this.gameObject);

		characterPicker = GetComponentInChildren<CharacterPicker>();
		characterPicker.AddEventListener(this.gameObject);

		characterPicker.SetTotalAmountOfTransformers(transformersInGame.Count);

		int extraSavedLives = SaveUtil.LoadData();
		livesOutput.text = transformersInGame.Count + extraSavedLives + "";
		livesSaved = transformersInGame.Count + extraSavedLives;

		if(cutSceneManagerToPlayAtStart) {
			cutSceneManagerToPlayAtStart.AddEventListener(this.gameObject);
			cutSceneManagerToPlayAtStart.StartCutScene(true);
		} else {
			Invoke ("InitiateCharacterPickerDelayed", .6f);
		}
	}

	public void OnWizardSoloDied(WizardKingSolo wizardKingSolo) {
		Destroy (wizardKingSolo.gameObject);
		
		Invoke ("RestartLevel", 2f);
	}

	public void OnWizardThrown(WizardKingSolo wizardKingSolo) {
		this.wizardKingSolo = wizardKingSolo;
		wizardKingSoloIsUsed = true;
		this.wizardKingSolo.AddEventListener(this.gameObject);
	}
	
	public void OnCutSceneManagerDone(CutSceneManager CutSceneManager) {
		InitiateCharacterPickerDelayed();
	}

	private void InitiateCharacterPickerDelayed() {
		characterPicker.Initialize(transformersInGame);
	}

	private void OnTransformerChosen(Transformer transformer) {
		pickedTransformer = transformer;

		Invoke("ActivateTransformerDelayed", .2f);
	}

	public void Update() {
		if(Input.GetButtonDown ("Back")) {
			SceneUtils.FindObject<SpecialBackgroundMusic>().SaveBackgroundMusicTime();
			Loader.LoadSceneQuickly(Scene.Menu);
		}
	}
	private void ActivateTransformerDelayed() {
		pickedTransformer.GetCharacterControl().AddEventListener(wizardKing.gameObject);
		pickedTransformer.OnChosen();
		pickedTransformer.OnActivate();
	}

	public void OnKingSelected() {
		pickedTransformer = null;

		characterPicker.ShowJumpAndThrowCommands();
		if(wizardKing.GetKingCharacterControl().HasHatOn()) {
			wizardKing.GetKingCharacterControl().OnTakeOffHat();
			wizardKing.GetCharacterControl().AddEventListener(this.gameObject);
		} else {
			OnHatTakenOff();
		}
	}

	public void OnHatTakenOff() {
		wizardKing.GetKingCharacterControl().EnableAnimationSwitching();
		wizardKing.GetKingCharacterControl().StandStill();
		wizardKing.OnActivate();
	}

	public void OnTransformed(Transformer transformer) {
		transformersInGame.Remove (transformer);
		if(transformer.IsTranformed()) {
			livesSaved--;
		}
		livesOutput.text = livesSaved + "";

		//deActivatedTransformerIDs.Add(transformer.ID);

		wizardKing.HideShoutCommands();
		characterPicker.HideTransformerButtons();

		if(transformersInGame.Count > 0) {
			characterPicker.Initialize(transformersInGame);
		} else {
			OnKingSelected();
		}
	}

	public void OnWizardKingDied(WizardKing wizardKing) {
		//let all peasants cheer!
		Destroy (wizardKing.gameObject);

		if(!wizardKingSoloIsUsed) {
			Invoke ("RestartLevel", 2f);
		}
	}

	private void RestartLevel() {
		SceneUtils.FindObject<SpecialBackgroundMusic>().SaveBackgroundMusicTime();
		Loader.ReloadLevelWithoutLoadingScene();
	}

	public void OnTransformerDied(Transformer transformer) {
		wizardKing.HideShoutCommands();
		transformersInGame.Remove (transformer);
		transformer.RemoveEventListener(this.gameObject);

		if(!transformer.IsTranformed()) {
			livesSaved--;
		}

		livesOutput.text = livesSaved + "";
		
		//do something else, let king facepalm
		if(transformersInGame.Count > 0) {
			InitiateCharacterPickerDelayed();
		} else {
			OnKingSelected();
		}

		Destroy (transformer.gameObject);
	}

	public List<int> GetDeActivatedTransformerIDs() {
		return deActivatedTransformerIDs;
	}

	public Player GetCurrentlyControlledPlayer() {
		if(wizardKingSolo && wizardKingSoloIsUsed) {
			return wizardKingSolo;
		}

		if(pickedTransformer != null) {
			return pickedTransformer;
		}

		if(wizardKing.IsActivated()) {
			return wizardKing;
		}

		return null;
	}

	public int GetLivesSaved() {
		return livesSaved;
	}
}
