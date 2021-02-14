using System.Collections;
//using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour{

    [SerializeField] Turtle turtle;

    public float minSpawnTime, maxSpawnTime, turtleSpeed;
    float timePassed, spawnTime;
    bool spawn;
    int waveNumber;
    public int numberOfSpawnedEnemy;

    public bool _menuState, _playState, _waveIntroState;

    void Start()    {
        spawn = true;
        spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
    }


    void Update()    {
        PlayState();
        WaveIntroState();
        MenuState();
    }

    //  ==============================================   //
    //  =================   STATES   =================   //
    //  ==============================================   //

    private void PlayState() {
        if (!_playState) { return; }
        _waveIntroState = _menuState = false;

        timePassed += Time.deltaTime;

        if (spawn && timePassed > spawnTime && numberOfSpawnedEnemy <= waveNumber * 3) {
            Spawn();
            numberOfSpawnedEnemy++;
        } else if (numberOfSpawnedEnemy > waveNumber * 3) {
            _playState = false;
            StartCoroutine(StartNextWave());
        }
    }
    private void WaveIntroState() {
        if (!_waveIntroState) { return; }
        _playState = _menuState = false;

        numberOfSpawnedEnemy = 0;
    }
    private void MenuState() {
        if (!_menuState) { return; }
        _waveIntroState = _playState = false;

        minSpawnTime = 3f; maxSpawnTime = 6f;

        timePassed += Time.deltaTime; 
        if (timePassed > spawnTime) {
            Spawn();
        }
    }


    //  =============   Private Methods   ============   //

    private void Spawn() {
        Turtle newTurtle = Instantiate(turtle, transform.position, transform.rotation) as Turtle;
        newTurtle.transform.parent = transform;
        FindObjectOfType<GameHandler>().ChangeEnemyNumber(1);   // Let GH about spawned enemy

        minSpawnTime = -0.3125f * waveNumber + 3.3125f; maxSpawnTime = (-waveNumber) + 7f;
        spawnTime = Random.Range(minSpawnTime, maxSpawnTime);    // Get new random time
        timePassed = 0; // Reset Timer

        turtleSpeed = (float)((5 * waveNumber) / 10f);
        newTurtle.speed = turtleSpeed;

        if (_menuState) {
            FindObjectOfType<Character>().AddMenuEnemy();
            newTurtle.speed = 1f;
        }
            
    }

    private IEnumerator StartNextWave() {
        yield return new WaitForSeconds(4);

        FindObjectOfType<GameHandler>().StartNextWave();
    }


    //  =============   Public Methods   ============   //

    public void SetSpawn(bool spawn) { this.spawn = spawn; }
    public void SetWaveNumber(int waveNumber) { this.waveNumber = waveNumber; }
}
