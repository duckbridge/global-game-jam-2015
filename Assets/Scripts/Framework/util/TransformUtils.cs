using UnityEngine;
using System.Collections;

public class TransformUtils : MonoBehaviour {

	public static void CopyXY(Transform source, Transform target, bool copyLocal = false) {
		if(source && target) {
			if(copyLocal) {
				target.localPosition = new Vector3(source.localPosition.x, source.localPosition.y, target.localPosition.z);
			} else {
				target.position = new Vector3(source.position.x, source.position.y, target.position.z);
			}
		}
	}

	public static bool IsInScreen(Camera cameraToUse, Transform target) {
		Vector3 viewPoint = cameraToUse.WorldToViewportPoint(target.position);
		if(viewPoint.x > 0 && viewPoint.x < 1) {
			return true;
		} else {
			return false;
		}
	}
}
