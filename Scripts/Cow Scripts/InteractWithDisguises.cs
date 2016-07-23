using UnityEngine;
using System.Collections;

public class InteractWithDisguises : MonoBehaviour {

	public void OnTriggerEnter(Collider col)
	{
		if (col.tag == (Global.ObjectTags.DISGUISE)) 
		{
			Debug.Log ("Donned Disguise");
			Destroy (col.gameObject);
			Global.CowInteraction.NowHasClothes (true);
		}
	}
}
