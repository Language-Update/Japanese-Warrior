using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour{

    [SerializeField] Projectile projectile;
    [SerializeField] GameObject gunBarrel;
    [SerializeField] AudioSource fireSound;
    [SerializeField] AudioSource deadSound;

    Animator animator;
    GameHandler gameHandler;

    bool fire;
    int energyValue, enemyNumber;

    void Start()    {
        animator = GetComponentInChildren<Animator>();
        enemyNumber = 0;
        gameHandler = FindObjectOfType<GameHandler>();
    }


    void Update()    {
        if (energyValue > 0) {
            animator.SetBool("hasBlade", true); // Equip Blade
                if (enemyNumber > 0) {  // And if there an enemy
                fire = true;
            }
        } else {
            animator.SetBool("hasBlade", false);
            fire = false;
        }

        if (fire) {
            animator.SetTrigger("doAttack");
            StartCoroutine(Fire());
            fire = false;
        }
    }

    public void SetFire(bool fire) {
        this.fire = fire;
    }

    public void Killed() {
        deadSound.Play();   // Play dead sound
        transform.position = new Vector2(-100, -100);   // Send player out of range to simulate death
        gameHandler.SetCharacterStatus(true); // Let the GH know player is dead now
    }

    private IEnumerator Fire() {
        energyValue--;
        gameHandler.ChangeEnemyNumber(-1);
        gameHandler.SetEnergyValue(energyValue);

        yield return new WaitForSeconds(0.4f);
        
        Projectile newProjectile = Instantiate(projectile, gunBarrel.transform.position, transform.rotation) as Projectile;
        newProjectile.transform.parent = transform;
        fireSound.Play();
    }



    public void SetEnergyValue(int energyValue) {
        this.energyValue = energyValue;
    }
    public void SetEnemyNumber(int enemyNumber) {
        this.enemyNumber = enemyNumber;
    }



}
