using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Vehicle
{
    public GameObject targetHuman;

    protected override void Start()
    {
        base.Start();
        //Heavier than the human
        mass = 1.5f;
        maxSpeed = 3.5f;
    }

    protected override void CalcSteeringForces()
    {
        //apply the force of seeking toward the human!
        ApplyForce(Seek(targetHuman));
    }

    protected override void Update()
    {
        base.Update();

        //always set y = a little more than 1
        //Value will always be around 1, with negligible variation
        position.y = 1.0003f;
    }
}
