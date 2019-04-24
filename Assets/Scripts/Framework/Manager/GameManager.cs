using UnityEngine;
using System.Collections;

public abstract class GameManager : MonoBehaviour {

	public virtual void Start() {
		if(!Loader.IS_USING_LOADER) {	
			OnStart();
		}
	}

	public abstract void OnStart();
}
