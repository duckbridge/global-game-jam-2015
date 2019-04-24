using UnityEngine;
using System.Collections;

public class SimpleTutorialBox : MonoBehaviour {

	public SimpleTutorialComponent tutorialObjectToShow;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnTriggerEnter(Collider coll) {
		Player player = coll.gameObject.GetComponent<Player>();
		if(player) {
			tutorialObjectToShow.Show ();
		}
	}

	public void OnTriggerExit(Collider coll) {
		Player player = coll.gameObject.GetComponent<Player>();
		if(player) {
			tutorialObjectToShow.Hide ();
		}
	}
}
