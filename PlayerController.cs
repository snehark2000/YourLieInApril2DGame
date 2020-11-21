using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    private enum State {idle, walk, jump, fall, hurt};
    private State state;
    private Collider2D coll;
    [SerializeField] private LayerMask ground;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private Text butterflyText;
    [SerializeField] private int butterflies = 0;
    [SerializeField] private float hurtForce = 10f;
    [SerializeField] private AudioSource footstep;
    [SerializeField] private AudioSource cherry;
    [SerializeField] private Text lifeText;
    [SerializeField] private int life;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        lifeText.text = life.ToString();
    }
    void Update()
    { 
        if (state != State.hurt)
        {
            Movement();
        }
        StateSwitch();
        anim.SetInteger("state", (int)state);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Collectable")
        {
            cherry.Play();
            Destroy(collision.gameObject);
            butterflies += 1;
            butterflyText.text = butterflies.ToString();
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Enemy")
        {
            Enemy enemy = other.gameObject.GetComponent<Enemy>();
            if(state == State.fall)
            {
                enemy.JumpedOn();
                Jump();
            }
            else
            {
                state = State.hurt;
                HandleLife();
                if(other.transform.position.x > transform.position.x)
                {
                    rb.velocity = new Vector2(-hurtForce, transform.position.y);
                }
                else
                {
                    rb.velocity = new Vector2(hurtForce, transform.position.y);
                }
            }
        }
    }
    private void HandleLife()
    {
        life -= 1;
        lifeText.text = life.ToString();
        if(life <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    private void Movement()
    {
        float hdirection = Input.GetAxis("Horizontal");
        if (hdirection < 0)
        {
            rb.velocity = new Vector2(-speed, rb.velocity.y);
            transform.localScale = new Vector2(-1, 1);
        }
        else if (hdirection > 0)
        {
            rb.velocity = new Vector2(speed, rb.velocity.y);
            transform.localScale = new Vector2(1, 1);
        }
        if (Input.GetButtonDown("Jump") && coll.IsTouchingLayers(ground))
        {
            Jump();
        }
    }
    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        state = State.jump;
    }
    private void StateSwitch()
    {
        if (state == State.jump)
        {
            if(rb.velocity.y < .1f)
            {
                state = State.fall;
            }
        }
        else if(state == State.fall)
        {
            if (coll.IsTouchingLayers(ground))
            {
                state = State.idle;
            }
        }
        else if(state == State.hurt)
        {
            if(Mathf.Abs(rb.velocity.x) < .1f)
            {
                state = State.idle;
            }
        }
        else if(Mathf.Abs(rb.velocity.x) > 1f)
        {
            state = State.walk;
        }
        else
        {
            state = State.idle; 
        }
    }
    private void Footstep()
    {
        footstep.Play();
    }
}
