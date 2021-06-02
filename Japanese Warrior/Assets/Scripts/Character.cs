using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour{

    [SerializeField] Projectile projectile = null;
    [SerializeField] GameObject gunBarrel = null;
    [SerializeField] int damagePoint = 100;

    Animator animator;
    GameHandler gameHandler;

    public bool _playState, _menuState, _multiplayerState;
    bool fire, fireCompleted, singlePlayerFire, menuFire;
    int numberOfBlade, bladeNeeded, menuEnemy;

    // Initiate the referances and values
    void Start()    {
        animator = GetComponentInChildren<Animator>();
        bladeNeeded = 0;
        gameHandler = FindObjectOfType<GameHandler>();
        fireCompleted = true;
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

        singlePlayerFire = true; menuFire = false;

        if (_multiplayerState) { this.bladeNeeded = 10; }       // Always need blade in multi
        //Debug.Log("I have blade ------>     " + numberOfBlade);

        if (numberOfBlade > 0) {
            animator.SetBool("hasBlade", true); // Equip Blade
            if (bladeNeeded > 0 && fireCompleted) {  // If there is enemy and we completed the fire
                fire = true; fireCompleted = false; // Then fire
            }
        }
        else {
            animator.SetBool("hasBlade", false);
            fire = false;
        }
        if (fire) {
            animator.SetTrigger("doAttack");
            StartCoroutine(Fire());
            Fire();
            fire = false;
        }
    }
    private void MenuState() {
        if (!_menuState) { return; }    // If not in Menu State, then return

        menuFire = true; singlePlayerFire = false;

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
  

    private IEnumerator Fire() {
        // If it is singleplayer then decrease number of blade 
        if (singlePlayerFire) {
            numberOfBlade--;
            ChangeBladeNeeded(-1);
            gameHandler.SetBladeNumber(numberOfBlade);

            yield return new WaitForSeconds(0.4f);

            Projectile newProjectile = Instantiate(projectile, gunBarrel.transform.position, transform.rotation) as Projectile;
            newProjectile.transform.parent = transform;
            FindObjectOfType<AudioManager>().Play("Fire");

            fireCompleted = true;
        }
        if (menuFire) {
            yield return new WaitForSeconds(0.7f);

            Projectile newProjectile = Instantiate(projectile, gunBarrel.transform.position, transform.rotation) as Projectile;
            newProjectile.transform.parent = transform;
            FindObjectOfType<AudioManager>().Play("Fire");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        collision.GetComponent<Health>().DealDamage(damagePoint);
    }

    //  ==============================================   //
    //  =============   Public Methods   =============   //
    //  =============== communication ================   //

    public void SetFire(bool fire) {
        this.fire = fire;
    }
    public void PerformDeath() {
        FindObjectOfType<AudioManager>().Play("DyingMan");   // Play dead sound
        transform.position = new Vector2(-100, -100);   // Send player out of range to simulate death
        gameHandler.SetCharacterStatus(true); // Let the GH know player is dead now
    }
    public void AddMenuEnemy() {
        menuEnemy++;
    }

    //  ****                            ****    //
    //  ****    Getters and Setters     ****    //
    //  ****                            ****    //

    public void SetBladeNumber(int bladeNumber) {
        this.numberOfBlade = bladeNumber;
    }
    public void ChangeBladeNeeded(int neededBladeChange) {
        this.bladeNeeded += neededBladeChange;        
        if (this.bladeNeeded <= 0) { this.bladeNeeded = 0; }    // Avoid needed blade below zero
    }



}
