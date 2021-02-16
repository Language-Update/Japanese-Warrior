using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour{

    [SerializeField] int damagePoint = 0;
    [SerializeField] bool opponent = false;

    void Update()    {
        if (!opponent)
            transform.Translate(Vector2.right * 2f * Time.deltaTime);
        else
            transform.Translate(Vector2.left * 2f * Time.deltaTime);
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        collision.GetComponent<Health>().DealDamage(damagePoint);
    }

    public void PerformDeath() { Destroy(gameObject); }
}
