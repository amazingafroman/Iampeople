using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AttachedWaypoints : MonoBehaviour {

	// the purpose of this is to have a list of all the attached waypoints so that we can iterate through them to find a path
    // from the butchers current position and the original waypoint

    //[SerializeField]
    //private List<AttachedWaypoints> privateList = new List<AttachedWaypoints>();
    //public List<AttachedWaypoints> GetAttached()
    //{
    //    return privateList;
    //}

    //public void FindAttachedWayPoints()
    //{
    //    privateList = new List<AttachedWaypoints>();
    //    foreach(Transform waypoint in Global.WaypointList.GetWayPoints())
    //    {
    //        RaycastHit hit;
    //        //Debug.Log("Checking between this + " + waypoint.position);
    //        if (!Physics.Linecast(transform.position, waypoint.position, out hit))
    //            privateList.Add(waypoint.GetComponent<AttachedWaypoints>());
    //        //else
    //        //    Debug.Log("We haven't used this one");
    //    }
    //    Debug.Log("Number of connected waypoints " + privateList.Count);
    //}

    //public bool CheckAgainstPlayer()
    //{
    //    return Physics.Linecast(transform.position, Global.CowInteraction.GetPosition());
    //}
}
