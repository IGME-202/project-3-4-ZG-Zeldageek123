using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour
{
    public void OnGrab()
    {
        Camera main = Camera.main;
        float totalCamHeight = 2f * main.orthographicSize;
        float totalCamWidth = totalCamHeight * main.aspect;

        transform.position = new Vector3(Random.Range(5, 95), //x
            .5f, //y
            Random.Range(5, 95)); //z
    }
}
