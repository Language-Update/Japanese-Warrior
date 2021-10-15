using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Database;
using Firebase.Auth;

public class MenuHandler : MonoBehaviour{

    [SerializeField] GameObject menuCanvas = null;
    [SerializeField] GameObject optionsCanvas = null;
    [SerializeField] GameObject loginCanvas = null;
    [SerializeField] GameObject characterGameObject = null;
    [SerializeField] GameObject enemyBase = null;
    [SerializeField] GameObject loginUI = null;
    [SerializeField] GameObject registerUI = null;
    [SerializeField] GameObject buttons = null;
    [SerializeField] GameObject matchingCanvas = null;
    [SerializeField] TextMeshProUGUI matchingText = null;
    [SerializeField] TextMeshProUGUI[] texts = null;
    // Profile
    [SerializeField] TextMeshProUGUI usernameText = null;
    [SerializeField] TextMeshProUGUI userEmail = null;
    [SerializeField] TextMeshProUGUI numberOfMatchText = null;
    [SerializeField] TextMeshProUGUI answeredQuestionsText = null;
    [SerializeField] TextMeshProUGUI SuccessRateText = null;
    [SerializeField] GameObject profilePage = null;
    [SerializeField] GameObject registerSection = null;
    [SerializeField] GameObject logOutButton = null;

    Color activeColor;
    Color NotActiveColor;

    FirebaseManager FBmanager;

    bool matchingUp;
    bool profileON;
    bool profileAnimChange;


    #region Content Part

    // Hiragana
    string[] hiragana_JP = new string[] {"あ", "い", "う", "え", "お",
        "か", "き", "く", "け", "こ", "が", "ぎ", "ぐ", "げ", "ご",
        "さ", "し", "す", "せ", "そ", "ざ", "じ", "ず", "ぜ", "ぞ",
        "た", "ち", "つ", "て", "と", "だ", "ぢ", "づ", "で", "ど",
        "な", "に", "ぬ", "ね", "の", "は", "ひ", "ふ", "へ", "ほ",
        "ば", "び", "ぶ", "べ", "ぼ", "ぱ", "ぴ", "ぷ", "ぺ", "ぽ",
        "ま", "み", "む", "め", "も", "や", "ゆ", "よ", "ら", "り",
        "る", "れ", "ろ", "わ", "を", "きゃ", "きゅ",  "きょ", "しゃ",
        "しゅ", "しょ", "ちゃ", "ちゅ", "ちょ", "にゃ", "にゅ", "にょ",
        "ひゃ", "ひゅ", "ひょ", "みゃ", "みゅ", "みょ", "りゃ", "りゅ",
        "りょ", "ぎゃ", "ぎゅ", "ぎょ", "じゃ", "じゅ", "じょ", "びゃ", "びゅ",
        "びょ", "ぴゃ", "ぴゅ", "ぴょ", "ん"
    };

    string[] hiragana_EN = new string[] {"a", "i", "u", "e", "o",
        "ka", "ki", "ku", "ke", "ko", "ga", "gi", "gu", "ge", "go",
        "sa", "shi", "su", "se", "so", "za", "ji", "zu", "ze", "zo",
        "ta", "chi", "tsu", "te", "to", "da", "ji", "zu", "de", "do",
        "na", "ni", "nu", "ne", "no", "ha", "hi", "fu", "he", "ho",
        "ba", "bi", "bu", "be", "bo", "pa", "pi", "pu", "pe", "po",
        "ma", "mi", "mu", "me", "mo", "ya", "yu", "yo", "ra", "ri",
        "ru", "re", "ro", "wa", "wo", "kya ", "kyu ", "kyo ", "sha",
        "shu ", "sho ", "cha ", "chu ", "cho", "nya", "nyu", "nyo",
        "hya", "hyu", "hyo", "mya", "myu", "myo", "rya", "ryu",
        "ryo", "gya", "gyu", "gyo", "ja", "ju", "jo", "bya", "byu",
        "byo", "pya", "pyu", "pyo", "n"
    };

