using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using Firebase;
using Firebase.Database;
using Firebase.Auth;

public class MenuHandler : MonoBehaviour{

    [SerializeField] GameObject menuCanvas = null;
    [SerializeField] GameObject optionsCanvas = null;
    [SerializeField] GameObject landingCanvas = null;
    [SerializeField] GameObject loginUI = null;
    [SerializeField] GameObject registerUI = null;
    [SerializeField] GameObject buttons = null;
    [SerializeField] GameObject matchingCanvas = null;
    [SerializeField] TextMeshProUGUI matchingText = null;
    [SerializeField] TextMeshProUGUI[] texts = null;

    Color activeColor;
    Color NotActiveColor;

    bool matchingUp;

    private void Awake() {
    }

    private void Start() {
        // If already loggedin, then no need to login UI. Bring menu now
        // if (PlayerPrefs.GetInt("loggedIN") == 1) { LoginSuccess(); }

        activeColor = texts[0].color;
        NotActiveColor = texts[0].color;
        activeColor.r = 0.07843138f; activeColor.g = 0.7843137f; activeColor.b = 0f; activeColor.a = 1f;
        NotActiveColor.r = 1f; NotActiveColor.g = 0.8627451f; NotActiveColor.b = 0.8627451f; NotActiveColor.a = 1f;

        matchingUp = false;

        // if this is first start on this device, set default options
        SetDefaultOptions();
    }

    private void Update() {
        MatchingTextAnim();
    }

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
    }

    //--------------------------//
    //      PUBLIC METHODS      //
    //--------------------------//

    public void OpenOptions() {
        optionsCanvas.SetActive(true);
        buttons.SetActive(false);

        FindObjectOfType<Algorithm>().OpenOptionsAction();
    }

    public void LoginSuccess() {
        menuCanvas.SetActive(true);
        landingCanvas.SetActive(false);
    }

    public void OpenRegisterUI() {
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
        // FBmanager.SignOut();    // User Sign-out
    }

    public void ToggleActivation(string optionName) {
        // if there no information in the system
        if (!PlayerPrefs.HasKey(optionName)) {
            Debug.Log("There is no information about " + optionName);
            return; 
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

            LoadSceneByName("MultiplayerScene");
        }
    }

    //      Private Methods     //

   
}
