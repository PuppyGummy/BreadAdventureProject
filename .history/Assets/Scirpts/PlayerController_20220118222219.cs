using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    public Animator anim;
    public Text coinNum;
    public GameObject startNote;
    public GameObject exitNote;


    public float speed;
    public float jumpforce;
    public float wallSlidingSpeed;
    private float gravityStore;
    private int coin;

    public Transform groundCheckPoint;
    public Transform wallGrabPoint;
    public Transform frontCheck;
    public LayerMask whatIsGround;
    public LayerMask whatIsWall;

    private bool isGround;
    private bool isWall;
    private bool isGrabbing;
    private bool isDead;
    private bool isTouchingFront;
    private bool wallSliding;

    bool jumpPressed;
    int jumpCount;


    public float wallJumpTime = .2f;
    private float wallJumpCounter;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gravityStore = rb.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && rb.velocity.y == 0)
        {
            jumpPressed = true;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            Invoke("Reload", 0.5f);
        }
    }

    private void FixedUpdate()
    {
        isGround = Physics2D.OverlapCircle(groundCheckPoint.position, 0.1f, whatIsGround);
        if (!isDead)
        {
            Movement();
        }
        switchAnim();
        isTouchingFront = Physics2D.OverlapCircle(frontCheck.position, 0.1f, whatIsGround);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Spikes")
        {
            isDead = true;
            SoundController.instance.HurtAudio();

            Invoke("Reload", 0.5f);
        }
        else if (collision.gameObject.tag == "Collection")
        {
            SoundController.instance.CollectAudio();
            Destroy(collision.gameObject);
            coin++;
            coinNum.text = coin.ToString();
        }
        else if (collision.gameObject.tag == "Enter")
        {
            startNote.SetActive(true);

        }
        else if (collision.gameObject.tag == "Exit")
        {
            exitNote.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enter")
        {
            startNote.SetActive(false);
        }
        else if (collision.gameObject.tag == "Exit")
        {
            exitNote.SetActive(false);
        }
    }

    void switchAnim()
    {
        anim.SetBool("idle", false);
        if (anim.GetBool("jumping"))
        {
            if (rb.velocity.y < 0)
            {
                anim.SetBool("jumping", false);
                anim.SetBool("falling", true);
            }
        }
        else if (isGround)
        {
            //SoundController.instance.LandAudio();
            anim.SetBool("falling", false);
            anim.SetBool("idle", true);
        }
        else if (isDead)
        {
            anim.SetBool("dead", true);
        }
    }

    void Reload()
    {
        gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
