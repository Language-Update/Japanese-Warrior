using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Spawner : MonoBehaviour{

    [SerializeField] Turtle turtle;

    float timePassed, timeNeeded;
    bool spawn;

    void Start()    {
        spawn = true;
        timeNeeded = Random.Range(0.5f, 3f);
    }


    void Update()    {
        timePassed += Time.deltaTime;

        if (spawn && timePassed > timeNeeded) {
            Turtle newTurtle = Instantiate(turtle, transform.position, transform.rotation) as Turtle;
            newTurtle.transform.parent = transform;

            timeNeeded = Random.Range(0.5f, 3f);    // Get new random time
            timePassed = 0; // Reset Timer
        }
    }
}
