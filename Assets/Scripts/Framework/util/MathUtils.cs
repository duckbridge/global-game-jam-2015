using UnityEngine;
using System.Collections;

public class MathUtils {

	public static Vector3 CalculateDirection(Vector3 target, Vector3 origin) {
		Vector3 direction = target - origin;
		direction.Normalize();
		return direction;
	}

	public static Vector2 CalculateDirection2D(Vector3 target, Vector3 origin) {

		Vector3 direction = new Vector3(target.x - origin.x, target.y - origin.y, 0f);
		direction.Normalize();

		return direction;
	}


	public static Direction GetHorizontalDirection(Vector3 target, Vector3 origin) {

		float direction = target.x - origin.x;

		if(direction > 0) {
			return Direction.RIGHT;
		} else {
			return Direction.LEFT;
		}
	}

	public static Direction GetVerticalDirection(Vector3 target, Vector3 origin) {
		
		float direction = target.y - origin.y;
		
		if(direction > 0) {
			return Direction.UP;
		} else {
			return Direction.DOWN;
		}
	}

	public static Vector2 CalculateDirection(Vector2 target, Vector2 origin) {
		Vector2 direction = target - origin;
		direction.Normalize();
		return direction;
	}

	public static bool IsWithinBounds(Bounds checkedBounds, Bounds withinBounds) {
		return checkedBounds.Intersects(withinBounds);
	}
}
