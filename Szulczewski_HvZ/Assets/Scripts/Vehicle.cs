using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Vehicle : MonoBehaviour
{
    protected Vector3 position;
    protected Vector3 direction;
    public Vector3 velocity;
    public Vector3 acceleration;

    public Vector3 forward;
    public Vector3 right;

    [Min (0.0001f)]
    public float mass;
    public float radius;
    public float maxSpeed;
    public float maxForce;

    public float safeDistance = 4f;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        mass = 1f;
        radius = 1f;
        maxSpeed = 1f;
        maxForce = 1f;

        position = transform.position;
        direction = Vector3.right;
        velocity = Vector3.zero;
        acceleration = Vector3.zero;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //Find the steering forces we need
        CalcSteeringForces();

        //Update the velocity and the acc vectors using delta time
        velocity += acceleration * Time.deltaTime;
        position += velocity * Time.deltaTime;

        //Wrap the vehicle if need be
        BoundVehicle();

        //Update the position in game
        transform.position = position;

        //Get the direction vector normalized (not a crazy magnitude)
        direction = velocity.normalized;
        //change the direction of the gameobj
        transform.rotation = Quaternion.LookRotation(Vector3.down, direction);

        //Acceleration must start fresh
        acceleration = Vector3.zero;

        if (direction != Vector3.zero)
        {
            forward = direction;
            right = Vector3.Cross(forward, Vector3.up);
        }
    }

    protected void ApplyForce(Vector3 force)
    {
        acceleration += force / mass;
    }

    void BoundVehicle()
    {
        Camera main = Camera.main;
        float totalCamHeight = 2f * main.orthographicSize;
        float totalCamWidth = totalCamHeight * main.aspect;

        // Check if not on viewport
        // Note - width / height must be divided by two because origin is centered.
        /*
        if (position.x < ((0)))
        {
            position = new Vector3((98), 0f, position.z);
            velocity.x = -velocity.x;
        }
        if (position.x > (100))
        {
            position = new Vector3((2), 0f, position.z);
            velocity.x = -velocity.x;
        }
        if (position.z < ((0)))
        {
            position = new Vector3(position.x, 0f, 98);
            velocity.z = -velocity.z;
        }
        if (position.z > (100))
        {
            position = new Vector3(position.x, 0f, 2);
            velocity.z = -velocity.z;
        }*/

        //While nearing the edge of the viewport, seek the center again so you never go out of bounds
        if (position.x <= ((4)))
        {
            Seek(new Vector3(0, position.y, 0));
        }
        if (position.x >= (96))
        {
            Seek(new Vector3(0, position.y, 0));
        }
        if (position.z <= ((4)))
        {
            Seek(new Vector3(0, position.y, 0));
        }
        if (position.z >= (96))
        {
            Seek(new Vector3(0, position.y, 0));
        }
    }

    protected abstract void CalcSteeringForces();

    public Vector3 Seek(Vector3 targetPos)
    {
        Vector3 desiredVelocity = targetPos - position;

        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        //Find seeking force by taking the desired minus the current (think of triangles)
        Vector3 seekingforce = desiredVelocity - velocity;

        return seekingforce;
    }

    public Vector3 Seek(GameObject target)
    {
        return Seek(target.transform.position);
    }

    public Vector3 Flee(Vector3 targetPos)
    {
        Vector3 desiredVelocity = position - targetPos;

        desiredVelocity = desiredVelocity.normalized * maxSpeed;

        //Find seeking force by taking the desired minus the current (think of triangles)
        Vector3 fleeingForce = desiredVelocity - velocity;

        return fleeingForce;
    }

    public Vector3 Flee (GameObject target)
    {
        return Flee(target.transform.position);
    }

    
    public Vector3 Avoidobstacle(GameObject toAvoid)
    {
        return AvoidObstacle(toAvoid.transform.position, toAvoid.GetComponent<Obstacle>().radius);
    }

    
    public Vector3 AvoidObstacle(Vector3 targetPosition, float obstacleRadius)
    {
        //Check if obstacle is behind me
        Vector3 meToObstacle = targetPosition - position;
        if(Vector3.Dot(forward, meToObstacle) < 0)
        {
            return Vector3.zero;
        }

        //Check if there's a potential collision (if the obstacle is too far to left or right of vehicle)
        float rightMeToObstacleDot = Vector3.Dot(right, meToObstacle);
        if (Mathf.Abs(rightMeToObstacleDot) > obstacleRadius + radius)
        {
            return Vector3.zero;
        }

        //Check if obstacle is in range
        float distance = meToObstacle.sqrMagnitude - (obstacleRadius * obstacleRadius);
        if (distance > safeDistance * safeDistance)
        {
            return Vector3.zero;
        }

        //weight the steering force based on how close we are
        float weight = 0; //Starting value

        //In the case that you phased inside the ostacle
        if(distance <= 0)
        {
            weight = float.MaxValue;
        }
        else
        {
            weight = (safeDistance * safeDistance) / distance;
        }

        //clamp weight within acceptable range
        weight = Mathf.Min(weight, 100000);

        //Give it an initial value
        Vector3 desiredVelocity = Vector3.zero;

        //if obstacle on left, go right
        if(rightMeToObstacleDot < 0)
        {
            desiredVelocity = right * maxSpeed;
        }
        //If on right, steer left
        else
        {
            desiredVelocity = right * -maxSpeed;
        }

        //Calculate steering force from desired velocity
        Vector3 steeringForce = (desiredVelocity - velocity) * weight;

        //return steering force
        return steeringForce;
    }
}
