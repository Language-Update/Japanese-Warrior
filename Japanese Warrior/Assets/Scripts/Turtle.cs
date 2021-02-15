using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : MonoBehaviour{

    [SerializeField] int damagePoint = 0;
    [Range(0.10f, 0.5f)] public float speedReference = 0;

    public float speed;

    void Start()    {
        
    }

    void Update()    {
        transform.Translate(Vector2.left * Mathf.Min(3f, speed) * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        collision.GetComponent<Health>().DealDamage(damagePoint);

        // if Character gets damage, then no need to throw blade, it's too late.
        if (collision.CompareTag("Player"))
            FindObjectOfType<Character>().ChangeBladeNeeded(-(gameObject.GetComponent<Health>().GetHealth()) / 10);
    }

    public void PerformDeath() {
        FindObjectOfType<GameHandler>().ChangeEnemyNumber(-1);
        Destroy(gameObject); 
    }
}
