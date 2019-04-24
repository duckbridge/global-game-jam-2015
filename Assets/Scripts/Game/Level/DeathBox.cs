using UnityEngine;
using System.Collections;

public class DeathBox : MonoBehaviour {

	public bool killsTransformedTransformers = true;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void OnCollisionEnter(Collision coll) {
		WizardKing wizardKing = coll.gameObject.GetComponent<WizardKing>();
		Transformer transformer = coll.gameObject.GetComponent<Transformer>();
		WizardKingSolo wizardKingSolo = coll.gameObject.GetComponent<WizardKingSolo>();

		if(wizardKingSolo) {
			wizardKingSolo.OnDie();
		}

		if(wizardKing) {
			wizardKing.OnDie();
		}

		if(transformer) {
			if(!killsTransformedTransformers) {
				if(!transformer.IsTranformed()) {
					transformer.OnDie();
				}
			} else {
				transformer.OnDie();
			}
		}
	}
}
