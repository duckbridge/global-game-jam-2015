using UnityEngine;
using System.Collections;

public class EndScreenGameManager : MonoBehaviour {

	public TextMesh totalAmountOfLivesSavedOutput;
	public Scene sceneToStart;

	private bool isActive = true;
	// Use this for initialization
	void Start () {
		totalAmountOfLivesSavedOutput.text = SaveUtil.LoadData()+"";
	}
	
	// Update is called once per frame
	void Update () {
		if(isActive) {
			if(Input.GetButtonDown("menu_select_transformer")) {
				isActive = false;
				PlayerPrefs.DeleteKey(GameSettings.GetFullSaveName());
				Loader.LoadSceneQuickly(sceneToStart);
			}
		}
	}
}
