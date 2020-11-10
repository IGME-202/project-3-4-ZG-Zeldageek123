using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Vehicle
{
    public Treasure targetTreasure;
    public float detectionRange;

    [SerializeField]
    Manager manager;

    /*
    //Debug lines
    enum Debug
    {
        On,
        Off
    }
    [SerializeField]
    Debug lines;
    */

    protected override void Start()
    {
        base.Start();
        //Goes faster than zombie
        maxSpeed = 4f;
        detectionRange = 10f;

        //At start, debug lines should be on
        //lines = Debug.On;

        manager = GameObject.Find("Manager").GetComponent<Manager>();
    }

    protected override void CalcSteeringForces()
    {
        Vector3 uForce = Vector3.zero;

        //Try to avoid ALL of the zombies.
        for (int i = 0; i < manager.zombies.Count; i++)
        {
            if (Vector3.Distance(position, manager.zombies[i].transform.position) < detectionRange)
            {
                //apply the force of fleeing from the zombie!
                uForce += Flee(manager.zombies[i]);
            }
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

        /*
        //If the user presses d, toggle the deug lines to off
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (lines == Debug.On)
            {
                lines = Debug.Off;
            }
            else
            {
                lines = Debug.On;
            }
        }*/
    }

    void OnRenderObject()
    {
        //Only run this if GUI is set to ON
        if (manager.lines == 0)
        {

            // Set the material to be used for the first line
            //FORWARD VECTOR ------------------------------------------------------
            manager.green.SetPass(0);

            // Draws one line		
            GL.Begin(GL.LINES);                 // Begin to draw lines
            GL.Vertex(transform.position);      // First endpoint of this line

            //forward vector
            Vector3 forward;
            //this is scaled up, but we have no need to use the normalized vector so we don't need a reference to it normalized.
            forward = transform.position + direction * 5;
            GL.Vertex(forward);                 // Second endpoint of this line
            GL.End();                           // Finish drawing the line

            // Second line
            // Set another material to draw this second line in a different color

            //RIGHT VECTOR -----------------------------------------------------------
            manager.blue.SetPass(0);
            GL.Begin(GL.LINES);

            //Get the right vector
            Vector3 rotatedVector = Quaternion.AngleAxis(90, Vector3.up) * direction;
            //this is scaled up, but we have no need to use the normalized vector so we don't need a reference to it normalized.
            Vector3 right = transform.position + rotatedVector * 5;

            GL.Vertex(transform.position);
            GL.Vertex(right);
            GL.End();

            //FUTURE POSITION VECTOR --- not required for project 3. Complete for project 4.
            //purple.SetPass(0);
            //GL.Begin(GL.LINES);
            //GL.Vertex(transform.position);
            //GL.Vertex(transform.position + velocity);
            //GL.End();
        }
    }

}
