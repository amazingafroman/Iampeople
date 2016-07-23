using UnityEngine;
using System.Collections;

public class CameraManager : MonoBehaviour {

	GameObject cameraDolly;
	GameObject playerCam; 
	Transform targetCow; 
	float camDamp = 5f;
	float rotateDamp = 5f;
	Vector3 offset;


	// Use this for initialization
	void Awake () {
		playerCam =  GameObject.Find("Main Camera");
		offset = Global.CowInteraction.transform.position - transform.position;
	}
	void Start () {
		
	}

	//camera follow
	void CameraFollow (){

		if (Global.CowInteraction.GetIsMooving()) {
			
			float currentAngle = transform.eulerAngles.y;
			float desiredAngle = Global.CowInteraction.transform.eulerAngles.y;
			currentAngle = Mathf.LerpAngle (currentAngle, desiredAngle, rotateDamp * Time.deltaTime);
			float currentXAngle = transform.eulerAngles.x;
			float desiredXAngle = Global.CowInteraction.transform.eulerAngles.y;
				
			Quaternion rotation = Quaternion.Euler (currentXAngle, currentAngle, 0);
	
			playerCam.transform.position = Global.CowInteraction.transform.position - (rotation * offset );
		
		} else{
			playerCam.transform.RotateAround (Global.CowInteraction.transform.position, Vector3.up, GeneralHelpers.GetMouseMoved().y);
			playerCam.transform.RotateAround (Global.CowInteraction.transform.position, Vector3.right, GeneralHelpers.GetMouseMoved ().x);
		} 
		//playerCam.transform.localPosition += new Vector3 (GeneralHelpers.GetMouseMoved().y, 0f, 0f);
		playerCam.transform.LookAt (Global.CowInteraction.transform.position);
	}
	//camera reset

			
	//camera steer


	// Update is called once per frame
	void LateUpdate () {
		CameraFollow ();
	}
}
