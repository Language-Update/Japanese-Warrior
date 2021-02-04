using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour{


    int energyValue;
    float timePassed;

    void Start()    {
        
    }


    void Update()    {
        timePassed += Time.deltaTime;
        //Debug.Log("GM.Energy : " + energyValue);
        if (energyValue > 0 && timePassed > 0.5f) {
            Debug.Log("GM: Fired at " + timePassed);
            FindObjectOfType<Player>().SetFire(true);   // Let player fire 
            energyValue--;  // Decrease energy 
            FindObjectOfType<UI_Manager>().SetEnergyValue(energyValue); // Update GUI
            timePassed = 0f;    // Reset Timer
        }
    }

    public void SetEnergyValue(int energyValue) {
        this.energyValue = energyValue;
    }
}
