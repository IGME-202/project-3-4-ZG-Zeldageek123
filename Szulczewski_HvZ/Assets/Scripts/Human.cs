using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Vehicle
{
    public GameObject targetZombie;
    public Treasure targetTreasure;
    public float detectionRange;

    protected override void Start()
    {
        base.Start();
        //Goes faster than zombie
        maxSpeed = 4f;
        detectionRange = 5f;
    }

    protected override void CalcSteeringForces()
    {
        Vector3 uForce = Vector3.zero;

        if (Vector3.Distance(position, targetZombie.transform.position) < detectionRange)
        {
            //apply the force of fleeing from the zombie!
            uForce += Flee(targetZombie);
        }

        //Now seek out that trasure, if danger isn't imminent...
        uForce += Seek(targetTreasure.gameObject);

        //Clamp it to the magnitude of the max force
        uForce = Vector3.ClampMagnitude(uForce, maxForce);

        //Apply the limited force for a reasonable result
        ApplyForce(uForce);
    }

    protected override void Update()
    {
        base.Update();

        if (Vector3.Distance(position, targetTreasure.transform.position) < radius)
        {
            //The human will grab the treasure if they can
            targetTreasure.OnGrab();
        }
        //Always set y = 0.5
        position.y = .5f;
    }
}
