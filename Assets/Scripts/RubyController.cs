using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class RubyController : MonoBehaviour
{
    public float speed = 3.0f;


    public int maxHealth = 5;
    public float timeInvincible = 2.0f;
    public float timeSlow = 2.0f;



    public int health { get { return currentHealth; } }
    int currentHealth;


    public int scoreAmount = 0;
    public static int totalRobots = 4;
    public int cogs = 4;
    public int waterAmount = 0;
    public int waterTotal = 5;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI cogsText;
    public TextMeshProUGUI waterText;
    public GameObject winText;
    public GameObject winTextTwo;
    public GameObject loseText;


    bool isInvincible;
    float invincibleTimer;
    bool isSlow;
    float slowTimer;


    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;


    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);


    public GameObject projectilePrefab;
    public GameObject healthParticle;
    public GameObject damageParticle;


    AudioSource audioSource;
    public AudioClip cogClip;
    public AudioClip hitClip;
    public AudioClip winClip;
    public AudioClip loseClip;
    public AudioClip bgMusic;
    public AudioClip collectedClip;
    public AudioClip ammoClip;
    public AudioClip talkClip;
    public AudioClip spiderClip;


    bool gameOver = false;


    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentHealth = maxHealth;

        audioSource = GetComponent<AudioSource>();
        audioSource.clip = bgMusic;
        audioSource.Play();
        audioSource.loop = true;

        scoreText.text = "Fixed Robots: " + scoreAmount + "/" + totalRobots;
        cogsText.text = "Cogs: " + cogs;
        waterText.text = "Water: " + waterAmount.ToString() + "/" + waterTotal;

        cogs = 4;

        winText.SetActive(false);
        winTextTwo.SetActive(false);
        loseText.SetActive(false);

        isInvincible = false;
        isSlow = false;

        Scene currentScene = SceneManager.GetActiveScene();
        string sceneName = currentScene.name;


        waterText.enabled = false;

        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("SceneTwo"))
        {
            waterText.enabled = true;
            waterAmount = 0;
        }
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    // Update is called once per frame
    void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        if (gameOver)
        {
            isInvincible = true;
        }

        if (isSlow)
        {
            slowTimer += Time.deltaTime;
            if (slowTimer < 0)
                isSlow = false;
        }

        if (Input.GetKeyDown(KeyCode.C) && cogs >= 1)
        {
            Launch();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                if (hit.collider != null)
                {
                    NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                    if (scoreAmount == 4 && character != null)
                    {
                        SceneManager.LoadScene("SceneTwo");
                    }
                    else if (scoreAmount < 4 && character != null)
                    {
                        character.DisplayDialog();
                        PlaySound(talkClip);
                    }
                }
            }
        }

        if (Input.GetKey(KeyCode.R))
        {
            if (gameOver == true)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // this loads the currently active scene
            }
        }
    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            animator.SetTrigger("Hit");
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;

            PlaySound(hitClip);
            Instantiate(damageParticle, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        }
        else if (amount > 0)
        {
            Instantiate(healthParticle, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        if (currentHealth == 0)
        {
            loseText.SetActive(true);
            gameOver = true;
            speed = 0;
            audioSource.loop = false;
            audioSource.Stop();
            audioSource.clip = loseClip;
            audioSource.Play();
        }

        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }

    public void WaterScore(int amount)
    {
        waterAmount = waterAmount + amount;
        waterText.text = "Water: " + waterAmount + "/" + waterTotal;

        if (scoreAmount == 4 && waterAmount == 5 && SceneManager.GetActiveScene() == SceneManager.GetSceneByName("SceneTwo"))
            {
                winTextTwo.SetActive(true);
                winText.SetActive(false);
                speed = 0;
                gameOver = true;

                audioSource.loop = false;
                audioSource.Stop();
                audioSource.clip = winClip;
                audioSource.Play();
            }
    }

    public void ChangeScore(int amount)
    {
        scoreAmount = scoreAmount + amount;
        scoreText.text = "Fixed Robots: " + scoreAmount + "/" + totalRobots;
        if (scoreAmount == 4 && SceneManager.GetActiveScene() == SceneManager.GetSceneByName("Main"))
        {
            winText.SetActive(true);
            gameOver = true;

            if (scoreAmount == 4 && waterAmount == 5 && SceneManager.GetActiveScene() == SceneManager.GetSceneByName("SceneTwo"))
            {
                winTextTwo.SetActive(true);
                winText.SetActive(false);
                speed = 0;
                gameOver = true;

                audioSource.loop = false;
                audioSource.Stop();
                audioSource.clip = winClip;
                audioSource.Play();
            }
        }
    }

    public void ChangeSpeed(int speed)
    {
        if (speed < 4)
        {
            speed = 2;
            animator.SetTrigger("Hit");

            if (isSlow)
                return;
            
            isSlow = true;
            slowTimer = timeSlow;

            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
            PlaySound(hitClip);
            Instantiate(damageParticle, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        }
    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
        PlaySound(cogClip);

        cogs = cogs - 1;
        cogsText.text = "Cogs: " + cogs;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ammo")
        {
            cogs = cogs + 4;
            cogsText.text = "Cogs: " + cogs;
            PlaySound(ammoClip);

            Destroy(collision.collider.gameObject);
        }

        //if (collision.collider.tag == "Potion")
        //{
            //waterAmount = waterAmount + 1;
            //waterText.text = "Water: " + waterAmount.ToString() + "/" + waterTotal;
            //PlaySound(bubbleClip);

            //Destroy(collision.collider.gameObject);
        //}
    }
}
