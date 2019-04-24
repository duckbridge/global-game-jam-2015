using UnityEngine;
using System.Collections;

public class DeathBall : DeathBox {

	public float rangeWithinWhichThePlayerDies = 5f;
	private TransformerGameManager transformGameManager;

	void Start () {
		transformGameManager = SceneUtils.FindObject<TransformerGameManager>();	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(transformGameManager) {
			Player playerUsed = transformGameManager.GetCurrentlyControlledPlayer();
			if(playerUsed) {
				if(Vector3.Distance(playerUsed.transform.position , this.transform.position) < rangeWithinWhichThePlayerDies) {
					playerUsed.OnDie();
				}
			}

		}
	}
}
