using UnityEngine;
using System.Collections;

public abstract class StaticObject {

	public StaticObject()
	{
		StaticUpdater.UpdateStatic += Update;
	}

	/// <summary>
	/// An update function that allows us to use the standard Unity update function 
	/// on our scripts that aren't attached to GameObjects
	/// </summary>
	public abstract void Update ();
}