    // Katakana
    string[] katakana_JP = new string[] {"ア", "イ", "ウ", "エ", "オ",
        "カ", "キ", "ク", "ケ", "コ", "ガ", "ギ", "グ", "ゲ", "ゴ",
        "サ", "シ", "ス", "セ", "ソ", "ザ", "ジ", "ズ", "ゼ", "ゾ",
        "タ", "チ", "ツ", "テ", "ト", "ダ", "ヂ", "ヅ", "デ", "ド",
        "ナ", "ニ", "ヌ", "ネ", "ノ", "ハ", "ヒ", "フ", "ヘ", "ホ",
        "バ", "ビ", "ブ", "ベ", "ボ", "パ", "ピ", "プ", "ペ", "ポ",
        "マ", "ミ", "ム", "メ", "モ", "ヤ", "ユ", "ヨ", "ラ", "リ", "ル", "レ", "ロ",
        "ワ", "ヲ", "ン", "キャ", "キュ", "キョ", "シャ", "シュ", "ショ",
        "チャ", "チュ", "チョ", "ニャ", "ニュ", "ニョ", "ヒャ", "ヒュ", "ヒョ",
        "ミャ", "ミュ", "ミョ", "リャ", "リュ", "リョ", "ギャ", "ギュ", "ギョ",
        "ジャ", "ジュ", "ジョ", "ビャ", "ビュ", "ビョ", "ピャ", "ピュ", "ピョ",
    };

    string[] katakana_EN = new string[] {"a", "i", "u", "e", "o",
        "ka", "ki", "ku", "ke", "ko", "ga", "gi", "gu", "ge", "go",
        "sa", "shi", "su", "se", "so", "zo", "ji", "zu", "ze", "zo",
        "ta", "chi", "tsu", "te", "to", "da", "ji", "zu", "de", "do",
        "na", "ni", "nu", "ne", "no", "ha", "hi", "hu", "he", "ho",
        "ba", "bi", "bu", "be", "bo", "pa", "pi", "pu", "pe", "po",
        "ma", "mi", "mu", "me", "mo", "ya", "yu", "yo", "ra", "ri", "ru", "re", "ro",
        "wa", "wo", "n", "kya", "kyu", "kyo", "sha", "shu", "sho",
        "cha", "chu", "cho", "nya", "nyu", "nyo", "hya", "hyu", "hyo",
        "mya", "myu", "myo", "rya", "ryu", "ryo", "gya", "gyu", "gyo",
        "ja", "ju", "jo", "bya", "byu", "byo", "pya", "pyu", "pyo",
    };

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


    private void Awake() {
    }

    private void Start() {
        activeColor = texts[0].color;
        NotActiveColor = texts[0].color;
        activeColor.r = 0.07843138f; activeColor.g = 0.7843137f; activeColor.b = 0f; activeColor.a = 1f;
        NotActiveColor.r = 1f; NotActiveColor.g = 0.8627451f; NotActiveColor.b = 0.8627451f; NotActiveColor.a = 1f;

        matchingUp = false;


        // Reset profile Status
        profileON = false;
        profileAnimChange = true;

        FBmanager = FindObjectOfType<FirebaseManager>();
        UpdateProfile();
    }

    private void Update() {
        MatchingTextAnim();
        ProfileAnim();
    }

    //      Private Methods     //

    private void MatchingTextAnim() {
        var alpha = matchingText.alpha;

        if (alpha >= 1) {
            matchingUp = false;
        }
        else if (alpha <= 0) {
            matchingUp = true;
        }

        if (matchingUp) { alpha += 1f * Time.deltaTime; }
        else { alpha -= 1.5f * Time.deltaTime; }

        matchingText.alpha = alpha;
    }

    void ProfileAnim() {
        if (profileAnimChange) {
            if (profileON && profilePage.transform.localPosition.x < -165) {
                profilePage.transform.Translate(Vector2.right * 1000 * Time.deltaTime);
            }
            else if (profileON && profilePage.transform.localPosition.x >= -165) { // Keep Y pos, set X pos to 195 to avoid gap
                profilePage.transform.localPosition = new Vector2(-165f, profilePage.transform.localPosition.y);
            }
            else if (!profileON && profilePage.transform.localPosition.x > -590) {
                profilePage.transform.Translate(Vector2.left * 1000 * Time.deltaTime);
            }
            else if (!profileON && profilePage.transform.localPosition.x <= -590) { // Keep Y pos, set X pos to -230 to avoid gap
                profilePage.transform.localPosition = new Vector2(-590, profilePage.transform.localPosition.y);
            }
        }
    }
    /*
    private void SetDefaultOptions() {     

        // it part for first time
        foreach (TextMeshProUGUI text in texts) {
            // if it's hiragana and there no record, then set it Active
            if (text.name == "hiragana") {
                if (!PlayerPrefs.HasKey(text.name)) {
                    Debug.Log(text.name + " CREATED");
                    text.color = activeColor;
                    PlayerPrefs.SetInt(text.name, 1);
                }
            }
            // if it's not hiragana and there no record, then set it Deactive
            else {
                if (!PlayerPrefs.HasKey(text.name)) {
                    Debug.Log(text.name + " CREATED");
                    text.color = NotActiveColor;
                    PlayerPrefs.SetInt(text.name, 0);
                }
            }
        }

        // this part for update when it opens
        foreach (TextMeshProUGUI text in texts) {
            if (PlayerPrefs.GetInt(text.name) == 1)
                text.color = activeColor;
            else
                text.color = NotActiveColor;
        }
    }*/
    //--------------------------//
    //      PUBLIC METHODS      //
    //--------------------------//

