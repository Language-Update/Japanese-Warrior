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
    }

    public void ProfilePicButton() {
        if (profileON) { profileON = false; profileAnimChange = true; }            
        else { profileON = true; profileAnimChange = true; }
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
