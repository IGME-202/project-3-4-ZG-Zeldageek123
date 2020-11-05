using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Vehicle : MonoBehaviour
{
    protected Vector3 position;
    protected Vector3 direction;
    protected Vector3 velocity;
    protected Vector3 acceleration;

    [Min (0.0001f)]
    public float mass;
    public float radius;
    public float maxSpeed;
    public float maxForce;

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
        WrapVehicle();

        //Update the position in game
        transform.position = position;

        //Get the direction vector normalized (not a crazy magnitude)
        direction = velocity.normalized;

        //Acceleration must start fresh
        acceleration = Vector3.zero;
    }

    protected void ApplyForce(Vector3 force)
    {
        acceleration += force / mass;
    }

    void WrapVehicle()
    {
        Camera main = Camera.main;
        float totalCamHeight = 2f * main.orthographicSize;
        float totalCamWidth = totalCamHeight * main.aspect;

        // Check if not on viewport
        // Note - width / height must be divided by two because origin is centered.
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
}