    // =======  Buttons  ======= //
    public void OpenOptions() {
        optionsCanvas.SetActive(true);
        buttons.SetActive(false);

        // Check activation status of the preferences and update
        #region CHECKING 
        if (PlayerPrefs.HasKey("hiragana")) { // if there a record. Then update
            if (PlayerPrefs.GetInt("hiragana") == 1)
                texts[0].color = activeColor;
            else
                texts[0].color = NotActiveColor;
        }
        if (PlayerPrefs.HasKey("katakana")) { // if there a record. Then update
            if (PlayerPrefs.GetInt("katakana") == 1)
                texts[1].color = activeColor;
            else
                texts[1].color = NotActiveColor;
        }
        if (PlayerPrefs.HasKey("first20")) { // if there a record. Then update
            if (PlayerPrefs.GetInt("first20") == 1)
                texts[2].color = activeColor;
            else
                texts[2].color = NotActiveColor;
        }
        #endregion

        //FindObjectOfType<Algorithm>().OpenOptionsAction();
    }

    public void LoginSuccess() {
        menuCanvas.SetActive(true);
        characterGameObject.SetActive(true);
        enemyBase.SetActive(true);
        loginCanvas.SetActive(false);
        UpdateProfile();
    }

    public void OpenRegisterUI() {
        menuCanvas.SetActive(false);
        characterGameObject.SetActive(false);
        enemyBase.SetActive(false);
        loginCanvas.SetActive(true);
        loginUI.SetActive(false);
        registerUI.SetActive(true);
    }

    public void RegisterSuccess() {
        registerUI.SetActive(false);
        loginUI.SetActive(true);
    }

    public void BackToMenu() {
        PlayerPrefs.Save();

        optionsCanvas.SetActive(false);
        buttons.SetActive(true);
    }

    public void QuitButton() { 
        Application.Quit();
    }
    public void LogOutButton() {
        PlayerPrefs.SetString("firstLogin", "yes");
        FBmanager.SignOut();    // User Sign-out
        Application.Quit();     // Quit App
    }

    public void ProfilePicButton() {
        if (profileON) { profileON = false; profileAnimChange = true; }            
        else { profileON = true; profileAnimChange = true; }

        // Do it once - Upload all the contents into EN-JP with their categories
        // hiragana
        /*
        int numberOfData = hiragana_EN.Length;
        Debug.Log("Number of data EN: " + numberOfData);
        Debug.Log("Number of data JP: " + hiragana_JP.Length);

        string[,] initialData = new string[numberOfData, numberOfData];
        for (int i = 0; i < numberOfData; i++) {
            // Create an ID
            string ID = "448100";           // 44-81 EN-JP, 00 - Hiragana
            if (i < 10) { ID += "000" + i; }
            else if (i < 100) { ID += "00" + i; }
            else { ID += "0" + i; }

            Content newContent = new Content(ID, "letter", "hiragana", "EN-JP",
                hiragana_JP[i], hiragana_EN[i]);

            initialData[0, i] = "EN-JP/hiragana/" + ID;
            initialData[1, i] = JsonUtility.ToJson(newContent);
        }
        FBmanager.WriteJsonData(initialData);
        */
        // katakana
        /*
        int numberOfData = katakana_EN.Length;
        Debug.Log("Number of data EN: " + numberOfData);
        Debug.Log("Number of data JP: " + katakana_JP.Length);

        string[,] initialData = new string[numberOfData, numberOfData];
        for (int i = 0; i < numberOfData; i++) {
            // Create an ID
            string ID = "448101";           // 44-81 EN-JP | 01 - Katakana
            if (i < 10) { ID += "000" + i; }
            else if (i < 100) { ID += "00" + i; }
            else { ID += "0" + i; }

            Content newContent = new Content(ID, "letter", "katakana", "EN-JP",
                katakana_JP[i], katakana_EN[i]);

            initialData[0, i] = "EN-JP/katakana/" + ID;
            initialData[1, i] = JsonUtility.ToJson(newContent);
        }
        FBmanager.WriteJsonData(initialData);
        */
        // first 20
        /*
        int numberOfData = first20_EN.Length;
        Debug.Log("Number of data EN: " + numberOfData);
        Debug.Log("Number of data JP: " + first20_JP.Length);

        string[,] initialData = new string[numberOfData, numberOfData];
        for (int i = 0; i < numberOfData; i++) {
            // Create an ID
            string ID = "448102";           // 44-81 EN-JP, 02 - first20
            if (i < 10) { ID += "000" + i; }
            else if (i < 100) { ID += "00" + i; }
            else { ID += "0" + i; }

            Content newContent = new Content(ID, "letter", "first20", "EN-JP",
                first20_JP[i], first20_EN[i]);

            initialData[0, i] = "EN-JP/first20/" + ID;
            initialData[1, i] = JsonUtility.ToJson(newContent);
        }
        FBmanager.WriteJsonData(initialData);
        */
    }


