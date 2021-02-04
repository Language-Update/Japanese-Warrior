using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour{


    void Start()    {
    }



    void Update()    {
        transform.Translate(Vector2.right * 2f * Time.deltaTime);        
    }


    private void OnTriggerEnter2D(Collider2D collision) {
        GetComponent<AudioSource>().Play();
        Destroy(collision.gameObject);
        transform.position = new Vector2(transform.position.x, 100);
        Destroy(this.gameObject, 0.3f);
    }
}
