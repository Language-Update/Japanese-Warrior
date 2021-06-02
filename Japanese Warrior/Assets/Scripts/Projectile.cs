using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour{

    [SerializeField] int damagePoint = 0;
    public bool opponent = false;

    void Update()    {
        transform.Translate(Vector2.right * 2f * Time.deltaTime);
    }


    private void OnTriggerEnter2D(Collider2D collider) {
        collider.GetComponent<Health>().DealDamage(damagePoint);
        Destroy(gameObject);
    }
}
