using UnityEngine;
using System.Collections;

public class ArrowController : MonoBehaviour 
{
    public float speed;
    public int damage;
    public int charge;
    public GameObject arrowParticles;
    public GameObject poweredArrowParticles;
    GameObject particleGO;

    int extraDamage;
    Rigidbody2D rb2d;


	void Start () 
    {
        transform.position = new Vector3(transform.position.x, transform.position.y ,transform.position.y + 2);
        if (charge > 0)
        {
            extraDamage = 1;
            particleGO = Instantiate(poweredArrowParticles, transform.position, arrowParticles.transform.rotation);
        }
        else
        {
            extraDamage = 0;
            particleGO = Instantiate(arrowParticles, transform.position, arrowParticles.transform.rotation);
        }

        particleGO.transform.parent = transform;

        rb2d = GetComponent<Rigidbody2D>();
	}


	void FixedUpdate () 
    {
        rb2d.velocity = transform.right * speed;
	}

    // Checks for enemy.
    void OnTriggerEnter2D (Collider2D _other)
    {
        if (_other.tag == "Enemy")
        {
            particleGO.transform.parent = null;
            particleGO.GetComponent<Destroy>().enabled = true;
            DoDamage(_other);
        }
    }

    // Calls TakeDamage function from EnemyController. 
    void DoDamage(Collider2D _other)
    {
        Destroy(gameObject);
        _other.gameObject.SendMessage("TakeDamage", damage + extraDamage);
        rb2d.velocity = transform.up * speed;
    }


    void OnBecameInvisible()
    {
        particleGO.transform.parent = null;
        particleGO.GetComponent<Destroy>().enabled = true;
        Destroy(gameObject);
    }
}
