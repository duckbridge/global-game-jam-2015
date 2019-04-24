using UnityEngine;
using System.Collections;

public class PhysicsUtils {

	public static void IgnoreCollisionBetween(GameObject gameObjectA, GameObject gameObjectB) {

		if(gameObjectA && gameObjectB) {
			IgnoreCollisionBetween(gameObjectA.collider, gameObjectB.collider);
		}
	}

	public static void IgnoreCollisionBetween(Collider colliderA, Collider colliderB) {

		if(colliderA && colliderA.enabled && colliderA.active
		   && colliderB && colliderB.enabled && colliderB.active
		   && colliderA != colliderB) {

			Physics.IgnoreCollision(colliderA, colliderB);

		}
	}

	public static void RestoreCollisionBetween(GameObject gameObjectA, GameObject gameObjectB) {

		if(gameObjectA && gameObjectB) {
			RestoreCollisionBetween(gameObjectA.collider, gameObjectB.collider);
		}
	}
	
	public static void RestoreCollisionBetween(Collider colliderA, Collider colliderB) {

		if(colliderA && colliderA.enabled && colliderA.active
		   && colliderB && colliderB.enabled && colliderB.active) {
			
			Physics.IgnoreCollision(colliderA, colliderB, false);
			
		}
	}

	public static void IgnoreOrRestoreCollisionBetween(Collider colliderA, Collider colliderB, bool ignore) {

		if(colliderA && colliderA.enabled
		   && colliderB && colliderB.enabled) {
			
			Physics.IgnoreCollision(colliderA, colliderB, ignore);
			
		}
	}
}
