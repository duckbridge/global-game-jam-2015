using UnityEngine;
using System.Collections;

public class IntroGameManager : MonoBehaviour {

	public float loadNextSceneTimeout = 2f;
	public Scene sceneToLoad;

	// Use this for initialization
	void Start () {
		Invoke ("LoadNextScene", loadNextSceneTimeout);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void LoadNextScene() {
		Loader.LoadSceneQuickly(sceneToLoad);
	}
}
