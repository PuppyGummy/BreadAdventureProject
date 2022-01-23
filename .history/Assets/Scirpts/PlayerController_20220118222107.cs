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
        isTouchingFront = Physics2D.overlap
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
    void Movement()
    {
        if (wallJumpCounter <= 0)
        {
            //角色移动
            float horizontalmove = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(horizontalmove * speed * Time.deltaTime, rb.velocity.y);
            anim.SetFloat("walking", Mathf.Abs(horizontalmove));
            //SoundController.instance.MoveAudio();

            if (horizontalmove != 0)
            {
                transform.localScale = new Vector3(horizontalmove, 1, 1);
            }

            //角色跳跃
            if (jumpPressed)
            {
                SoundController.instance.JumpAudio();
                rb.velocity = new Vector2(rb.velocity.x, jumpforce * Time.deltaTime);
                anim.SetBool("jumping", true);
                jumpPressed = false;
            }

            //角色蹬墙反跳
            isWall = Physics2D.OverlapCircle(wallGrabPoint.position, 0.1f, whatIsWall);
            //isGrabbing = false;

            if (isWall && !isGround)
            {
                isGrabbing = true;
            }
            else
            {
                isGrabbing = false;
            }

            if (isGrabbing == true)
            {
                //rb.gravityScale = 5f;
                //rb.gravityScale = rb.gravityScale/5;
                if (horizontalmove != 0)
                {
                    rb.velocity = Vector2.zero;
                    rb.gravityScale = gravityStore;
                }
                if (Input.GetButtonDown("Jump"))
                {
                    if (transform.localScale.x == 1f)
                    {
                        //rb.gravityScale = 0f;
                        rb.gravityScale = gravityStore;
                        wallJumpCounter = wallJumpTime;
                        transform.localScale = new Vector3(-1, 1, 1);
                        rb.velocity = new Vector2(-1.5f * speed * Time.deltaTime, jumpforce * Time.deltaTime);
                        //isGrabbing = false;
                    }
                    else
                    {
                        //rb.gravityScale = 0f;
                        rb.gravityScale = gravityStore;
                        wallJumpCounter = wallJumpTime;
                        transform.localScale = new Vector3(1, 1, 1);
                        rb.velocity = new Vector2(1.5f * speed * Time.deltaTime, jumpforce * Time.deltaTime);
                        //isGrabbing = false;
                    }
                }
            }
            else
            {
                rb.gravityScale = gravityStore;
            }
        }
        else
        {
            wallJumpCounter -= Time.deltaTime;
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
