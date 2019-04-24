using UnityEngine;
using System.Collections;

public class MenuGameManager : MonoBehaviour {

	public Scene sceneToStart;

	private bool isActive = true;
	// Use this for initialization
	void Start () {
		PlayerPrefs.DeleteKey(GameSettings.GetFullSaveName());
	}
	
	// Update is called once per frame
	void Update () {
		if(isActive) {
			if(Input.GetButtonDown("menu_select_transformer")) {
				isActive = false;
				SceneUtils.FindObject<SpecialBackgroundMusic>().SaveBackgroundMusicTime();
				Loader.LoadSceneQuickly(sceneToStart);
			}
		}
	}
}
