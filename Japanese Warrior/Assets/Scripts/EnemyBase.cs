using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour{

    [SerializeField] Turtle turtle;
    [SerializeField] float minSpawnTime, maxSpawnTime;

    float timePassed, spawnTime;
    bool spawn;

    public bool isMenu;

    void Start()    {
        spawn = true;
        //minSpawnTime = 0.3f; maxSpawnTime = 3f;
        spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
    }


    void Update()    {
        timePassed += Time.deltaTime;

        if (spawn && timePassed > spawnTime) {
            Spawn();
        }
    }

    private void Spawn() {
        Turtle newTurtle = Instantiate(turtle, transform.position, transform.rotation) as Turtle;
        newTurtle.transform.parent = transform;
        FindObjectOfType<GameHandler>().ChangeEnemyNumber(1);   // Let GH about spawned enemy
        spawnTime = Random.Range(minSpawnTime, maxSpawnTime);    // Get new random time
        timePassed = 0; // Reset Timer

        if (isMenu)
            FindObjectOfType<Character>().AddMenuEnemy();
    }

    public void SetSpawn(bool spawn) {
        this.spawn = spawn;
    }
}
