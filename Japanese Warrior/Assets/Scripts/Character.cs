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

    public bool _playState, _menuState;
    bool fire, singlePlayerFire, multiplayerFire, menuFire;
    int energyValue, enemyNumber, menuEnemy;

    void Start()    {
        animator = GetComponentInChildren<Animator>();
        enemyNumber = 0;
        gameHandler = FindObjectOfType<GameHandler>();
    }


    void Update()    {
        PlayState();
        MenuState();
    }

    //  ==============================================   //
    //  =================   STATES   =================   //
    //  ==============================================   //

    private void PlayState() {
        if (!_playState) { return; }    // If not in Play State, then return

        singlePlayerFire = true; multiplayerFire = menuFire = false;

        if (energyValue > 0) {
            animator.SetBool("hasBlade", true); // Equip Blade
            if (enemyNumber > 0) {  // And if there an enemy
                fire = true;
            }
        }
        else {
            animator.SetBool("hasBlade", false);
            fire = false;
        }
        if (fire) {
            animator.SetTrigger("doAttack");
            StartCoroutine(Fire());
            fire = false;
        }
    }
    private void MenuState() {
        if (!_menuState) { return; }    // If not in Menu State, then return

        menuFire = true; multiplayerFire = singlePlayerFire = false;

        animator.SetBool("hasBlade", true); // Equip Blade

        if (menuEnemy > 0) {
            animator.SetTrigger("doAttack");
            StartCoroutine(Fire());
            menuEnemy--;
        }
    }


    //  ==============================================   //
    //  =============   Private Methods   ============   //
    //  ==============================================   //

    public void SetFire(bool fire) {
        this.fire = fire;
    }    

    private IEnumerator Fire() {
        if (singlePlayerFire) {
            energyValue--;
            gameHandler.ChangeEnemyNumber(-1);
            gameHandler.SetEnergyValue(energyValue);

            yield return new WaitForSeconds(0.4f);

            Projectile newProjectile = Instantiate(projectile, gunBarrel.transform.position, transform.rotation) as Projectile;
            newProjectile.transform.parent = transform;
            fireSound.Play();
        }
        if (menuFire) {
            yield return new WaitForSeconds(0.7f);

            Projectile newProjectile = Instantiate(projectile, gunBarrel.transform.position, transform.rotation) as Projectile;
            newProjectile.transform.parent = transform;
            fireSound.Play();
        }
    }





    //  ==============================================   //
    //  =============   Public Methods   =============   //
    //  ==============================================   //

    public void Killed() {
        deadSound.Play();   // Play dead sound
        transform.position = new Vector2(-100, -100);   // Send player out of range to simulate death
        gameHandler.SetCharacterStatus(true); // Let the GH know player is dead now
    }
    public void AddMenuEnemy() {
        menuEnemy++;
    }

    //  ****                            ****    //
    //  ****    Getters and Setters     ****    //
    //  ****                            ****    //

    public void SetEnergyValue(int energyValue) {
        this.energyValue = energyValue;
    }
    public void SetEnemyNumber(int enemyNumber) {
        this.enemyNumber = enemyNumber;
    }



}
