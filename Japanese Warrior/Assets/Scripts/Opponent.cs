using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opponent : MonoBehaviour {

    [SerializeField] Projectile projectile = null;
    [SerializeField] GameObject gunBarrel = null;
    [SerializeField] int damagePoint = 100;

    [SerializeField] [Range(1, 5)] float minFireTime = 0;
    [SerializeField] [Range(3, 8)] float maxFireTime = 0;

    Animator animator = null;

    bool fire, fireCompleted;
    int bladeNeeded;
    float timePassed = 0; float timeNeeded;


    void Start() {
        animator = GetComponentInChildren<Animator>();
        bladeNeeded = 10;
        fireCompleted = true;
        timeNeeded = Random.Range(minFireTime + 1, maxFireTime);
    }


    void Update() {
        timePassed += Time.deltaTime;        

        if (timePassed > timeNeeded) {
            animator.SetBool("hasBlade", true); // Equip Blade
            if (bladeNeeded > 0 && fireCompleted) {  // And if there an enemy
                fire = true; fireCompleted = false;
                
                // Get a new random needed time
                timeNeeded = Random.Range(minFireTime, maxFireTime);
                timePassed = 0; // Reset timer
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

    private IEnumerator Fire() {
        yield return new WaitForSeconds(0.4f);

        Projectile newProjectile = Instantiate(projectile, gunBarrel.transform.position, transform.rotation) as Projectile;
        newProjectile.transform.parent = transform;
        newProjectile.gameObject.layer = 11;
        FindObjectOfType<AudioManager>().Play("Fire");

        fireCompleted = true;

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        collision.GetComponent<Health>().DealDamage(damagePoint);
    }

    public void PerformDeath() {
        FindObjectOfType<AudioManager>().Play("DyingMan");   // Play dead sound
        transform.position = new Vector2(-100, -100);   // Send player out of range to simulate death
        FindObjectOfType<GameHandler>().SetCharacterStatus(true); // Let the GH know player is dead now
    }
}
