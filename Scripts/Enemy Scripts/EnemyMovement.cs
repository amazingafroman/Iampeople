using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {

    private const float MOVEMENT_SPEED = 4;
    private const float TURN_SPEED = 120;
    private const float ANGLE_DELTA = 30;
    private float speed;

    EnemyMovement()
    {
        //Global.EnemyMovement = this;
    }

    public void MoveTowardsLocation(Vector3 _targetLocation, bool chasingEnemy)
    {
        speed = chasingEnemy ? MOVEMENT_SPEED : MOVEMENT_SPEED / 2;

        Vector3 _direction = (_targetLocation - transform.position).normalized;
        Quaternion _lookRotation = Quaternion.LookRotation(_direction);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _lookRotation, Time.deltaTime * TURN_SPEED);

        //Debug.Log("Rotation difference " + Vector3.Angle(_targetLocation, transform.position));

        if(Vector3.Angle(_targetLocation, transform.position) < ANGLE_DELTA)
            transform.position = Vector3.MoveTowards(transform.position, _targetLocation, MOVEMENT_SPEED * Time.deltaTime);
    }

}
