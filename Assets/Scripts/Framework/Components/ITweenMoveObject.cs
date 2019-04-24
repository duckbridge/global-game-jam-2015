﻿using UnityEngine;
using System.Collections;

public class ITweenMoveObject : DispatchBehaviour {

	public bool destroyOnMovingDone = false;
	public bool moveInLocalSpace = false;

	public Transform moveTarget;
	public float moveTime;
	private Vector3 originalPosition;
	private Vector3 moveTargetPosition;

	// Use this for initialization
	void Awake () {
		originalPosition = this.transform.position;
	}

	void Start() {
	}

	void Update() {
	}

	public void Reset() {
		iTween.StopByName(this.gameObject, "Move");
		this.transform.position = originalPosition;
	}

	public void DoMove() {
		moveTargetPosition = moveTarget.position;
		iTween.MoveTo(this.gameObject, 
			              new ITweenBuilder().SetName("Move")
			              .SetPosition(moveTargetPosition)
			              .SetOnCompleteTarget(this.gameObject)
			              .SetOnComplete("OnMovingDone")
		             	  .SetLocal(moveInLocalSpace)
			              .SetTime(moveTime)
			              .Build()
		              );
	}

	public void OnMovingDone() {
		DispatchMessage("OnMovingDone", null);
	}

	public void DoMoveToStart() {
		iTween.MoveTo(this.gameObject, 
			              new ITweenBuilder().SetName("Move")
			              .SetPosition(originalPosition)
			              .SetOnCompleteTarget(this.gameObject)
		             	  .SetLocal(moveInLocalSpace)
			              .SetOnComplete("OnMovingDone")
			              .SetTime(moveTime)
			              .Build()
		              );
	}
}
