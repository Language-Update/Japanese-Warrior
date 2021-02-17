using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuHandler : MonoBehaviour{

    [SerializeField] GameObject optionsCanvas = null;
    [SerializeField] GameObject buttons = null;

    [SerializeField] TextMeshProUGUI[] texts;

    Color activeColor;
    Color NotActiveColor;

    private void Start() {
        activeColor = texts[0].color;
        NotActiveColor = texts[0].color;
        activeColor.r = 0.07843138f; activeColor.g = 0.7843137f; activeColor.b = 0f; activeColor.a = 1f;
        NotActiveColor.r = 1f; NotActiveColor.g = 0.8627451f; NotActiveColor.b = 0.8627451f; NotActiveColor.a = 1f;

        // if this is first start on this device, set default options
        SetDefaultOptions();
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

    public void OpenOptions() {
        optionsCanvas.SetActive(true);
        buttons.SetActive(false);
    }

    public void BackToMenu() {
        PlayerPrefs.Save();

        optionsCanvas.SetActive(false);
        buttons.SetActive(true);
    }

    public void QuitButton() { Application.Quit(); ; }

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

}
