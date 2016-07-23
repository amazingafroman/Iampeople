using UnityEngine;
using System.Collections;

public static class Global {

	public static CowInteraction CowInteraction;
	//public static PlayerDetection PlayerDetection;
	public static FatigueTimer FatigueTimer;
    public static CameraManager CameraManager;
    public static WaypointList WaypointList;
    //public static EnemyMovement EnemyMovement;

	static Global()
	{
		InitializeGlobalVariables ();
	}
	static void InitializeGlobalVariables()
	{
		FatigueTimer = new FatigueTimer();
	}

	public static class ObjectTags
	{
		public const string DISGUISE = "Disguise";
		public const string ENEMY = "Enemy";
		public const string PLAYER = "TotallyAPeople";
        public const string WAYPOINT = "Waypoints";
	}

	public static class InputAxis
	{
		public const string HORIZONTAL = "Horizontal";
		public const string VERTICAL = "Vertical";
		public const string MOUSE_X = "Mouse X";
		public const string MOUSE_Y = "Mouse Y";
	}

}
