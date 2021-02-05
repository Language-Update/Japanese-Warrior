using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameHandler : MonoBehaviour{


    int energyValue, enemyNumber;
    float timePassed;
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
    }
    public void ChangeEnemyNumber(int change) {
        enemyNumber += change;
    }            // Changes number of the enemy
    public void SetPlayerStatus(bool isPlayerDead) {
        this.isPlayerDead = isPlayerDead;
    }       // Sets players

    #endregion

    #region Getters

    #endregion

    #region Private Methods

    private void FireControl() {
        timePassed += Time.deltaTime;
        //Debug.Log("GM.Energy : " + energyValue);
        if (energyValue > 0 && timePassed > 0.5f && enemyNumber > 0) {
            enemyNumber--;  // Decrease enemy number because this projectile will kill one certainly
            // The reason why I decrease enemy number not by death of the enemy is if I do that, while projectile
            // goes to the enemy, player still continues to fire. Because there is stil an enemy. 
            FindObjectOfType<Player>().SetFire(true);   // Let player fire 
            energyValue--;  // Decrease energy 
            FindObjectOfType<UIManager>().SetEnergyValue(energyValue); // Update GUI
            timePassed = 0f;    // Reset Timer
        }
    }   // Controls Fire conditions
    private void ResetGameParamaters() {
        energyValue = enemyNumber = 0;  // Integer values
        timePassed = 0;                 // Float values
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
