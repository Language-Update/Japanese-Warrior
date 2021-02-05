using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour{

    [SerializeField] Projectile projectile;
    [SerializeField] GameObject gunBarrel;
    [SerializeField] AudioSource fireSound;
    [SerializeField] AudioSource deadSound;

    bool fire;

    void Start()    {
        
    }


    void Update()    {
        if (fire) {
            Projectile newProjectile = Instantiate(projectile, gunBarrel.transform.position, transform.rotation) as Projectile;
            newProjectile.transform.parent = transform;
            fireSound.Play();

            fire = false;
        }
    }

    public void SetFire(bool fire) {
        this.fire = fire;
    }

    public void Killed() {
        deadSound.Play();   // Play dead sound
        transform.position = new Vector2(-100, -100);   // Send player out of range to simulate death
        FindObjectOfType<GameHandler>().SetPlayerStatus(true); // Let the GH know player is dead now
    }

    /*private IEnumerator Fire() {
        fire = false;

        yield return new WaitForSeconds(1);
    }*/



}
