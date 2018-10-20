using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float coinRange;
    public float coinSpeed;
    bool invincible = false;
    public GameObject bag;
    //define maxspeed as 1.5 (used to control players movement speed)
    float maxSpeed = 1.5f;
    //define playerhp as 5 (used to control players health)
    public int playerHP = 100;
    //define facingright (used to control character texture flipping)
    bool facingRight;
    //define death as true (used to control players death)
    bool death = true;
    //define public arrowtransform (used to set arrows spawn position)
    public GameObject arrowTransform;
    //define public arrow (used to spawn arrows)
    public GameObject arrow;
    //define public bloodparticles (used to control blood particles when hurt)
    public GameObject bloodParticles;
    //define public healthparticles (used to control particles when picking up healing potions)
    public GameObject healthParticles;
    //define public powerparticles (used to control particles when picking up power potions)
    public GameObject powerParticles;
    //define public tutMovement (used to control movement tutorial sign)
    public RectTransform tutMovement;
    //define public tutPotions (used to control potion tutorial sign)
    public RectTransform tutPotions;
    //define public tutorialPotions (used to control tutorial potion gameobjects)
    public GameObject tutorialPotions;
    //define public coinText (used to control players visual coin counter)
    public Text coinText;
    //define public damageslider (used to control players visual charge/power slider)
    public Slider damageSlider;
    //define public powerSFX (used to play power sound effect)
    public AudioClip powerSFX;
    //define public healthSFX (used to play health sound effect)
    public AudioClip coinSFX;
    public AudioClip bagSFX;
    //define public gameovertext (used to control gameover text)
    public Text gameOverText;
    //define public GameController (used to enable enemy spawning and scoreboard after completing the tutorial)
    public GameObject GameContoller;

    public GameObject coin;

    //define audiosouce audiosrc
    AudioSource audioSrc;
    //define rigidbody rb2d
    Rigidbody2D rb2d;
    //define animator anim
    Animator anim;
    //define vector3 movement
    Vector3 movement;
    //define shooting as false (used to control shooting)
    bool shooting = false;

    void Start()
    {
        StartCoroutine(tutorial());

        //set arrows charge value to 0
        arrow.GetComponent<ArrowController>().charge = 0;

        //set facing right value as true (by default player character is facing right)
        facingRight = true;

        //get audiosource as audioSrc
        audioSrc = GetComponent<AudioSource>();
        //get rigidbody2D as rb2d
        rb2d = GetComponent<Rigidbody2D>();
        //get animator as anim
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(0);
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(1);
    }

    void FixedUpdate()
    {
        //moves the particle generator accordingly
        Vector2 particlePosition = new Vector2(gameObject.transform.position.x, gameObject.transform.position.y - 0.2f);
        //sets particle effects position to the position of the generator
        powerParticles.transform.position = particlePosition;
        healthParticles.transform.position = particlePosition;

        //get input values
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveY = Input.GetAxisRaw("Vertical");

        //sets hp and power values to the slider UI
        damageSlider.value = arrow.GetComponent<ArrowController>().charge;
        coinText.text = "Coins: " + playerHP;

        if (!gameOverText.enabled)
        {
            //move character
            Move(moveX, moveY);
            //flip character
            Flip(moveX);
            //animate character
            Animate(moveX, moveY);
        }

        //cheatcodes
        if (Input.GetKeyDown(KeyCode.G) && Input.GetKeyDown(KeyCode.M) && Input.GetKeyDown(KeyCode.I) && Input.GetKeyDown(KeyCode.P))
        {
            //give me infinite power
            InvokeRepeating("gmip", 0, 0.1f);
        }
        else if (Input.GetKeyDown(KeyCode.I) && Input.GetKeyDown(KeyCode.W) && Input.GetKeyDown(KeyCode.D))
        {
            //i wont die
            InvokeRepeating("iwd", 0, 0.1f);
        }
        else if (Input.GetKeyDown(KeyCode.K) && Input.GetKeyDown(KeyCode.A) && Input.GetKeyDown(KeyCode.E))
        {
            //kill all enemies
            Invoke("kae", 0);
        }
    }

    // Potion pickup.
    void OnTriggerEnter2D(Collider2D _other)
    {
        //layer character
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y + 2);
        //if hit gameobject is tagged health potion
        if (_other.tag == "Coin")
        {
            //destroy the gameobject
            Destroy(_other.gameObject);
            //add 2 to the player hp value
            playerHP += 1;
            //play sound effect
            Sound(1);

            //enable particle emission
            GameObject ParticleFX = Instantiate (healthParticles);
            //keep particle emitter at the position of the player
            ParticleFX.transform.parent = gameObject.transform;
        }

        if (_other.tag == "CoinBag")
        {
            //destroy the gameobject
            Destroy(_other.gameObject);
            //add 2 to the player hp value
            playerHP += 10;
            //play sound effect
            Sound(3);

            //enable particle emission
            GameObject ParticleFX = Instantiate(healthParticles);
            //keep particle emitter at the position of the player
            ParticleFX.transform.parent = gameObject.transform;
        }

        //if hit gameobject is tagged damage potion
        if (_other.tag == "Damage Potion")
        {
            //destroy the gameobject
            Destroy(_other.gameObject);
            //add 5 to the charged arrows value
            arrow.GetComponent<ArrowController>().charge += 5;
            //if charged arrows value is more than 5 (the player hasnt used rest of the charge arrows)
            if (arrow.GetComponent<ArrowController>().charge >= 5)
                //set charged arrows amount to 5 (preventing stacking of charged arrows)
                arrow.GetComponent<ArrowController>().charge = 5;
            //play sound effect
            Sound(2);

            //enable particle emission
            GameObject ParticleFX = Instantiate(powerParticles);
            //keep particle emitter at the position of the player
            ParticleFX.transform.parent = gameObject.transform;
        }
    }


    // Movement
    void Move(float _moveX, float _moveY)
    {
        movement.Set(_moveX, _moveY, 0);
        movement = movement.normalized * maxSpeed * Time.deltaTime;
        rb2d.MovePosition(transform.position + movement);
    }

    // Shooting
    void Shoot()
    {
        //spawn arrow to the arrowtransform position with the same rotation as the arrowtransform
        Instantiate(arrow, arrowTransform.transform.position, arrowTransform.transform.rotation);

        //decrease arrows charge value by 1 (so that the player can't use charged arrows indefientely)
        arrow.GetComponent<ArrowController>().charge -= 1;
        //if arrows charge value is less than 0
        if (arrow.GetComponent<ArrowController>().charge < 0)
            //set arrows charge value to 0
            arrow.GetComponent<ArrowController>().charge = 0;
    }

    // Flipping
    void Flip(float _moveX)
    {
        //if player moveX value is more than 0 (player moving right) and character is not facing right
        if (_moveX > 0 && !facingRight)
        {
            //set facing right value as the opposite of itself
            facingRight = !facingRight;
            //set sprite flipX to false (flipping the texture of the player character)
            GetComponent<SpriteRenderer>().flipX = false;

            //rotate the arrow spawn-point.
            arrowTransform.transform.Rotate(Vector3.forward, 180);
        }

        //if player moveX value is less than 0 (player moving left) and character is facing right
        else if (_moveX < 0 && facingRight)
        {
            //set facing right value as the opposite of itself
            facingRight = !facingRight;
            //set sprite flipX to true (flipping the texture of the player character)
            GetComponent<SpriteRenderer>().flipX = true;

            //rotate the arrow spawn-point.
            arrowTransform.transform.Rotate(Vector3.forward, -180);
        }
    }

    // Damage
    public void TakeDamage(int _damage)
    {
        //layer character
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.y + 2);
        //generate blood particles
        Instantiate(bloodParticles, transform.position, bloodParticles.transform.rotation);
        //decrease players HP by the damage dealt
        if (playerHP == 0 && !invincible) { Death(); }

        //drop money
        for (int i = playerHP; i > 0; i--)
        {
            if (i > 10 && Random.Range(1,5) < 4)
            {
                GameObject Bag = Instantiate(bag, transform.position, Quaternion.identity);
                Bag.GetComponent<Rigidbody2D>().velocity = Bag.transform.TransformDirection(Random.Range(-coinRange, coinRange), coinSpeed, 0);
                Bag.GetComponent<CoinController>().playerY = transform.position.y;
                i -= 9;
            }
            else
            {
                GameObject Coin = Instantiate(coin, transform.position, Quaternion.identity);
                Coin.GetComponent<Rigidbody2D>().velocity = Coin.transform.TransformDirection(Random.Range(-coinRange, coinRange), coinSpeed, 0);
                Coin.GetComponent<CoinController>().playerY = transform.position.y;
            }
        }
        if (!gameOverText.enabled)
        {
            invincible = true;
            StartCoroutine(Invincible());
            InvokeRepeating("flash", 0, 0.08f);
            playerHP = 0;
        }
    }

    IEnumerator Invincible()
    {
        yield return new WaitForSeconds(2);
        invincible = false;
        CancelInvoke("flash");
        GetComponent<SpriteRenderer>().enabled = true;
    }

    void flash()
    {
        GetComponent<SpriteRenderer>().enabled = !GetComponent<SpriteRenderer>().enabled;
    }


    // Animation
    void Animate(float _moveX, float _moveY)
    {
        // Moving Animation
        bool moving = _moveX != 0f || _moveY != 0f;
        anim.SetBool("IsMoving", moving);

        // Shooting animation
        anim.SetBool("IsShooting", shooting);

        //if key Z is being pressed
        if (Input.GetKey(KeyCode.Z))
            //set shooting true
            shooting = true;
        else
            //set shooting false
            shooting = false;
    }

    // Sound effects.
    void Sound (int _clip)
    {
        //if clip 1 is called to be played (health sound)
        if (_clip == 1)
        {
            //set audiosource clip to health sound effect
            audioSrc.clip = coinSFX;
            //play sound effect
            audioSrc.Play();
        }

        //if clip 1 is called to be played (powerup sound)
        else if (_clip == 2)
        {
            //set audiosource clip to power sound effect
            audioSrc.clip = powerSFX;
            //play sound effect
            audioSrc.Play();
        }

        else if (_clip == 3)
        {
            //set audiosource clip to power sound effect
            audioSrc.clip = bagSFX;
            //play sound effect
            audioSrc.Play();
        }
    }

    // Dying 
    void Death()
    {
        //enable gameover text
        gameOverText.enabled = true;

        //activate death animation
        anim.SetBool("IsDead", death);

        //set players collider as trigger (so player can fly out of frame)
        gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
        //apply upward velocity
        rb2d.velocity = transform.up * maxSpeed / 2;
        //activate camera black and white filter
        GameObject.Find("Camera").GetComponent<CameraController>().BW();
    }

    //Tutorial
    IEnumerator tutorial()
    {
        StartCoroutine(MoveSign(tutMovement, true));
        while (!shooting)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1);
        StartCoroutine(MoveSign(tutMovement, false));
        StartCoroutine(MoveSign(tutPotions, true));
        tutorialPotions.SetActive(true);
        while (arrow.GetComponent<ArrowController>().charge != 5)
        {
            yield return null;
        }
        while (playerHP < 3)
        {
            yield return null;
        }
        arrow.GetComponent<ArrowController>().charge = 0;
        StartCoroutine(MoveSign(tutPotions, false));
        GameContoller.SetActive(true);
        yield return null;
    }

    //moves the specified RectTransform sign down (true) or up (false)
    IEnumerator MoveSign(RectTransform sign, bool direction)
    {
        yield return new WaitForSeconds(1);
        float elapsedTime = 0;
        while (elapsedTime < 1)
        {
            if (direction == true)
            {
                sign.anchoredPosition = new Vector3(0, Mathf.Lerp(800, 500, (elapsedTime / 1)), 0);
            }
            else
            {
                sign.anchoredPosition = new Vector3(0, Mathf.Lerp(500, 800, (elapsedTime / 1)), 0);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(1);
        //waits an extra second to prevent signs from overlapping
        if(direction == false)
        {
            yield return new WaitForSeconds(1);
        }
    }

    //cheatcode functions

    void gmip()
    {
        arrow.GetComponent<ArrowController>().charge = 5;
    }
    void iwd()
    {
        playerHP = 10;
    }
    void kae()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = enemies.Length - 1; i >= 0; i--)
        {
            Destroy(enemies[i].gameObject);
        }
    }
}