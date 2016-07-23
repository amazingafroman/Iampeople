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
    //private Camera cowCam;

    private Transform m_Cam;                  // A reference to the main camera in the scenes transform
    private Vector3 m_CamForward;             // The current forward direction of the camera
    private Vector3 m_Move;
    float m_TurnAmount;
  
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
        m_Cam = Camera.main.transform;
    }

    public void Update()
    {
        Move(GeneralHelpers.GetMovementKeysPressed());
        CalculateRotate();
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
    /// This is just here so that we get some nice turning going on with the camera script
    /// </summary>
    private void CalculateRotate()
    {
        // TODO: see if we can still allow the player to run backwards

        float h = GeneralHelpers.GetRawHorizontal();
        float v = GeneralHelpers.GetRawVertical();

        if (m_Cam != null)
        {
            // calculate camera relative direction to move:
            m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
            m_Move = v * m_CamForward + h * m_Cam.right;
        }
        else
        {
            // we use world-relative directions in the case of no main camera
            m_Move = v * Vector3.forward + h * Vector3.right;
        }

        if (m_Move.magnitude > 1f)
            m_Move.Normalize();
        Vector3 move = transform.InverseTransformDirection(m_Move);
        //move = Vector3.ProjectOnPlane(move, Vector3.up);
        m_TurnAmount = Mathf.Atan2(move.x, move.z);
     
        float turnSpeed = 360;
        transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
        //Move(m_Move);
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

        isMooving = movementSpeed != Vector3.zero;


        if(movementSpeed.z == 0)
        {
            movementSpeed.z = movementSpeed.x;
            movementSpeed.x = 0;
        }
        movementSpeed.z = Mathf.Abs(movementSpeed.z);

        //movementSpeed.x = Mathf.Abs(movementSpeed.x);

        transform.Translate(movementSpeed * Time.deltaTime);
    }

}
