using UnityEngine;
using System.Collections;

public class Destroy : MonoBehaviour 
{
    public float destroyTime;

	void Start () 
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y+2);
        Destroy(gameObject, destroyTime);
        if (gameObject.tag == "Coin")
        {
            InvokeRepeating("flash", 1, 0.1f);
        }
        else if (destroyTime > 2)
        {
            InvokeRepeating("flash", destroyTime - 2, 0.1f);
        }
	}

    void flash()
    {
        GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;
    }
}
