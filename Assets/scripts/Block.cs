using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public Rigidbody rigid;
    private bool hasLanded = false;

    private static bool AreFloatsEqual(float a, float b, float epsilon)
    {
        return Mathf.Abs(a - b) <= epsilon;
    }

    private void Update()
    {

        // If the block fell and is now at a negative location then the player lost
        if (transform.position.y < -0.1)
        {
            GameManager.Instance.EndGame();
        } 
        // Check if the block is resting.
        else if (hasLanded && AreFloatsEqual(rigid.velocity.sqrMagnitude, 0, 0.000001f))
        {
            GameManager.Instance.AddBlock(transform.position.y);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (hasLanded) return;

        // Check if the block landed on the platform or another block
        if (collision.gameObject.CompareTag("Platform") ||
            collision.gameObject.CompareTag("Block"))
        {
            hasLanded = true;
        }
        // If it landed on something else then the player lost
        else
        {
            GameManager.Instance.EndGame();
        }
    }
}