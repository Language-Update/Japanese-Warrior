using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour{

    [SerializeField] Projectile projectile;
    [SerializeField] GameObject gunBarrel;

    bool fire;

    void Start()    {
        
    }


    void Update()    {
        if (fire) {
            Projectile newProjectile = Instantiate(projectile, gunBarrel.transform.position, transform.rotation) as Projectile;
            newProjectile.transform.parent = transform;
            GetComponent<AudioSource>().Play();

            fire = false;
        }
    }

    public void SetFire(bool fire) {
        this.fire = fire;
    }

    private void Fire() {
    }

    /*private IEnumerator Fire() {
        fire = false;

        yield return new WaitForSeconds(1);
    }*/



}
