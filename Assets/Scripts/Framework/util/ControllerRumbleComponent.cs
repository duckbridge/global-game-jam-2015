﻿using UnityEngine;
using System.Collections;

public class ControllerRumbleComponent : MonoBehaviour {

	public void Update() {
	}

	public void Start() {
	}

	public static bool IsXboxControllerPluggedIn() {
		return Input.GetJoystickNames().Length > 0;
	}
}
