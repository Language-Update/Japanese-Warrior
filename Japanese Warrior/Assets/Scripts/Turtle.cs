using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : MonoBehaviour{

    [SerializeField] [Range(0.1f, 2f)] float speed;

    void Start()    {
        
    }

    void Update()    {
        transform.Translate(Vector2.left * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            FindObjectOfType<Character>().Killed();
            Destroy(collision.gameObject, 2);
            Destroy(this.gameObject);
        }
    }
}
