using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Loader : DispatchBehaviour {

	public static bool IS_USING_LOADER = false;
	private static bool HAS_USED_LOADER = false;
	public static bool HAS_DONE_FULL_RELOAD = false;

	private enum LoaderState { PreLoading, Loading, Done, None}
	private LoaderState currentLoaderState = LoaderState.None;
	private Scene currentScene;

	private static Scene loadingScene = Scene.NONE;

	public virtual void Start() {
		if(transform.parent) {
			Debug.Log("[LOADER] WARNING! Loader should NOT have a parent object!");
		}
		StartLoading();
	}

	void Update () {
		switch(currentLoaderState) {
			case LoaderState.PreLoading:
				DontDestroyOnLoad(transform.gameObject);
				Application.LoadLevelAdditive(loadingScene.ToString());
				currentLoaderState = LoaderState.Loading;
			break;
			case LoaderState.Loading:
				if(!Application.isLoadingLevel) {
					currentLoaderState = LoaderState.Done;
				}
			break;
			case LoaderState.Done:
				currentScene = loadingScene;
				OnLoadingDone();
				currentLoaderState = LoaderState.None;
			break;
		}
	}

	private void StartLoading() {
		currentLoaderState = LoaderState.PreLoading;
		float fxVolume = PlayerPrefs.GetFloat(GameSettings.FX_SAVE_NAME, GameSettings.DEFAULT_FX_VOLUME);
		SoundUtils.SetSoundVolume(SoundType.FX, fxVolume);
	}

	public static void LoadScene(Scene scene) {

		PauseHelper.ResumeGame();
		Time.timeScale = 1f;

		Loader.IS_USING_LOADER = true;
		Loader.HAS_USED_LOADER = true;
		Loader.HAS_DONE_FULL_RELOAD = true;

		loadingScene = scene;

		Application.LoadLevel(Scene.Empty.ToString());
		Application.LoadLevelAdditive(Scene.Loading.ToString());
	}

	public static void LoadSceneQuickly(Scene scene) {
		
		PauseHelper.ResumeGame();
		Time.timeScale = 1f;

		Loader.IS_USING_LOADER = false;
		Loader.HAS_DONE_FULL_RELOAD = false;

		loadingScene = scene;
		
		Application.LoadLevel (scene.ToString());
	}

	public static void ReloadLevelWithoutLoadingScene() {
		string sceneToLoad = loadingScene.ToString();
		if(!Loader.HAS_USED_LOADER) {
			sceneToLoad = Application.loadedLevelName;
		}

		HAS_DONE_FULL_RELOAD = false;
		Application.LoadLevel (sceneToLoad);
	}

	public static void ReloadLevel() {

		string sceneToLoad = loadingScene.ToString();
		if(!Loader.HAS_USED_LOADER) {
			sceneToLoad = Application.loadedLevelName;
		}

		Scene scene = (Scene) System.Enum.Parse(typeof(Scene), sceneToLoad);
		HAS_DONE_FULL_RELOAD = true;

		Loader.LoadScene(scene);
	}

	public Scene GetCurrentScene() {
		return currentScene;
	}

	public static Scene GetLoadingScene() {
		return loadingScene;
	}

	public virtual void OnLoadingDone() {
		Destroy (this.gameObject);
	}
}
