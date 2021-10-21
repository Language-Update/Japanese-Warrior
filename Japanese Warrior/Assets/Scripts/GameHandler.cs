using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameHandler : MonoBehaviour{

    [SerializeField] TextMeshProUGUI waveText = null;
    [SerializeField] GameObject sectionsCanvas = null;

    UI_Handler uiHandler;
    EnemyBase enemyBase;
    FirebaseManager FBmanager;

    #region Content Part
    Content[] contentPool;

    // Hiragana
    string[] hiragana_JP = new string[] {"あ", "い", "う", "え", "お", "か", "き",
    "く", "け", "こ", "が", "ぎ", "ぐ", "げ", "ご", "さ", "し", "す", "せ", "そ"};

    string[] hiragana_EN = new string[] {"a", "i", "u", "e", "o", "ka", "ki",
    "ku", "ke", "ko", "ga", "gi", "gu", "ge", "go", "sa", "shi", "su", "se", "so"};

    // Katakana
    string[] katakana_JP = new string[] {"ア", "イ", "ウ", "エ", "オ", "カ", "キ",
    "ク", "ケ", "コ", "サ", "シ", "ス", "セ", "ソ", "タ", "チ", "ツ", "テ", "ト"};

    string[] katakana_EN = new string[] {"a", "i", "u", "e", "o", "ka", "ki",
    "ku", "ke", "ko", "sa", "shi", "su", "se", "so", "ta", "chi", "tsu", "te", "to"};

    // first20
    string[] first20_JP = new string[] {"おはようございます", "こんにちは", "こんばんは",

        "おやすみなさい", "ありがとうございます", "わたしのなまえは ～ です", "わたしの ～ です",

        "～ へいきたいです", "～ はどこですか？", "まっすぐです", "～ をください",

        "いくらですか？", "～ ありますか？", "すきです", "だいじょうぶです",

        "なに", "わかりません", "ぐあいがわるいです", "おいしい", "トイレ"};

    string[] first20_EN = new string[] {"Good Morning", "Hello", "Good Evening",

        "Good Night", "Thank You", "My name is ~", "This is my ~",

        "I want to go to ~", "Where is ~ ?", "Go straight", "May I have ~",

        "How much is it?", "Is there any ~ ?", "I like it", "I'm fine",

        "What?", "I don't understand", "I don't feel well", "Delicious", "Toilet"};


    #endregion


    int bladeNumber, enemyNumber, waveNumber;
    float timePassed;
    bool isPlayerDead, waveUp;

    public bool _playState, _waveIntroState, _multiplayerState, _menuState;

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
        if (!_waveIntroState || _menuState) { return; }    // if not in Wave Intro, then return

        waveText.SetText("Wave " + waveNumber);   // Write wave
        enemyBase.SetWaveNumber(waveNumber); enemyBase._waveIntroState = true;

        if (_multiplayerState) {    // if we're in multiState, jump to the end
            uiHandler.SetLoadQuestion(true);    // Start loading Questions
            uiHandler.interactableButtons = true;
            enemyBase.SetSpawn(true);           // Let enemy base spawn for now, will be inform about wave later on
            enemyBase.SetWaveNumber(waveNumber); enemyBase._playState = true;
            _waveIntroState = false; _playState = true; // Change State
            waveUp = false;
            return;
        }

        // Get it in screen
        if (!waveUp && waveText.transform.localPosition.y > 500) {
            timePassed = 0; // Reset up conditions
            waveText.transform.Translate(Vector2.down * 300f * Time.deltaTime);
        } 
        else if (!waveUp && waveText.transform.localPosition.y <= 500) { // Once we arrive the pos, wait in there
            timePassed++; ;
            if (timePassed > 50) {
                waveUp = true;
            }
        }

        // Get it out screen
        if (waveUp && waveText.transform.localPosition.y < 800) {   // if time is up
            waveText.transform.Translate(Vector2.up * 300f * Time.deltaTime);
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
        if (_menuState) { return; }
        bladeNumber = enemyNumber = 0;
        waveNumber = 1;  
        isPlayerDead = waveUp = false;     

        _playState = false; _waveIntroState = true;     // Initialize states

        uiHandler = FindObjectOfType<UI_Handler>();
        uiHandler.SetLoadQuestion(false);

        enemyBase = FindObjectOfType<EnemyBase>();
        enemyBase.SetSpawn(false);

        FBmanager = FindObjectOfType<FirebaseManager>();
    }

    // Update Character about firing conditions
    private void FireControl() {
        // Let player know the situation so that it can fire
        FindObjectOfType<Character>().SetBladeNumber(bladeNumber); // Let it know the blade number
    }    
    
    // Creates the content
    private void CreateContents() {
        int numberOfContent = 0;

        if (PlayerPrefs.GetInt("hiragana") == 1) {
            //Debug.Log("Hiragana Detected");
            numberOfContent += 20;
        }
        if (PlayerPrefs.GetInt("katakana") == 1) {
            //Debug.Log("Katakana Detected");
            numberOfContent += 20;
        }
        if (PlayerPrefs.GetInt("first20") == 1) {
            //Debug.Log("First20 Detected");
            numberOfContent += 20;
        }

        //Debug.Log("Number of Content:" + numberOfContent);
        contentPool = new Content[numberOfContent];
        int startingNumber = 0;

        // If there no option selected, then give them just Hiragana
        if (numberOfContent < 20) {
            //Debug.Log("Selected Option Number: 0");
        
            contentPool = new Content[20];
            for (int i = 0; i < 20; i++) {
                contentPool[i] = new Content(i, "hiragana", hiragana_JP[i], hiragana_EN[i]);
            }
        }
       
        // If there is 1 option selected
        else if (numberOfContent < 40) {
            //Debug.Log("Selected Option Number: 1");

            if (PlayerPrefs.GetInt("hiragana") == 1) {
                for (int i = 0; i < 20; i++) {
                    contentPool[i] = new Content(i, "hiragana", hiragana_JP[i], hiragana_EN[i]);
                }
                return; // if hiragana added, then finish the work
            }

            if (PlayerPrefs.GetInt("katakana") == 1) {
                for (int i = 0; i < 20; i++) {
                    contentPool[i] = new Content(i, "katakana", katakana_JP[i], katakana_EN[i]);
                }
                return; // if katakana added, then finish the work
            }

            if (PlayerPrefs.GetInt("first20") == 1) {
                for (int i = 0; i < 20; i++) {
                    contentPool[i] = new Content(i, "first20", first20_JP[i], first20_EN[i]);
                }
                return; // if first20 added, then finish the work
            }
        }
       
        // If there are 2 option selected
        else if (numberOfContent < 60) {
            //Debug.Log("Selected Option Number: 2");

            if (PlayerPrefs.GetInt("hiragana") == 1) {
                for (int i = startingNumber; i < startingNumber + 20; i++) {
                    contentPool[i] = new Content(i, "hiragana", hiragana_JP[i], hiragana_EN[i]);
                }
                startingNumber += 20;
                //Debug.Log("Starting Number: " + startingNumber);
            }
            // HERE ! tempAddresser variable for keep string array's position 0-20
            if (PlayerPrefs.GetInt("katakana") == 1) {
                int tempAddresser = 0;
                for (int i = startingNumber; i < startingNumber + 20; i++) {
                    contentPool[i] = new Content(i, "katakana", katakana_JP[tempAddresser], katakana_EN[tempAddresser]);
                    tempAddresser++;
                }
                startingNumber += 20;
                //Debug.Log("Starting Number: " + startingNumber);
            }

            if (PlayerPrefs.GetInt("first20") == 1) {
                int tempAddresser = 0;
                for (int i = startingNumber; i < startingNumber + 20; i++) {
                    contentPool[i] = new Content(i, "first20", first20_JP[tempAddresser], first20_EN[tempAddresser]);
                    tempAddresser++;
                }
                startingNumber += 20;
                //Debug.Log("Starting Number: " + startingNumber);
            }
        }
       
        // If all options selected
        else {
            Debug.Log("Selected Option Number: 3");

            if (PlayerPrefs.GetInt("hiragana") == 1) {
                for (int i = startingNumber; i < startingNumber + 20; i++) {
                    contentPool[i] = new Content(i, "hiragana", hiragana_JP[i], hiragana_EN[i]);
                }
                startingNumber += 20;
                //Debug.Log("Starting Number: " + startingNumber);
            }

            if (PlayerPrefs.GetInt("katakana") == 1) {
                int tempAddresser = 0;
                for (int i = startingNumber; i < startingNumber + 20; i++) {
                    contentPool[i] = new Content(i, "katakana", katakana_JP[tempAddresser], katakana_EN[tempAddresser]);
                    tempAddresser++;
                }
                startingNumber += 20;
                //Debug.Log("Starting Number: " + startingNumber);
            }

            if (PlayerPrefs.GetInt("first20") == 1) {
                int tempAddresser = 0;
                for (int i = startingNumber; i < startingNumber + 20; i++) {
                    contentPool[i] = new Content(i, "first20", first20_JP[tempAddresser], first20_EN[tempAddresser]);
                    tempAddresser++;
                }
                startingNumber += 20;
                //Debug.Log("Starting Number: " + startingNumber);
            }
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
    public void LoadLearningCanvas(string contentType) {
        if (contentType == "hiragana") {

            contentPool = new Content[20];
            for (int i = 0; i < 20; i++) {
                contentPool[i] = new Content(i, "hiragana", hiragana_JP[i], hiragana_EN[i]);
            }
        }
        if (contentType == "katakana") {

            contentPool = new Content[20];
            for (int i = 0; i < 20; i++) {
                contentPool[i] = new Content(i, "katakana", katakana_JP[i], katakana_EN[i]);
            }
        }
        if (contentType == "first20") {

            contentPool = new Content[20];
            for (int i = 0; i < 20; i++) {
                contentPool[i] = new Content(i, "first20", first20_JP[i], first20_EN[i]);
            }
        }

        uiHandler.LaydownQuestions();
        sectionsCanvas.SetActive(false);
    }
    #endregion

    // Increase wave number and start it in Enemy Base 
    public void StartNextWave() {
        waveNumber++;

        enemyBase._waveIntroState = true;
        _playState = false; _waveIntroState = true;
    }

    

    //  ****                            ****    //
    //  ****    Getters and Setters     ****    //
    //  ****                            ****    //

    public void SetBladeNumber(int bladeNumber) {
        this.bladeNumber = bladeNumber;
        // Update UI
        FindObjectOfType<UI_Handler>().SetBladeNumber(bladeNumber); 
    }

    // Changes number of the enemy
    public void ChangeEnemyNumber(int change) {  enemyNumber += change;  }

    // Sets players 
    public void SetCharacterStatus(bool isPlayerDead) { this.isPlayerDead = isPlayerDead;  }  
    

    public Content[] GetQuestionContent() { return contentPool; }

}
