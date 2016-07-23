using UnityEngine;
using System.Collections;

public static class GeneralHelpers {

	public static Vector3 GetMovementKeysPressed()
	{
		return (Vector3.right * Input.GetAxis(Global.InputAxis.HORIZONTAL)) + (Vector3.forward * Input.GetAxis(Global.InputAxis.VERTICAL));
	}

	public static Vector3 GetMouseMoved()
	{
		return (Vector3.up * Input.GetAxis (Global.InputAxis.MOUSE_X)) + (Vector3.right * -Input.GetAxis (Global.InputAxis.MOUSE_Y));
	}

	public static bool GetStateSwitchKeyPressed()
	{
		return (
		    Input.GetKeyDown(KeyCode.LeftControl) ||
		    Input.GetKeyDown(KeyCode.RightControl) 
		);
	}

}
