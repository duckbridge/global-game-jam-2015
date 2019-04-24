using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class TutorialEvent : DispatchBehaviour {

	public abstract void OnEventStarted();

	public abstract void OnEventFinished();
}
