using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemyBase : MonoBehaviour{

    [SerializeField] Turtle turtle = null;
    [SerializeField] Turtle bigTurtle = null;
    [SerializeField] Turtle armoredTurtle = null;
    [SerializeField] Turtle armoredBigTurtle = null;

    [SerializeField] TextMeshProUGUI waveCounterText = null;
    [SerializeField] TextMeshProUGUI enemyCounterText = null;

    GameHandler gameHandler = null;

    public float minSpawnTime, maxSpawnTime, turtleSpeed;
    float timePassed, spawnTime;
    bool spawn;
    int waveNumber;
    public int numberOfEnemyToSpawn;

    public bool _menuState, _playState, _waveIntroState;

    void Start()    {
        spawn = true;
        spawnTime = Random.Range(minSpawnTime, maxSpawnTime);
        gameHandler = FindObjectOfType<GameHandler>();
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

        if (spawn && timePassed > spawnTime && numberOfEnemyToSpawn > 0) {
            Spawn();
            numberOfEnemyToSpawn--;
            enemyCounterText.SetText("Enemy: " + numberOfEnemyToSpawn);
        } else if (numberOfEnemyToSpawn <= 0) {
            _playState = false;
            StartCoroutine(StartNextWave());
        }
    }
    private void WaveIntroState() {
        if (!_waveIntroState) { return; }
        _playState = _menuState = false;

        waveCounterText.SetText("Wave: " + waveNumber);
        numberOfEnemyToSpawn = waveNumber * 3;
        enemyCounterText.SetText("Enemy: " + numberOfEnemyToSpawn);
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

        // Select turtle variant by chances
        Turtle selectedTurtle;  int randomNumber = Random.Range(0, 100);
        if (waveNumber <= 3) {
            if (randomNumber > 40)
                selectedTurtle = turtle;
            else if (randomNumber > 20 && waveNumber > 1)
                selectedTurtle = bigTurtle;
            else if (randomNumber > 10 && waveNumber > 2)
                selectedTurtle = armoredTurtle;
            else if (randomNumber > 5 && waveNumber > 3)
                selectedTurtle = armoredBigTurtle;
            else
                selectedTurtle = turtle;
        } 
        else if (waveNumber <= 6) {
            if (randomNumber > 60)
                selectedTurtle = turtle;
            else if (randomNumber > 30)
                selectedTurtle = bigTurtle;
            else if (randomNumber > 15)
                selectedTurtle = armoredTurtle;
            else if (randomNumber > 8)
                selectedTurtle = armoredBigTurtle;
            else
                selectedTurtle = turtle;
        }
        else {
            if (randomNumber > 90)
                selectedTurtle = turtle;
            else if (randomNumber > 45)
                selectedTurtle = bigTurtle;
            else if (randomNumber > 22)
                selectedTurtle = armoredTurtle;
            else if (randomNumber > 11)
                selectedTurtle = armoredBigTurtle;
            else
                selectedTurtle = turtle;
        }

        Turtle newTurtle = Instantiate(selectedTurtle, transform.position, transform.rotation) as Turtle;
        newTurtle.transform.parent = transform;
        gameHandler.ChangeEnemyNumber(1);   // Let GH about spawned enemy

        // Reduce minTime by 0.2s and maxTime by 0.6s with every wave but limit min to 0.5s and max to 5s
        minSpawnTime = Mathf.Max(0.5f, 3f - (0.2f * waveNumber)); maxSpawnTime = Mathf.Max(5f, 11f - (0.6f * waveNumber));
        spawnTime = Random.Range(minSpawnTime, maxSpawnTime);    // Get new random time
        timePassed = 0; // Reset Timer

        // Update Turtle speed
        newTurtle.speed = newTurtle.speedReference * waveNumber;

        // Update the turtle position for the big ones
        if (selectedTurtle == bigTurtle || selectedTurtle == armoredBigTurtle) {
            var turtlePos = new Vector2(0, 0.25f);
            newTurtle.transform.localPosition = turtlePos;
        }

        // Let the GameHandler know how many blade needed for this one
        FindObjectOfType<Character>().ChangeBladeNeeded(newTurtle.GetComponent<Health>().GetHealth() / 10);
        //Debug.Log("EnemyBase Sent: " + newTurtle.GetComponent<Health>().GetHealth() / 10);

        if (_menuState) {
            FindObjectOfType<Character>().AddMenuEnemy();
            newTurtle.speed = 1f;
        }
            
    }

    // COM
    private IEnumerator StartNextWave() {
        yield return new WaitForSeconds(4);

        gameHandler.StartNextWave();
    }


    //  =============   Public Methods   ============   //

    public void SetSpawn(bool spawn) { this.spawn = spawn; }
    public void SetWaveNumber(int waveNumber) { this.waveNumber = waveNumber; }
}
