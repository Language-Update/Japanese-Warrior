using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LazyLogin : MonoBehaviour {

    [SerializeField] GameObject characterObject = null;
    [SerializeField] GameObject LazyCanvas = null;
    [SerializeField] GameObject menuCanvas = null;
    [SerializeField] GameObject usernameCanvas = null;
    [SerializeField] GameObject learningPreferences = null;
    [SerializeField] GameObject guideCanvas = null;
    [SerializeField] GameObject[] guidePhotos = null;
    [SerializeField] TMP_InputField usernameInput = null;
    [SerializeField] TextMeshProUGUI usernameText = null;
    [SerializeField] Button[] buttons = null;


    FirebaseManager FBmanager;

    int guideCount = 0;
    ColorBlock selectedColor;
    ColorBlock notSelectedColor;


    private void Awake() {
        // Lazy Login feature
        if (PlayerPrefs.HasKey("firstLogin")) {
            if (PlayerPrefs.GetString("firstLogin") == "yes") {
                StartLazyLogin();
            }
            else {  // means user already gone through lazy login
                LazyCanvas.SetActive(false);
                menuCanvas.SetActive(true);
                characterObject.SetActive(true);
            }
        }
        else { // means it is very-first time to open the applaction
            PlayerPrefs.SetString("firstLogin", "yes");
            StartLazyLogin();
        }
    }

    // Start is called before the first frame update
    void Start() {
        FBmanager = FindObjectOfType<FirebaseManager>();
        guideCount = 0;
        notSelectedColor = selectedColor = buttons[0].colors;
        notSelectedColor.selectedColor = new Color(0.756f, 0.756f, 0.756f);
        selectedColor.normalColor = new Color(1f, 1f, 1f);
        selectedColor.selectedColor = new Color(1f, 1f, 1f);
    }
    
    private void StartLazyLogin() {
        // Show Lazy Landing Page
        PlayerPrefs.DeleteAll(); // clear all prefs becase we will selct the ones we whant to learn
        menuCanvas.SetActive(false);
        characterObject.SetActive(false);
        LazyCanvas.SetActive(true);
    }
    public void SetUsername() {
        if (usernameInput.text == "") {
            Debug.LogError("Username Shouldn't be empty!");
        }
        else {
            // Store username with the unique ID you created
            string username = usernameInput.text;
            string tempUserID = FBmanager.GetUniqueID("users");
            PlayerPrefs.SetString("lazyUsername", username);
            PlayerPrefs.SetString("lazyUserID", tempUserID);
            PlayerPrefs.SetString("userEmail", "");
            PlayerPrefs.SetInt("lazyNumberOfMatch", 0);

            string dataPath = "Lazy Users/" + tempUserID + "/username";
            string[,] dataWrite = { { dataPath }, { username } };
            FBmanager.WriteData(dataWrite, false);
            usernameText.SetText(username);

            // Ask what player wants to learn
            usernameCanvas.SetActive(false);
            learningPreferences.SetActive(true);

            // Give all the contents that 
        }
    }
    public void SetLearningPreferences() {
        // Saved! what to learn
        // Show guide
        learningPreferences.SetActive(false);
        guideCanvas.SetActive(true);
    }
    public void GuideProcces(string _input) {
        Debug.Log("Button Clicked: " + _input);
        if (_input == "next") {
            Debug.Log("Log #0 - in NEXT");
            // if it was not the last one, jump to the next one
            if (guideCount < guidePhotos.Length - 1) {
                Debug.Log("Log #1 - Next Execution");
                guidePhotos[guideCount].SetActive(false);   // close the current photo
                guideCount++;                               // so the next one appears
            }
            else { // if it was the last one, show the menu
                Debug.Log("Log #2 - Next finished");
                PlayerPrefs.SetString("firstLogin", "no");  // Set that lazy login is complete
                guideCanvas.SetActive(false);
                LazyCanvas.SetActive(false);
                menuCanvas.SetActive(true);
                characterObject.SetActive(true);
                FindObjectOfType<MenuHandler>().UpdateProfile();
            }
        }
        else {
            Debug.Log("Log #3 - in BACK");
            // if it was not the FIRST one, jump to the back
            if (guideCount != 0) {
                Debug.Log("Log #4 - Back execution");
                guideCount--;                               // get back the previous one
                guidePhotos[guideCount].SetActive(true);   // open it again
            }
        }
    }
    public void ToggleLearningActivation(string _selection) {
        // if there is no record, then open one.
        if (!PlayerPrefs.HasKey(_selection)) {
            PlayerPrefs.SetInt(_selection, 1);
            foreach (Button button in buttons) {
                Debug.Log("HAS NOT KEY");
                Debug.Log("Testing: " + button.gameObject.name + "    and comparing with: " + _selection);
                if (button.gameObject.name == _selection) {
                    button.colors = selectedColor;
                }
            }
        }
        else { // if there is a record, then toggle it.
            if (PlayerPrefs.GetInt(_selection) == 0) {
                PlayerPrefs.SetInt(_selection, 1);
                Debug.Log("HAS KEY 0 => 1");
                foreach (Button button in buttons) {
                    Debug.Log("Testing: " + button.gameObject.name + "    and comparing with: " + _selection);
                    if (button.gameObject.name == _selection) {
                        button.colors = selectedColor;
                    }
                }
            }
            else {
                PlayerPrefs.SetInt(_selection, 0);
                Debug.Log("HAS KEY 1 => 0");
                foreach (Button button in buttons) {
                    if (button.gameObject.name == _selection) {
                        button.colors = notSelectedColor;
                    }
                }
            }
        }        
    }
}
