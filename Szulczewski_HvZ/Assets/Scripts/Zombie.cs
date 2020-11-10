using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : Vehicle
{
    /*
    public GameObject targetHuman1;
    public GameObject targetHuman2;
    public GameObject targetHuman3;*/
    public GameObject targetHuman;

    public Manager manager;

    //Debug lines
    /*
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
        //Heavier than the human originally, but less heavy for debugging / presentation purposes
        mass = .9f;
        maxSpeed = 4.2f;

        //At start, debug lines should be on
        //lines = Debug.On;

        manager = GameObject.Find("Manager").GetComponent<Manager>();
        targetHuman = manager.humans[0];

        /*
        humans.Add(targetHuman1);
        humans.Add(targetHuman2);
        humans.Add(targetHuman3);
        */
    }

    protected override void CalcSteeringForces()
    {
        //apply the force of seeking toward the human!

        //But only if the target human is ACTIVE
        if (targetHuman.activeSelf == true)
        {
            ApplyForce(Seek(targetHuman));
        }
        else
        {
            //Stop the zombie if its target is not active
            velocity = Vector3.zero;
            acceleration = Vector3.zero;
        }
    }

    protected override void Update()
    {
        base.Update();

        if (manager.humans.Count != 0)
        {
            //CLOSEST IS THE TARGET
            //determine the target -----------------------------------------------------------------------------------
            for (int i = 0; i < manager.humans.Count; i++)
            {
                if (Vector3.Distance(position, targetHuman.transform.position) > //distance btwn zombie and current target
                    Vector3.Distance(position, manager.humans[i].transform.position))   //Distance btwn this and the human in list
                {
                    //change the target to this one
                    targetHuman = manager.humans[i];
                }
            }

            bool empty = true;
            //ACCOUNT FOR NULL TARGET
            foreach (GameObject human in manager.humans)
            {
                if (targetHuman == human)
                {
                    empty = false;
                    break;
                }
            }
            if (empty)
            {
                targetHuman = manager.humans[0];
            }

            //always set y = a little more than 1
            //Value will always be around 1, with negligible variation
            position.y = 1.0003f;
        }

        /*
        //If the user presses d, toggle the debug lines
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

        /*
        //Collision detection with human - remove them and change them into a zombie
        if (Vector3.Distance(position, targetHuman.transform.position) < radius * 2)
        {
            //Turn this human into a zombiiiiie!
            //get its position first
            Vector3 zomPos = targetHuman.transform.position;

            //But the y position must be set to 1 bc it is bigger.
            zomPos.y = 1f;

            //Kill the human and make a new zomb
            humans.Remove(targetHuman);
            Destroy(targetHuman);
            //set this equal to the first index - full until the list is empty
            //the "real" target human is updated anyways
            targetHuman = humans[0];

            //make new zom
            Instantiate(this.gameObject, zomPos, Quaternion.identity);
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

            //VECTOR TO TARGET --------------------------------------------------------
            
            if (manager.humans.Count > 0)
            {
                manager.black.SetPass(0);
                GL.Begin(GL.LINES);

                GL.Vertex(transform.position);
                GL.Vertex(targetHuman.transform.position);
                GL.End();
            }

            //FUTURE POSITION VECTOR --- not required for project 3. Complete for project 4.
            //red.SetPass(0);
            //GL.Begin(GL.LINES);
            //GL.Vertex(transform.position);
            //GL.Vertex(transform.position + velocity);
            //GL.End();
        }
    }
}
