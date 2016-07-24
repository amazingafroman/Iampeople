using UnityEngine;
using System.Collections;

public static class GeneralEventManager {

//	GeneralEventManager ()

	public delegate void HasSeenCow();
	/// <summary>
	/// Will activate if the cow has been seen by an enemy butcher
	/// </summary>
	public static event HasSeenCow CowHasBeenSeen;

	public static void EnemyHasSeenCow()
	{
		CowHasBeenSeen();
	}

}
