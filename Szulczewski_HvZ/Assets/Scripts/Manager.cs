using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    //create data structures for zombies and humans
    public List<GameObject> zombies;
    public List<GameObject> humans;

    public enum Debug
    {
        On,
        Off
    }

    public Debug lines;

    //starting zombies and humans
    [SerializeField]
    GameObject zom1;
    [SerializeField]
    GameObject zom2;
    [SerializeField]
    GameObject zom3;
    [SerializeField]
    GameObject hum1;
    [SerializeField]
    GameObject hum2;
    [SerializeField]
    GameObject hum3;

    //The zombie and human we will instantiate
    [SerializeField]
    GameObject human;
    [SerializeField]
    GameObject zombie;

    [SerializeField]
    public Material green;
    [SerializeField]
    public Material blue;
    [SerializeField]
    public Material black;

    // Start is called before the first frame update
    void Start()
    {
        //Instantiate these lists
        zombies = new List<GameObject>();
        humans = new List<GameObject>();

        //Add proper objects to the lists
        humans.Add(hum1);
        humans.Add(hum2);
        humans.Add(hum3);
        zombies.Add(zom1);
        zombies.Add(zom2);
        zombies.Add(zom3);

        lines = Debug.On;
    }

    // Update is called once per frame
    void Update()
    {
        //Check for collision between every zombie and every human
        if (humans.Count > 0)
        {
        foreach(GameObject zom in zombies)
        {
                for (int i = 0; i < humans.Count; i++)
                {
                    //Collision detection with human - remove them and change them into a zombie
                    if (Vector3.Distance(zom.transform.position, humans[i].transform.position) <
                        zom.GetComponent<Zombie>().radius + humans[i].GetComponent<Human>().radius)
                    {
                        //Turn this human into a zombiiiiie!
                        //get its position first
                        Vector3 zomPos = humans[i].transform.position;

                        //But the y position must be set to 1 bc it is bigger.
                        zomPos.y = 1f;

                        //Kill the human and make a new zomb
                        humans.Remove(zom.GetComponent<Zombie>().targetHuman);
                        zom.GetComponent<Zombie>().targetHuman.SetActive(false);

                        //make new zom
                        zombies.Add(Instantiate(zombie, zomPos, Quaternion.identity));
                    }
                }
            }
        }
        //Check to toggle debug lines
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
        }
    }
}
