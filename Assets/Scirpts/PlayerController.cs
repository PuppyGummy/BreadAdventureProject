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
    public float jumpForce;
    public float wallSlidingSpeed;
    public float xWallForce;
    public float yWallForce;
    public float wallJumpTime;
    public float disableInputTime;
    public float groundedCheckRadius;
    public float slidingCheckRadius;
    private float horizontalMove;
    private int coin;

    public Transform groundCheckPoint;
    public Transform wallCheckPoint;
    public LayerMask groundLayer;

    private bool isGrounded;
    private bool isDead;
    private bool isTouchingWall;
    private bool isWallSliding;
    private bool isWallJumping;
    private bool enableInput;

    bool jumpPressed;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enableInput = true;
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

        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundedCheckRadius, groundLayer);

        isTouchingWall = Physics2D.OverlapCircle(wallCheckPoint.position, slidingCheckRadius, groundLayer);

        if (isTouchingWall && horizontalMove != 0 && !isGrounded)
        {
            isWallSliding = true;
        }
        else
        {
            isWallSliding = false;
        }

        if (!isDead)
        {
            PlayerMove();
        }
        switchAnim();
    }

    void PlayerMove()
    {
        if (enableInput)
        {
            horizontalMove = Input.GetAxisRaw("Horizontal");
            rb.velocity = new Vector2(horizontalMove * speed, rb.velocity.y);
            if (horizontalMove != 0)
            {
                transform.localScale = new Vector3(horizontalMove, 1, 1);
            }
        }
            
        anim.SetFloat("walking", Mathf.Abs(horizontalMove));



        if (jumpPressed)
        {
            SoundController.instance.JumpAudio();
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            anim.SetBool("jumping", true);
            jumpPressed = false;
        }

        if (isWallSliding)
        {
            
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlidingSpeed, float.MaxValue));
            if (Input.GetButtonDown("Jump"))
            {
                SoundController.instance.WallJumpAudio();

                isWallJumping = true;
                enableInput = false;
                Invoke("SetWallJumpingToFalse", wallJumpTime);
                Invoke("EnableInput", disableInputTime);
                transform.localScale = new Vector3(-transform.localScale.x, 1, 1);
            }
        }
        
        if (isWallJumping)
        {
            rb.velocity = new Vector2(xWallForce * -horizontalMove, yWallForce);
        }
    }

    private void SetWallJumpingToFalse()
    {
        isWallJumping = false;
    }

    private void EnableInput()
    {
        enableInput = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Spikes")
        {
            isDead = true;
            SoundController.instance.HurtAudio();
            rb.velocity = new Vector2(0, rb.velocity.y);

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
        else if (isGrounded)
        {
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
