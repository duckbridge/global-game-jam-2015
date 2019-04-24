using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelFinisherCutScene : CutSceneComponent {

	public Scene sceneToLoadOnFinish;

	void Start () {
	
	}
	
	void FixedUpdate () {
	}

	public override void OnActivated() {
		SaveUtil.SaveData(SceneUtils.FindObject<TransformerGameManager>().GetLivesSaved());
		SceneUtils.FindObject<SpecialBackgroundMusic>().SaveBackgroundMusicTime();
		Loader.LoadSceneQuickly(sceneToLoadOnFinish);
	}
}
