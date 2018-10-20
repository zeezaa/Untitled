using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinController : MonoBehaviour {

    public float playerY;
	
	// Update is called once per frame
	void FixedUpdate () {
        if (GetComponent<Rigidbody2D>().velocity.y < 2 && transform.position.y < Random.Range(playerY-0.2f,playerY+0.1f))
        {
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            GetComponent<BoxCollider2D>().enabled = true;
        }
        else if (transform.position.x < -5 || transform.position.x > 5)
        {
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            GetComponent<Rigidbody2D>().angularVelocity = 0;
            GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX;
        }
    }
}
