using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;

public class SlimeConTroller : MonoBehaviour
{
    public float HpSlime = 10f;
    public Transform player;
    public float moveSpeed = 0.5f;
    public float collisionOffset = 0.01f;
    public ContactFilter2D movementFilter;
    public float radius = 1f;
    bool canMoveSlime = true;
    public SlimeAttck slimeAttck;
    public TextScore score;
    AudioManager audioManager;


    public Image healthBarFill;
    public float maxHp = 10f;

    Vector2 movementSlimeInput;
    Rigidbody2D rb;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    Animator animator;
    SpriteRenderer spriteRenderer;

    public float Health
    {
        set
        {
            HpSlime = Mathf.Clamp(value, 0, maxHp);
            UpdateHealthUI();
            //HpSlime = value;

            if (HpSlime <= 0)
            {
                Defeated();
            }
        }
        get { return HpSlime; }
    }

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void FixedUpdate()
    {
        if (canMoveSlime)
        {
            bool enemy = false;
            Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius);

            foreach (var hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Player"))
                {
                    enemy = true;

                    if (player != null)
                    {
                        movementSlimeInput = (player.position - transform.position).normalized;
                        bool success = TryMove(movementSlimeInput);
                        if (!success)
                        {
                            success = TryMove(new Vector2(movementSlimeInput.x, 0));
                        }
                        if (!success)
                        {
                            success = TryMove(new Vector2(0, movementSlimeInput.y));
                        }
                        animator.SetBool("isSlimeMoving", success);
                    }
                    else
                    {
                        animator.SetBool("isSlimeMoving", false);
                    }
                    if (movementSlimeInput.x > 0)
                    {
                        spriteRenderer.flipX = false;
                    }
                    else if (movementSlimeInput.x < 0)
                    {
                        spriteRenderer.flipX = true;
                    }

                    break;
                }
            }
            if (!enemy)
            {

                animator.SetBool("isSlimeMoving", false);
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
        return false;
    }

    public void Defeated()
    {
        audioManager.PlaySFXOneShot(audioManager.kill);
        animator.SetTrigger("defeated");
    }
    public void RemovedSlime()
    {
        if (score != null) 
        {
            score.AddScore(1);
            score.LowScore(1);
        }
        Destroy(gameObject);
    }

    public void UpdateHealthUI()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = HpSlime / maxHp;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "SwordAttackHitbox")
        {
            LockMovement();
            animator.SetBool("isSlimeMoving", false);
        }
        else if (other.tag == "Player")
        {
            animator.SetTrigger("attack");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "SwordAttackHitbox")
        {
            StartCoroutine(WaitAndUnlockMovement(0.5f));
        }
    }

    private IEnumerator WaitAndUnlockMovement(float delay)
    {
        yield return new WaitForSeconds(delay);
        UnlockMovement();
    }


    public void SlimeAttack()
    {
        LockMovement();
        if (spriteRenderer.flipX == true)
        {
            slimeAttck.SlimeAttackLeft();
        }
        else
        {
            slimeAttck.SlimeAttackRight();
        }
    }
    public void EndSlimeAttack()
    {
        UnlockMovement();
        slimeAttck.StopSlimeAttack();
    }

    private void LockMovement()
    {
        canMoveSlime = false;

    }

    private void UnlockMovement()
    {
        canMoveSlime = true;
    }
}
