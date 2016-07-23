using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

    private const float MOVEMENT_SPEED = 3;
    private const float TURN_SPEED = 120;
    private const float ANGLE_DELTA = 30;
    private float speed;

    private PlayerDetection thisDetection;

    EnemyMovement()
    {
        //Global.EnemyMovement = this;
    }

    void Awake()
    {
        thisDetection = GetComponentInChildren<PlayerDetection>();
    }

    public void MoveTowardsLocation(Vector3 _targetLocation, bool chasingEnemy)
    {
        speed = chasingEnemy ? MOVEMENT_SPEED : MOVEMENT_SPEED / 2;

        Vector3 direction = (_targetLocation - transform.position);
        float angleFrom = Vector3.Angle(transform.forward, direction);

        Vector3 normalizedDir = direction.normalized;
        Quaternion _lookRotation = Quaternion.LookRotation(normalizedDir);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _lookRotation, Time.deltaTime * TURN_SPEED);

        if (angleFrom < ANGLE_DELTA)
            transform.position = Vector3.MoveTowards(transform.position, _targetLocation, speed * Time.deltaTime);

        if (Vector3.Equals(transform.position, _targetLocation))
            thisDetection.WeHaveReachedTarget();
    }


    // start loop
    // find new angle
    // move until it reaches the angle
    // start again?
    public bool running = false;
    public IEnumerator RotateToNewDir(float timer)
    {
        running = true;
        Debug.Log("Timer " + timer);
        yield return new WaitForSeconds(timer);

        float difference = Random.Range(-40f, 40f);
        difference = difference < 0 ? Mathf.Clamp(difference, -40f, -20f) :
            Mathf.Clamp(difference, 20f, 40f);

        Debug.Log("difference " + difference);

        Vector3 newAngle = transform.eulerAngles;
        newAngle.y += difference;

        Debug.Log("New target angle " + newAngle.y);

        newAngle.y = 
            newAngle.y < 0 ? 360 - Mathf.Abs(newAngle.y) : 
            newAngle.y > 360 ? newAngle.y - 360 :
            newAngle.y;

        Debug.Log("New target angle " + newAngle.y);

        while (!Mathf.Approximately(transform.eulerAngles.y, newAngle.y))
        {
            Vector3 fuckingShhhhhh = transform.eulerAngles;
            fuckingShhhhhh.y = Mathf.MoveTowards(fuckingShhhhhh.y, newAngle.y, TURN_SPEED * Time.deltaTime);
            transform.eulerAngles = fuckingShhhhhh;
            yield return new WaitForEndOfFrame();
        }

        thisDetection.co = RotateToNewDir(Random.Range(4f,8f));
        StartCoroutine(thisDetection.co);
        yield return null;
    }

    //public void RotateToNewDir(Vector3 _newAngle)
    //{
    //    //Vector3 normalizedDir = _newAngle.normalized;
    //    //Quaternion _lookRotation = Quaternion.LookRotation(normalizedDir);
    //    Quaternion _rotationAngle = Quaternion.Euler(_newAngle);
    //    transform.rotation = Quaternion.RotateTowards(transform.rotation, _rotationAngle, Time.deltaTime * TURN_SPEED);

    //    if (transform.eulerAngles.Equals(_newAngle))
    //        this.thisDetection.FinishedTurning();
    //}

}
