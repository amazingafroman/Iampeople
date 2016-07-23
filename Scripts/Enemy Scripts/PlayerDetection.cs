using UnityEngine;
using System.Collections;

public class PlayerDetection : MonoBehaviour {

	private const float FIELD_OF_VIEW = 110;
	private SphereCollider detectionCol;
	private bool isPlayerVisible = false;

	PlayerDetection()
	{
		Global.PlayerDetection = this;
	}

	void Awake()
	{
		detectionCol = GetComponent<SphereCollider> ();
	}

	void OnTriggerStay(Collider col)
	{
		if (col.transform.root.tag == Global.ObjectTags.PLAYER) 
		{
			Debug.Log ("Player is in notice area");
			isPlayerVisible = false;

			Vector3 direction = Global.CowInteraction.GetPosition() - transform.position;
			float angleFrom = Vector3.Angle(direction, transform.forward);

			if (angleFrom < FIELD_OF_VIEW / 2)
			{
				RaycastHit hit;
				Debug.Log ("Player is in vision cone");

				if (Physics.Linecast(transform.position, Global.CowInteraction.GetPosition(), out hit)) 
				{
					if (hit.collider.transform.root.tag == Global.ObjectTags.PLAYER) 
					{
						Debug.Log ("Player has been hit!");
                        if (Global.CowInteraction.CanBeSeen())
                        {
                            GeneralEventManager.EnemyHasSeenCow();
                            isPlayerVisible = true;
                        }
					}
				}
			}
		}
	}

	void Update()
	{
		
	}



}
