using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TwoPlayerFollowCamera2D : DispatchBehaviour {
	
	public int maximumZoomInSize = 20;
	public float zoomAmount = 2;
	public float zoomTime = .5f;

	public float zoomOutMaxPercentage = 0.9f;
	public float zoomOutMinPercentage = 0.1f;

	public float zoomInMinPercentage = 0.3f;
	public float zoomInMaxPercentage = 0.7f;

	private Camera usedCamera;

	private List<Player> playersToFollow;
	private Vector3 oldPosition;
	private bool isBusy = false;

	void Start () {
		playersToFollow = SceneUtils.FindObjects<Player>();
		usedCamera = this.GetComponentInChildren<Camera>();
	}
	
	void FixedUpdate () {
		oldPosition = this.transform.position;

		this.transform.position = new Vector3(CalculateCenterXPoint(), this.transform.position.y, this.transform.position.z);

		ZoomOutIfPlayersAreOutsideCameraBounds();

		if((this.transform.position - oldPosition) != Vector3.zero) {
			DispatchMessage("OnCameraMoved", (this.transform.position - oldPosition));
		}
	}

	private void ZoomOutIfPlayersAreOutsideCameraBounds() {
		foreach(Player player in playersToFollow) {
			Vector3 viewPoint = usedCamera.WorldToViewportPoint(player.transform.position);

			if(viewPoint.x > zoomOutMaxPercentage || viewPoint.x < zoomOutMinPercentage) {
				ZoomOut();
			}

			if(viewPoint.x > zoomInMinPercentage && viewPoint.x < zoomInMaxPercentage) {
				ZoomIn();
			}
		}
	}

	private void ZoomIn() {
		if(usedCamera.orthographicSize - zoomAmount >= maximumZoomInSize) {
			DoZoom(usedCamera.orthographicSize - zoomAmount);
		}
	}

	private void ZoomOut() {
		DoZoom(usedCamera.orthographicSize + zoomAmount);
	}

	private void DoZoom(float newSize) {
		if(!isBusy) {
			isBusy = true;
			iTween.ValueTo (this.gameObject, new ITweenBuilder()
                .SetFromAndTo(usedCamera.orthographicSize, newSize)
                .SetTime(zoomTime)
                .SetOnUpdate("OnCameraZoomChanged")
                .SetOnCompleteTarget(this.gameObject)
                .SetOnComplete("OnZoomed")
                .Build()
        	);
		}

		SceneUtils.FindObjects<UIPositioner>().ForEach(uiPositioner => uiPositioner.Start ());

	}

	private void OnZoomed() {
		isBusy = false;
	}

	public void OnCameraZoomChanged(float newZoomValue) {
		usedCamera.orthographicSize = newZoomValue;
	}

	private float CalculateCenterXPoint() {
		float totalOfXPositions = 0f;

		for(int i = 0; i < playersToFollow.Count ;i++) {
			totalOfXPositions += playersToFollow[i].transform.position.x;
		}

		return (totalOfXPositions / playersToFollow.Count);
	}
}
