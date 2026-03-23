using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public static List<Block> blocks = new List<Block>();

    private void Awake()
    {
        blocks.Add(this);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Check if the block landed on the platform or another block
        if (collision.gameObject.CompareTag("Platform") ||
            collision.gameObject.CompareTag("Block"))
        {
            GameManager.Instance.AddBlock(transform.position.y);
        }
        // If it landed on something else then the player lost
        else
        {
            GameManager.Instance.EndGame();
        }
    }
}