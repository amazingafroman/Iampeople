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

}
