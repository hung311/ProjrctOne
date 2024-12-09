using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAttck : MonoBehaviour
{
    public float damageSlime = 1f;
    public Collider2D slimeAttack2D;
    Vector3 rightSlimeAttackOffset;

    private void Start()
    {
        rightSlimeAttackOffset = transform.localPosition;
    }

    public void SlimeAttackRight()
    {
        slimeAttack2D.enabled = true;
        transform.localPosition = rightSlimeAttackOffset;
    }

    public void SlimeAttackLeft()
    {

        slimeAttack2D.enabled = true;
        transform.localPosition = new Vector3(rightSlimeAttackOffset.x * -1, rightSlimeAttackOffset.y);
    }

    public void StopSlimeAttack()
    {
        slimeAttack2D.enabled = false;
    }

    private float damageInterval = 1f;
    private float lastDamageTime = -Mathf.Infinity;
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            if (Time.time >= lastDamageTime + damageInterval)
            {
                PlayerController player = other.GetComponent<PlayerController>();
                if (player != null)
                {
                    player.HealthPlayer -= damageSlime;

                    Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();

                    if (playerRb != null)
                    {
                        Vector2 direction = other.transform.position - transform.position;
                        direction.Normalize();

                        float pushStrength = 1f;
                        playerRb.AddForce(direction * pushStrength, ForceMode2D.Impulse);

                    }

                }
                lastDamageTime = Time.time;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if (other.tag == "Player")
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null)
            {
                player.HealthPlayer -= damageSlime;

                Rigidbody2D playerRb = player.GetComponent<Rigidbody2D>();

                if (playerRb != null)
                {
                    Vector2 direction = other.transform.position - transform.position;
                    direction.Normalize();

                    float pushStrength = 1f;
                    playerRb.AddForce(direction * pushStrength, ForceMode2D.Impulse);

                }

            }
        }
    }
}
