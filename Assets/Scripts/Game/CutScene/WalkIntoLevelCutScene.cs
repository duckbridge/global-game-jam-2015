using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WalkIntoLevelCutScene : CutSceneComponent {

	public List<Player> players;
	public float moveTime = 3f;
	public float moveSpeed = 2f;

	void Start () {
		
	}
	
	void Update () {
	
	}

	public override void OnActivated () {
		players[0].GetComponent<BodyControl>().AddEventListener(this.gameObject);
		for(int i = 0 ; i < players.Count ; i++) {
			if(players[i].active) {
				RunningAnimation runningAnimation = players[i].GetComponentInChildren<RunningAnimation>();
				if(runningAnimation) {
					runningAnimation.SetCurrentFrame(Random.Range (0, runningAnimation.frames.Length));
				}
				players[i].GetComponent<BodyControl>().MoveAutomaticKinematicForTime(moveTime, moveSpeed);
			}
		}
	}

	public void OnAutomaticMovementDone() {
		players[0].GetComponent<BodyControl>().RemoveEventListener(this.gameObject);
		for(int i = 0 ; i < players.Count ; i++) {
			if(players[i].active) {
				players[i].GetCharacterControl().StandStill();
				if(players[i] is Transformer) {
					players[i].GetComponent<BodyControl>().Turn((float)Direction.LEFT);
				}
			}
		}

		DeActivate();
	}
}
