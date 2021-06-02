using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : MonoBehaviour{

    [SerializeField] int damagePoint = 0;
    [Range(0.10f, 0.5f)] public float speedReference = 0;

    public float speed;

    void Update()    {
        transform.Translate(Vector2.left * Mathf.Min(3f, speed) * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision) {   
        // if Character gets damage, then no need to throw blade, it's too late.
        if (collision.CompareTag("Player")) {
            FindObjectOfType<Character>().ChangeBladeNeeded(-(gameObject.GetComponent<Health>().GetHealth()) / 10);
            collision.GetComponent<Health>().DealDamage(damagePoint);
        }
    }

    public void PerformDeath() {
        FindObjectOfType<AudioManager>().Play("EnemyDeath");
        FindObjectOfType<GameHandler>().ChangeEnemyNumber(-1);
        Destroy(gameObject); 
    }
}
