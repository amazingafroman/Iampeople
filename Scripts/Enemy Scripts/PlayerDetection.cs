using UnityEngine;
using System.Collections;

public class PlayerDetection : MonoBehaviour {

	private const float FIELD_OF_VIEW = 110;
    private const float SECONDARY_FIELD_OF_VIEW = 170;
    private const float CLOSENESS_DISCOVERY = 10;
    private const float DISTANCE_THAT_WILL_CONFIRM_DISCOVERY = 5;
	private SphereCollider detectionCol;
	private bool isPlayerVisible = false;
    private Vector3 localLastPlayerSighting;

    private EnemyMovement thisEnemy;

	PlayerDetection()
	{
		Global.PlayerDetection = this;
	}

	void Awake()
	{
		detectionCol = GetComponent<SphereCollider> ();
        thisEnemy = transform.root.GetComponent<EnemyMovement>();
	}

	void OnTriggerStay(Collider col)
	{
		if (col.transform.root.tag == Global.ObjectTags.PLAYER) 
		{
			//Debug.Log ("Player is in notice area");
			isPlayerVisible = false;

			Vector3 direction = Global.CowInteraction.GetPosition() - transform.position;
            float distance = Vector3.Distance(Global.CowInteraction.GetPosition(), transform.position);
            float angleFrom = Vector3.Angle(direction, transform.forward);

            //Debug.Log("Distance = " + distance);

            bool willGiveChase =
                distance < DISTANCE_THAT_WILL_CONFIRM_DISCOVERY ? true :
                distance < CLOSENESS_DISCOVERY && angleFrom < SECONDARY_FIELD_OF_VIEW / 2 ? true :
                angleFrom < FIELD_OF_VIEW / 2 ? true :
                false;

			if (willGiveChase)
			{
				RaycastHit hit;
				//Debug.Log ("Player is in vision cone");

				if (Physics.Linecast(transform.position, Global.CowInteraction.GetPosition(), out hit)) 
				{
					if (hit.collider.transform.root.tag == Global.ObjectTags.PLAYER) 
					{
                        Debug.Log("Player will be seen if not standing");
                        if (Global.CowInteraction.CanBeSeen())
                        {
                            Debug.Log("Player has been seen!!!");
                            GeneralEventManager.EnemyHasSeenCow();
                            isPlayerVisible = true;
                            localLastPlayerSighting = Global.CowInteraction.GetPosition();
                        }
					}
				}
			}
		}
	}

	void Update()
	{
		
        // if isPlayerVisible == true then we want to move towards the cow
        // move to the last known location of the cow
        // will continue to move to the last known location of the cow until it gets there

        // stay at that location for a bit, then head back to the waypoint location

        if(isPlayerVisible)
        {
            thisEnemy.MoveTowardsLocation(localLastPlayerSighting, true);
        }
        else
        {
            thisEnemy.MoveTowardsLocation(localLastPlayerSighting, false);
        }

	}

}
