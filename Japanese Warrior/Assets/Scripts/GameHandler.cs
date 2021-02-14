using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameHandler : MonoBehaviour{

    [SerializeField] TextMeshProUGUI waveText = null;

    UI_Handler uiHandler;
    EnemyBase enemyBase;

    Content[] contentPool = new Content[20];
    string[] japaneseContent = new string[] {"あ", "い", "う", "え", "お", "か", "き",
    "く", "け", "こ", "が", "ぎ", "ぐ", "げ", "ご", "さ", "し", "す", "せ", "そ"};
    string[] englishContent = new string[] {"a", "i", "u", "e", "o", "ka", "ki",
    "ku", "ke", "ko", "ga", "gi", "gu", "ge", "go", "sa", "shi", "su", "se", "so"};

    int energyValue, enemyNumber, waveNumber;
    float timePassed;
    bool isPlayerDead, waveUp;

    public bool _playState, _waveIntroState;

    void Start() {
        InitilizeParameters();
        CreateContents();
    }


    void Update()    {
        PlayState();
        WaveIntroState();
    }

    //  ==============================================   //
    //  =================   STATES   =================   //
    //  ==============================================   //

    private void PlayState() {
        if (!_playState) { return; }    // if not in Play State, then return

        FireControl();
        GameStatusControl();
    }

    private void WaveIntroState() {
        if (!_waveIntroState) { return; }    // if not in Wave Intro, then return

        waveText.SetText("Wave " + waveNumber);   // Write wave
        uiHandler.SetLoadQuestion(false);
        uiHandler.interactableButtons = false;

        // Get it in screen
        if (!waveUp && waveText.transform.localPosition.y > 500) {
            timePassed = 0; // Reset up conditions
            waveText.transform.Translate(Vector2.down * 200f * Time.deltaTime);
        } 
        else if (!waveUp && waveText.transform.localPosition.y <= 500) { // Once we arrive the pos, wait in there
            timePassed++;
            if (timePassed > 150) {
                waveUp = true;
            }
        }

        // Get it out screen
        if (waveUp && waveText.transform.localPosition.y < 800) {   // if time is up
            waveText.transform.Translate(Vector2.up * 200f * Time.deltaTime);
        }
        else if (waveUp && waveText.transform.localPosition.y >= 800) {   // if we arrive there, then finish intro
            uiHandler.SetLoadQuestion(true);    // Start loading Questions
            uiHandler.interactableButtons = true;
            enemyBase.SetSpawn(true);           // Let enemy base spawn for now, will be inform about wave later on
            enemyBase.SetWaveNumber(waveNumber);    enemyBase._playState = true;
            _waveIntroState = false; _playState = true; // Change State
            waveUp = false;
        }

    }

    //  ==============================================   //
    //  =============   Private Methods   ============   //
    //  ==============================================   //

    // Resets all game values
    private void InitilizeParameters() {
        energyValue = enemyNumber = 0;
        waveNumber = 1;  
        isPlayerDead = waveUp = false;     

        _playState = false; _waveIntroState = true;     // Initialize states

        uiHandler = FindObjectOfType<UI_Handler>();
        uiHandler.SetLoadQuestion(false);

        enemyBase = FindObjectOfType<EnemyBase>();
        enemyBase.SetSpawn(false);
    }

    // Update Character about firing conditions
    private void FireControl() {
        // Let player know the situation so that it can fire
        FindObjectOfType<Character>().SetEnergyValue(energyValue); // Let it know the energy Value
        FindObjectOfType<Character>().SetEnemyNumber(enemyNumber); // Let it know enemy number
    }

    
    
    // Creates the content
    private void CreateContents() {
        // 20 Hiragana Creation. I'll add others later on
        for (int i = 0; i < contentPool.Length; i++) {
            contentPool[i] = new Content(i, "hiragana", japaneseContent[i], englishContent[i]);
        }
    }

    //Control Game Status
    private void GameStatusControl() {
        // Check is player dead or not
        if (isPlayerDead) {
            // if players is dead, then go back to menu for now
            StartCoroutine(LoadAfterGameScene());   // But after 3 seconds
        }
    }

    // If player is dead, load the Menu
    private IEnumerator LoadAfterGameScene() {
        yield return new WaitForSeconds(3);
        LoadSceneByName("Menu");
    }


    //  ==============================================   //
    //  =============   Public Methods   =============   //
    //  ==============================================   //

    #region Button Functions
    public void LoadSceneByName(string SceneName) {
        SceneManager.LoadScene(SceneName);
    }
    #endregion

    public void StartNextWave() {
        waveNumber++;

        enemyBase._waveIntroState = true;
        _playState = false; _waveIntroState = true;
    }


    //  ****                            ****    //
    //  ****    Getters and Setters     ****    //
    //  ****                            ****    //

    public void SetEnergyValue(int energyValue) {
        this.energyValue = energyValue;
        // Update UI
        FindObjectOfType<UI_Handler>().SetEnergyValue(energyValue); 
    }

    // Changes number of the enemy
    public void ChangeEnemyNumber(int change) {  enemyNumber += change;  }

    // Sets players 
    public void SetCharacterStatus(bool isPlayerDead) { this.isPlayerDead = isPlayerDead;  }  
    
    public int GetEnergyValue() { return energyValue; }

    public Content[] GetQuestionContent() { return contentPool; }




    //  ==============================================   //
    //  =================   Buttons  =================   //
    //  ==============================================  //
    
}
