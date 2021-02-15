using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : MonoBehaviour{

    [SerializeField] int damagePoint = 0;

    public float speed;

    void Start()    {
        
    }

    void Update()    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        collision.GetComponent<Health>().DealDamage(damagePoint);
        /*
        if (collision.CompareTag("Player")) {
            FindObjectOfType<Character>().PerformDeath();
            Destroy(collision.gameObject, 2);
            Destroy(this.gameObject);
        }
        */
    }

    public void PerformDeath() { Destroy(gameObject); }
}
