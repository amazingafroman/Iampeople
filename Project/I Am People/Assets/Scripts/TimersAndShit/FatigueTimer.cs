using UnityEngine;
using System.Collections;

public class FatigueTimer : StaticObject {

	private const float MAX_FATIGUE = 10;
	private const float DIFFERENCE = 0.7f;
	private const float DELAY = 1;

	private float currentDelay = 0;
	private float standingFatigue = MAX_FATIGUE;
	private bool cowIsSuperFatigued = false;

	public FatigueTimer()
	{
		Global.FatigueTimer = this;
	}

	public bool IsFatigued()
	{
		return cowIsSuperFatigued;
	}
	/// <summary>
	/// This increases the fatigue of the cow. When the cows fatigue hits 0 the cow will default to FOUR_LEG state
	/// </summary>
	public void IncreaseFatigue(float decrease)
	{
		if (!IsFatigued ())
		{
			standingFatigue -= Time.deltaTime * decrease;
			currentDelay = DELAY;
		}
	}
	/// <summary>
	/// This dereases the fatigue of the cow. This is a good thing.. we want our cow to be happy..
	/// because it is people
	/// </summary>
	public void DecreaseFatigue()
	{
		if(currentDelay <= 0)
			standingFatigue += Time.deltaTime * DIFFERENCE;
	}
	/// <summary>
	/// Clamps all appropriate values between 0 and their MAX (or MIN)
	/// </summary>
	private void ClampAllValues()
	{
		currentDelay = Mathf.Clamp(currentDelay, 0, DELAY);
		standingFatigue = Mathf.Clamp(standingFatigue, 0, MAX_FATIGUE);
	}
	/// <summary>
	/// Sets the fatigued bool of the cow if standing fatigue hits 0
	/// </summary>
	private void SetSuperFatigue()
	{
		if (standingFatigue <= 0 && !IsFatigued())
		{
			cowIsSuperFatigued = true;
			currentDelay = 0;
			Global.CowInteraction.SetCowState(CowState.StateOfCow.FOUR_LEGS);
		}
		if (standingFatigue == MAX_FATIGUE)
			cowIsSuperFatigued = false;
	}

	public override void Update()
	{
		currentDelay -= Time.deltaTime;
		ClampAllValues();

		SetSuperFatigue();

		DecreaseFatigue();
	}


	// set bool == true when fatigue timer hits zero
	// make cow sit the fuck down
	// allow cool down
	// reset bool == false

}
