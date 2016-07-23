using UnityEngine;
using System.Collections;

public class StaticUpdater : MonoBehaviour {


	public delegate void StaticUpdate();
	/// <summary>
	/// Will send an event notification on every standard unity Update().
	/// This allows us to send updates to scripts that are not attached to GameObjects
	/// </summary>
	public static event StaticUpdate UpdateStatic;


	void Update ()
	{
//		Debug.Log ("Update is working");
		UpdateStatic();
	}


}