    // =======  Others  ======= //
    public void ToggleActivation(string optionName) {
        // if there no information in the system then open one
        if (!PlayerPrefs.HasKey(optionName)) { // then open one
            PlayerPrefs.SetInt(optionName, 1);

            foreach (TextMeshProUGUI text in texts) {
                if (text.name == optionName)
                    text.color = activeColor;
            }
        }

        if (PlayerPrefs.GetInt(optionName) == 0) { 
            PlayerPrefs.SetInt(optionName, 1);

            foreach (TextMeshProUGUI text in texts) {
                if (text.name == optionName)
                    text.color = activeColor;
            }
        }
        else { 
            PlayerPrefs.SetInt(optionName, 0);

            foreach (TextMeshProUGUI text in texts) {
                if (text.name == optionName)
                    text.color = NotActiveColor;
            }
        }
    }
    public void LoadSceneByName(string SceneName) {
        SceneManager.LoadScene(SceneName);
    }

    public void Matching() {
        buttons.SetActive(false);
        matchingCanvas.SetActive(true);

        StartCoroutine(LoadMultiplayerScene());
        IEnumerator LoadMultiplayerScene() {
            float matchingTime = Random.Range(3f, 10f);
            yield return new WaitForSeconds(matchingTime);
            // Increase number of match user have been
            int currentMatch = PlayerPrefs.GetInt("lazyNumberOfMatch");
            currentMatch++;
            PlayerPrefs.SetInt("lazyNumberOfMatch", currentMatch);
            LoadSceneByName("MultiplayerScene");
        }
    }
    public void UpdateProfile() {
        // Dislpay profile Info
        usernameText.text = PlayerPrefs.GetString("lazyUsername");
        numberOfMatchText.text = PlayerPrefs.GetInt("lazyNumberOfMatch").ToString();
        if (PlayerPrefs.GetString("userEmail") != "") // if we already set an email, then write it
            userEmail.text = PlayerPrefs.GetString("userEmail");
        else // else the user has not registered yet, then write this
            userEmail.text = "Please Register";

        // Display Statistics
        float answeredQuestions = PlayerPrefs.GetFloat("answeredQuestions");
        float trueAnswers = PlayerPrefs.GetFloat("trueAnswers");
        float successRate = (trueAnswers / answeredQuestions) * 100;

        answeredQuestionsText.text = answeredQuestions.ToString();
        SuccessRateText.text = successRate.ToString("0.00") + "%";

        // Remove Register section if user logged in
        string currentUser = FBmanager.GetUsername();
        if (currentUser == PlayerPrefs.GetString("lazyUsername")) { 
            registerSection.SetActive(false); 
            logOutButton.SetActive(true); 
        }
        else { 
            registerSection.SetActive(true); 
            logOutButton.SetActive(false); 
            StartCoroutine(TryLoginAgainLater()); 
        }
    }
    IEnumerator TryLoginAgainLater() {
        Debug.Log("Trying again to login !!");
        yield return new WaitForSeconds(2);
        // Remove Register section if user logged in
        string currentUser = FBmanager.GetUsername();
        if (currentUser == PlayerPrefs.GetString("lazyUsername")) {
            registerSection.SetActive(false);
            logOutButton.SetActive(true);
        }
        else {
            registerSection.SetActive(true);
            logOutButton.SetActive(false);
        }
    }
   
}
