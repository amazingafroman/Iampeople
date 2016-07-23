using UnityEngine;
using System.Collections;

public class CowInteraction : CowState {

	private const float TWO_LEGS_SPEED = 3;
	private const float FOUR_LEGS_SPEED = 5;
	private const float RUNNING_SPEED = 7;

	[SerializeField]
	private GameObject twoLegs;
	[SerializeField]
	private GameObject fourLegs;
	private Camera cowCam;

	private GameObject TWO_LEGS_MODEL;

	private bool hasClothes = false;
	private bool isMooving = false;
	/// <summary>
	/// Allows us to set whether the cow is wearing clothes or not
	/// </summary>
	public void NowHasClothes (bool hasClothes)
	{
		this.hasClothes = hasClothes;
	}

	CowInteraction()
    {
		Global.CowInteraction = this;
		GeneralEventManager.CowHasBeenSeen += HasBeenSeen;
    }

	private void HasBeenSeen()
	{
		if (hasClothes) 
		{
			hasClothes = false;
			SetCowState (StateOfCow.FOUR_LEGS);
			SetCowModel ();
			Debug.Log ("Cow has been seen! Losing clothes!");
		}
	}

	public void Start()
	{
		SetCowState(StateOfCow.FOUR_LEGS);
		SetCowModel();
		cowCam = transform.GetComponentInChildren<Camera>();
	}

    public void Update()
    {
		Move(GeneralHelpers.GetMovementKeysPressed());
		Rotate (GeneralHelpers.GetMouseMoved ());
        SetMoveState(GeneralHelpers.GetMovementSwtichKeyPressed());
		ToggleState(GeneralHelpers.GetStateSwitchKeyPressed());
		IncreaseFatigue();
		SetCowModel();
    }

	public bool GetIsMooving() {
		return isMooving;
	}
	


	public Vector3 GetPosition()
	{
		return transform.position;
	}

	/// <summary>
	/// Sets our cows model to either twoLegs or fourLegs based on the state of the cow
	/// </summary>
	private void SetCowModel()
	{
		fourLegs.SetActive(GetCowState() == StateOfCow.FOUR_LEGS);
		twoLegs.SetActive(GetCowState() == StateOfCow.TWO_LEGS && CheckIfAllowedToStand());
	}

	/// <summary>
	/// Starts to increase the fatigue of our cow if it is standing on TWO_LEGS
	/// </summary>
	private void IncreaseFatigue()
	{
		if (GetCowState () == StateOfCow.TWO_LEGS) {
			Global.FatigueTimer.IncreaseFatigue (1f);
		} else if (GetMovementState () == MovementState.RUNNING) {
			Global.FatigueTimer.IncreaseFatigue (1.3f);
		}
			
	}

	/// <summary>
	/// Toggles the state of our cow between FOUR_LEGS and TWO_LEGS
	/// </summary>
	private void ToggleState(bool _doStateSwitch)
	{
		if (!_doStateSwitch || !CheckIfAllowedToStand())
			return;

		StateOfCow newState = 
			GetCowState () == StateOfCow.FOUR_LEGS ? StateOfCow.TWO_LEGS :
			StateOfCow.FOUR_LEGS;

		SetCowState(newState);
		SetCowModel();
	}

    public bool CanBeSeen()
    {
        if (GetCowState() == CowState.StateOfCow.FOUR_LEGS)
            return true;
        else if (!hasClothes)
            return true;

        return false;
    }

	/// <summary>
	/// Checks if our cow is allowed to stand.
	/// </summary>
	private bool CheckIfAllowedToStand()
	{
		return
			!Global.FatigueTimer.IsFatigued () &&
			hasClothes;
	}

	/// <summary>
	/// Rotates the cow along the Y axis and tilts our camera along the X axis
	/// </summary>
	private void Rotate(Vector3 rotation)
	{
		if (isMooving) {
			Vector3 rotateCow = Vector3.up * rotation.y;
			transform.Rotate (rotateCow);
		}
		//Vector3 tiltCamera = Vector3.right * rotation.x;
		//cowCam.transform.Rotate (tiltCamera);
		//ClampCameraTilt ();
	}

	/// <summary>
	/// Clamps the camera tilt between a MAX and a MIN value
	/// </summary>
	private void ClampCameraTilt()
	{
		Vector3 clampedRotation = cowCam.transform.localEulerAngles;

		float xVal = clampedRotation.x;

		if (xVal < 180)
			xVal = Mathf.Clamp (xVal, 0, 26);
		else if (xVal > 180)
			xVal = Mathf.Clamp (xVal, 350, 360);

		clampedRotation.x = xVal;
		cowCam.transform.localEulerAngles = clampedRotation;
	}

	/// <summary>
	/// Moves out cow based on our input
	/// </summary>
    private void Move(Vector3 _direction)
    {
        Vector3 movementSpeed =
			GetCowState() == StateOfCow.FOUR_LEGS ? _direction * FOUR_LEGS_SPEED :
			GetCowState() == StateOfCow.TWO_LEGS ? _direction * TWO_LEGS_SPEED :
			Vector3.zero; // shouldn't happen...

        movementSpeed = 
            GetMovementState() == MovementState.RUNNING ? _direction * RUNNING_SPEED :
            movementSpeed;

        //Debug.Log(string.Format("Movement speed {0}", movementSpeed));

		transform.Translate(movementSpeed * Time.deltaTime);
    }

}
