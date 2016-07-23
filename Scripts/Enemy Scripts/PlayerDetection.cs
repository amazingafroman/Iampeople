using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class PlayerDetection : MonoBehaviour {

    public enum ButcherState
    {
        STATIONAIRY,
        CHASING_COW,
        LOOKING_FOR_COW,
        RETURN_TO_WAYPOINT
    };
    private ButcherState butcherState = ButcherState.STATIONAIRY;
    private EnemyMovement thisEnemy;

	private const float FIELD_OF_VIEW = 110;
    private const float SECONDARY_FIELD_OF_VIEW = 170;
    private const float CLOSENESS_DISCOVERY = 10;
    private const float DISTANCE_THAT_WILL_CONFIRM_DISCOVERY = 5;
    private const float MAX_LOOKING_DELAY = 6;
    private const float DIFFERENCE_TO_STORE_WAYPOINT = 10;
    private Vector3 ORIGINAL_WAYPOINT;

	private SphereCollider detectionCol;
	private bool isPlayerVisible = false;
    private Vector3 localLastPlayerSighting;
    private Vector3 nextPointToGoTo;

    private bool isRandomTurning = false;
    private float turnDelay = 0;

    public IEnumerator co;

    private float detectionSize;
    private float lookingDelay = 0;
    //private bool targetReached = true;

    private List<GameObject> waypointList = new List<GameObject>();


	PlayerDetection()
	{
		//Global.PlayerDetection = this;
	}

	void Awake()
	{
		detectionCol = GetComponent<SphereCollider> ();
        detectionSize = detectionCol.radius;
        thisEnemy = transform.root.GetComponent<EnemyMovement>();
        ORIGINAL_WAYPOINT = thisEnemy.transform.position;
        nextPointToGoTo = ORIGINAL_WAYPOINT;
        co = thisEnemy.RotateToNewDir(4);
    }


    void SetNewWaypointPosition()
    {
        // find the original waypoint position
        // find any waypoint nodes that can be seen by the original position
        // loop through those nodes, and their attached nodes until we find the butcher

        //List<Vector3> waypointPositions = new List<Vector3>();
        //Vector3 butcherPos = Vector3.zero;
        //bool foundButcher = false;
        //while(!foundButcher)
        //{

        //}

    }

    public void FinishedTurning()
    {
        isRandomTurning = false;
        Debug.Log("finished out turning");
    }

    // once the butcher has reached the last known position we have a cool down that makes them wait
    // once the timer has finshed we want to reset the radius of the viewable area
    // we also want to traverse back to where the waypoint is

    public void WeHaveReachedTarget()
    {
        //Debug.Log(string.Format("We have reached target and have {0} waypoints to go to", waypointList.Count));

        SetNewWaypointPosition();

        if (butcherState == ButcherState.CHASING_COW)
            butcherState = ButcherState.RETURN_TO_WAYPOINT; // TODO: make this back to looking for a cow when this is implemented
        else if (butcherState == ButcherState.LOOKING_FOR_COW)
            butcherState = ButcherState.RETURN_TO_WAYPOINT;
        else if (butcherState == ButcherState.RETURN_TO_WAYPOINT && Vector3.Equals(thisEnemy.transform.position, ORIGINAL_WAYPOINT))
        {
            butcherState = ButcherState.STATIONAIRY;
            waypointList.Clear();
        }

        Debug.Log("Butcher state " + butcherState);


        //lookingDelay = MAX_LOOKING_DELAY;
    }

	void OnTriggerStay(Collider col)
	{
        Debug.Log("Object tag " + col.tag);
        if (col.tag == Global.ObjectTags.WAYPOINT)
        {
            GameObject waypointCol = col.gameObject;
            if (!waypointList.Contains(waypointCol))
            {
                waypointList.Add(waypointCol);
                Debug.Log("New waypoint added");
            }
        }
        if (col.transform.root.tag == Global.ObjectTags.PLAYER) 
		{
			isPlayerVisible = false;

			Vector3 direction = Global.CowInteraction.GetPosition() - transform.position;
            float distance = Vector3.Distance(Global.CowInteraction.GetPosition(), transform.position);
            float angleFrom = Vector3.Angle(transform.forward, direction);

            bool willGiveChase =
                distance < DISTANCE_THAT_WILL_CONFIRM_DISCOVERY ? true :
                distance < CLOSENESS_DISCOVERY && angleFrom < SECONDARY_FIELD_OF_VIEW / 2 ? true :
                angleFrom < FIELD_OF_VIEW / 2 ? true :
                false;

            //Debug.Log("Angle is: " + angleFrom);

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

                            if (butcherState == ButcherState.STATIONAIRY)
                            {
                                //waypointList = new List<KeyValuePair<Vector3, float>>();
                                //waypointList.Add(new KeyValuePair<Vector3, float>(transform.position, transform.eulerAngles.y));
                            }

                            butcherState = ButcherState.CHASING_COW;
                            // this makes it slightly easier for the cow to escape the butchers clutches
                            //detectionCol.radius = detectionSize * .7f;
                        }
					}
				}
			}
		}
      
	}

	void Update()
	{

        // will look around or something
        if (lookingDelay > 0)
        {
            lookingDelay -= Time.deltaTime;
        }

        #region OLD CODE

        /*
        // if our current angle is more than x difference from our last angle then we will add another point to our list
        float angle = transform.eulerAngles.y;
        if (waypointList.Count != 0)
        {
            if (Mathf.DeltaAngle(angle, waypointList[waypointList.Count - 1].Value) > DIFFERENCE_TO_STORE_WAYPOINT ||
                Vector3.Distance(waypointList[waypointList.Count - 1].Key, transform.position) > 10)
            {
                Debug.Log("Angle difference means we can save a new waypoint");
                if (Vector3.Distance(waypointList[waypointList.Count - 1].Key, transform.position) > 10)
                {
                    Debug.Log("Adding position to list");
                    waypointList.Add(new KeyValuePair<Vector3, float>(transform.position, angle));
                }
                else
                {
                    Debug.Log("our new way point is too close, we will not use it");
                }
            }
        }
        */
        #endregion

        // if we are in the right state we will turn around after a little while
        // set a delay time
        // once delay runs down generate a new target angle
        // pass the angle to the movement
        // don't enter the loop until the turn has finished

        //if (butcherState != ButcherState.STATIONAIRY)
        //    StopCoroutine(co);
        //this.thisEnemy.running = false;
        if (butcherState == ButcherState.STATIONAIRY)
        {
            //if (!thisEnemy.running)
            //    StartCoroutine(co);
            // do some random rotating stuff here
        }
        else if(butcherState == ButcherState.CHASING_COW)
        {
            Vector3 positionToGoTo = localLastPlayerSighting;
            thisEnemy.MoveTowardsLocation(positionToGoTo, true);
        }
        else if(butcherState == ButcherState.LOOKING_FOR_COW)
        {
            // do some rotating stuff while we are waiting for our cooldown to finish
        }
        else if(butcherState == ButcherState.RETURN_TO_WAYPOINT)
        {
            // walk back through some positions until we get back to our original waypoint
            //Vector3 positionToGoTo = nextPointToGoTo;
            thisEnemy.MoveTowardsLocation(nextPointToGoTo, false);
        }

    }

}
