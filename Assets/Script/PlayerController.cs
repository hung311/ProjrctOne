using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Image healthBar;
    public float maxHp = 100f;
    public float HpPlayer = 100f;

    public float moveSpeed = 1f;
    public float collisionOffset = 0.01f;
    public ContactFilter2D movementFilter;
    public SwordAttack swordAttack;
    AudioManager audioManager;
    //public Joystick joystick;

    bool canMove = true;

    Vector2 movementInput;
    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRenderer;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    public float HealthPlayer
    {
        set 
        {
            HpPlayer = Mathf.Clamp(value, 0, maxHp);
            UpdateHealthUI();
            //HpPlayer = value;
            if (HpPlayer <= 0 )
            {
                animator.SetTrigger("death");
                audioManager.PlaySFXOneShot(audioManager.death);    
            }
        }
        get { return HpPlayer; }
    }

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        LoadPlayer();
    }

    void Update()
    {
        /*if (joystick != null)
        {
            movementInput = new Vector2(joystick.Horizontal, joystick.Vertical);
        }*/
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            if (movementInput != Vector2.zero)
            {
                if (movementInput.magnitude > 1)
                {
                    movementInput = movementInput.normalized;
                }

                bool success = TryMove(movementInput);
                if (!success)
                {
                    success = TryMove(new Vector2(movementInput.x, 0));
                }
                if (!success)
                {
                    success = TryMove(new Vector2(0, movementInput.y));
                }
                animator.SetBool("isMoving", success);

            }
            else
            {
                animator.SetBool("isMoving", false);
            }

            if (movementInput.x > 0)
            {
                spriteRenderer.flipX = false;
            }
            else if (movementInput.x < 0)
            {
                spriteRenderer.flipX = true;
            }
        }
    }

    private bool TryMove(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            int count = rb.Cast(
            direction,
            movementFilter,
            castCollisions,
            moveSpeed * Time.fixedDeltaTime + collisionOffset);

            if (count == 0)
            {
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    void OnFire(InputValue fireValue)
    {
        animator.SetTrigger("swordAttack");
    }

    /*public void OnSwordAttackClick()
    {
        if (animator != null)
        {
            animator.SetTrigger("swordAttack");
        }
    }*/

    public void SwordAttack()
    {
        LockMovement();
        if (spriteRenderer.flipX == true)
        {
            swordAttack.AttackLeft();
        }
        else
        {
            swordAttack.AttackRight();
        }
    }
    public void EndSwordAttack()
    {
        UnlockMovement();
        swordAttack.StopAttack();
    }

    private void LockMovement()
    {
        canMove = false;

    }

    private void UnlockMovement()
    {
        canMove = true;
    }

    private void UpdateHealthUI()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = HpPlayer / maxHp;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Slime")
        {
            LockMovement();
            animator.SetBool("isMoving", false);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Slime")
        {
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(WaitAndUnlockMovement(0.5f));
            }
        }
    }

    private IEnumerator WaitAndUnlockMovement(float delay)
    {
        yield return new WaitForSeconds(delay);
        UnlockMovement();
    }

    public void Die()
    {
        SceneManager.LoadScene("End");
    }

    private void LoadPlayer() 
    {
        if (PlayerPrefs.HasKey("PlayerX") && PlayerPrefs.HasKey("PlayerY"))
        {
            float playerX = PlayerPrefs.GetFloat("PlayerX");
            float playerY = PlayerPrefs.GetFloat("PlayerY");
            float playerZ = PlayerPrefs.GetFloat("PlayerZ");

            float Hp = PlayerPrefs.GetFloat("HpPlayer");
            HpPlayer = Hp;
            UpdateHealthUI();

            transform.position = new Vector3(playerX, playerY, playerZ);
        }
    }
}
