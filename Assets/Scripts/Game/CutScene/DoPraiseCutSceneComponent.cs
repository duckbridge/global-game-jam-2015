using UnityEngine;
using System.Collections;

public class DoPraiseCutSceneComponent : CutSceneComponent {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void OnActivated () {
		Vector3 praisePositionTarget = Vector3.zero;

		WizardKing wizardKing = SceneUtils.FindObject<WizardKing>();
		WizardKingSolo wizardKingSolo = SceneUtils.FindObject<WizardKingSolo>();

		if(wizardKingSolo) {
			praisePositionTarget = wizardKingSolo.transform.position;
			wizardKingSolo.CannotDie();
		} else if(wizardKing) {
			praisePositionTarget = wizardKing.transform.position;
		}

		foreach(Transformer transformer in SceneUtils.FindObjects<Transformer>()) {
			if(!transformer.IsTranformed()) {

				Direction directionToKing = MathUtils.GetHorizontalDirection(praisePositionTarget, transformer.transform.position);
				transformer.GetComponent<BodyControl>().Turn((float)directionToKing);
				transformer.DoPraise();
			}
		}
	
		DeActivate();
	}
}
