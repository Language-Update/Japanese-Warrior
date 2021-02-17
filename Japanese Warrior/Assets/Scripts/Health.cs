using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour{

    [SerializeField] int health = 100;
    [SerializeField] bool displayAlways;

    [SerializeField] Canvas healthCanvas;


    private void Start() {
        // initialize current health
        healthCanvas.GetComponentInChildren<Slider>().maxValue = health;
        healthCanvas.GetComponentInChildren<Slider>().value = health;

        // If we want to display healt bar always, we set it, otherwise close it
        if (displayAlways)
            healthCanvas.enabled = true;
        else
            healthCanvas.enabled = false;        
    }

    public void DealDamage(int damage) {
        // process damage
        health -= damage;

        // display the health bar after getting damage
        healthCanvas.enabled = true;

        // Update the heatl bar
        healthCanvas.GetComponentInChildren<Slider>().value = health;

        // Call PerformDeath method of the object
        if (health <= 0) {
            if (TryGetComponent<Character>(out var character)) { character.PerformDeath(); }
            if (TryGetComponent<Opponent>(out var opponent)) { opponent.PerformDeath(); }
            if (TryGetComponent<Turtle>(out var turtle)) { turtle.PerformDeath(); }
            if (TryGetComponent<Projectile>(out var projectile)) { projectile.PerformDeath(); }
        }
    }

    public int GetHealth() { return health; }
}
