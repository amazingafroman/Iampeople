using UnityEngine;
using System.Collections;

public abstract class CowState : MonoBehaviour {

	public enum StateOfCow
    {
		TWO_LEGS,
        FOUR_LEGS
    };
	public enum MovementState
	{
		RUNNING,
		WALKING
	};

    private StateOfCow cowState = StateOfCow.FOUR_LEGS;
	private MovementState moveState = MovementState.WALKING;

    public StateOfCow GetCowState()
    {
        return cowState;
    }

	public MovementState GetMovementState()
	{
		return moveState;
	}

	public void SetMoveState (bool setRunning){
		moveState = setRunning ? MovementState.RUNNING : MovementState.WALKING;
        if(moveState == MovementState.RUNNING)
            Debug.Log("Cow is running");
    }

    public void SetCowState(StateOfCow newState)
	{
		cowState = newState;
		Debug.Log (string.Format("Cow state set to {0}", cowState.ToString()));
	}
}
