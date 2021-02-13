using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour{


    int energyValue, enemyNumber;
    bool isPlayerDead;

    Content[] contentPool = new Content[20];
    string[] japaneseContent = new string[] {"あ" + "い" + "う" + "え" + "お" + "か" + "き"
    + "く" + "け" + "こ" + "が"+ "ぎ"+ "ぐ"+ "げ"+ "ご"+ "さ"+ "し"+ "す"+ "せ"+ "そ"};
    string[] englishContent = new string[] {"a" + "i" + "u" + "e" + "o" + "ka" + "ki"
    + "ku" + "ke" + "ko" + "ga"+ "gi"+ "gu"+ "ge"+ "go"+ "sa"+ "shi"+ "su"+ "se"+ "so"};

    void Start()    {
        ResetGameParamaters();
        CreateContents();
    }


    void Update()    {
        FireControl();
        GameStatusControl();
    }

    //  ==============================================   //
    //  =================   In-Game  =================   //
    //  ==============================================  //

    #region Setters

    public void SetEnergyValue(int energyValue) {
        this.energyValue = energyValue;
        FindObjectOfType<UI_Handler>().SetEnergyValue(energyValue); // Update UI
    }
    public void ChangeEnemyNumber(int change) {
        enemyNumber += change;
    }            // Changes number of the enemy
    public void SetCharacterStatus(bool isPlayerDead) {
        this.isPlayerDead = isPlayerDead;
    }       // Sets players

    #endregion

    #region Getters
    public int GetEnergyValue() {
        return energyValue;
    }
    #endregion

    #region Private Methods

    private void FireControl() {
        // Let player know the situation so that it can fire
        FindObjectOfType<Character>().SetEnergyValue(energyValue); // Let it know the energy Value
        FindObjectOfType<Character>().SetEnemyNumber(enemyNumber); // Let it know enemy number
    }

    // Resets all game values
    private void ResetGameParamaters() {
        energyValue = enemyNumber = 0;  // Integer values
        isPlayerDead = false;           // Boolean values
    }   

    private void CreateContents() {
        // 20 Hiragana Creation. I'll add others later on
        for (int contentID = 0; contentID < contentPool.Length; contentID++){
            contentPool[contentID] = new Content(contentID, "hiragana", japaneseContent[contentID], englishContent[contentID]);
        }
    }
    
    private void GameStatusControl() {

        // Check is player dead or not
        if (isPlayerDead) {
            // if players is dead, then go back to menu for now
            StartCoroutine(LoadAfterGameScene());   // But after 3 seconds
        }
    }
    
    //Control Game Status
    private IEnumerator LoadAfterGameScene() {
        yield return new WaitForSeconds(3);
        LoadSceneByName("Menu");
    }

    #endregion


    //  ==============================================   //
    //  =================   Buttons  =================   //
    //  ==============================================  //
    #region Button Functions
    public void LoadSceneByName(string SceneName) {
        SceneManager.LoadScene(SceneName);
    }
    #endregion
}
