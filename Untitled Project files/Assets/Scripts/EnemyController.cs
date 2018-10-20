using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour
{
    public GameObject coin;
    public GameObject damagePotion;
    public GameObject gameController;
    public GameObject bloodParticles;
    public GameObject player;
    public int enemyHP;
    public int damage;
    public int enemyScore;
    public bool moveRight = true;
    public float speed;

    bool enter = true;
    Rigidbody2D rb2d;



    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y + 2);
    }


    void FixedUpdate()
    {
        Move();
    }

    // Checks for collision
    void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.tag == "Player")
        {
            DoDamage(_other);
        }

        if (_other.tag == "Border")
        {
            if (enter == false)
            {
                moveRight = !moveRight;
                GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
                StartCoroutine(PositionChange());
            }
            else
                enter = false;
        }
    }

    //randomizes a new y position for the zombie
    IEnumerator PositionChange()
    {
        float rndPosY = Random.Range(-1.3f, -0.6f);
        float elapsedTime = 0;
        while (elapsedTime < 2)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Lerp(transform.position.y, rndPosY, (elapsedTime / 2)), transform.position.y+2);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    // Calls the TakeDamage function from PlayerController.
    void DoDamage(Collider2D _other)
    {
        _other.gameObject.SendMessage("TakeDamage", damage);
    }

    public void TakeDamage(int _damage)
    {
        enemyHP -= _damage;
        Instantiate(bloodParticles, transform.position, bloodParticles.transform.rotation);

        if (enemyHP <= 0)
        {
            GameController.score += enemyScore;
            Death();
        }
    }

    // Movement left/right
    void Move()
    {
        if (moveRight == false)
            rb2d.velocity = -transform.right * speed;

        else
            rb2d.velocity = transform.right * speed;
    }

    // Death and drops.
    void Death()
    {
        // Big enemy drop
        if (gameObject.name == "Big Enemy Right(Clone)" || gameObject.name == "Big Enemy Left(Clone)")
        {
            int rndDropBig = Random.Range(1, 100);

            if (rndDropBig <= 35)
            {
                int rndPotion = Random.Range(1, 100);

                if (rndPotion <= 80)
                {
                    Vector2 position = new Vector2(transform.position.x, transform.position.y - 0.2f);
                    Instantiate(coin, position, gameObject.transform.rotation);
                    Destroy(gameObject);
                }

                else
                {
                    Vector2 position = new Vector2(transform.position.x, transform.position.y - 0.2f);
                    Instantiate(damagePotion, position, gameObject.transform.rotation);
                    Destroy(gameObject);
                }
            }
            else
                Destroy(gameObject);
        }


        if (gameObject.name == "Small Enemy Right(Clone)" || gameObject.name == "Small Enemy Left(Clone)")
        {
            int rndDropSmall = Random.Range(1, 100);

            if (rndDropSmall <= 15)
            {
                int rndPotion = Random.Range(1, 100);

                if (rndPotion <= 72)
                {
                    Vector3 position = new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.y + 0.8f);
                    Instantiate(coin, position, gameObject.transform.rotation);
                    Destroy(gameObject);
                }

                else
                {
                    Vector3 position = new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.y + 0.8f);
                    Instantiate(damagePotion, position, gameObject.transform.rotation);
                    Destroy(gameObject);
                }
            }

            else
                Destroy(gameObject);
        }
    }
}