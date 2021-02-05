using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turtle : MonoBehaviour{


    void Start()    {
        
    }


    void Update()    {
        transform.Translate(Vector2.left * 1f * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player")) {
            FindObjectOfType<Player>().Killed();
            Destroy(collision.gameObject, 2);
            Destroy(this.gameObject);
        }
    }
}
