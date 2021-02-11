using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour{


    int energyValue, enemyNumber;
    bool isPlayerDead;

    void Start()    {
        ResetGameParamaters();
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

    private void ResetGameParamaters() {
        energyValue = enemyNumber = 0;  // Integer values
        isPlayerDead = false;           // Boolean values
    }   // Resets all game values
    private void GameStatusControl() {

        // Check is player dead or not
        if (isPlayerDead) {
            // if players is dead, then go back to menu for now
            StartCoroutine(LoadAfterGameScene());   // But after 3 seconds
        }
    }       //Control Game Status
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
