using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : MonoBehaviour
{
    public float damage = 2;
    Vector2 rightAttackOffset;
    public Collider2D swordCollider2D;
    AudioManager audioManager;


    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    void Start()
    {
        rightAttackOffset = transform.localPosition;
    }

    public void AttackRight()
    {
        swordCollider2D.enabled = true;
        transform.localPosition = rightAttackOffset;
    }
    public void AttackLeft()
    {
        swordCollider2D.enabled = true;
        transform.localScale = new Vector3(-Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);
    }
    public void StopAttack()
    {
        swordCollider2D.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Slime")
        {
            audioManager.PlaySFXOneShot(audioManager.attack);
            SlimeConTroller slime = other.GetComponent<SlimeConTroller>();
            if (slime != null)
            {
                slime.Health -= damage;

                Rigidbody2D slimeRb = slime.GetComponent<Rigidbody2D>();

                if (slimeRb != null)
                {
                    Vector2 direction = other.transform.position - transform.position;

                    direction.Normalize();

                    float pushStrength = 1f;
                    slimeRb.AddForce(direction * pushStrength, ForceMode2D.Impulse);

                }

            }

        }
    }

}