using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockSpawner : MonoBehaviour
{
    public GameObject blockPrefab;
    public float moveRange = 1.5f;
    public float speed = 2f;
    public float spawnHeight = 3f;

    private GameObject currentBlock;
    private bool movingRight = true;

    void Start()
    {
        // Spawn first block
        SpawnBlock();
    }

    void Update()
    {
        if (!GameManager.Instance.gameStarted || GameManager.Instance.gameOver)
            return;

        MoveBlock();

        // GetMouseButtonDown also is true if the player touches the screen on his phone
        if (Input.GetMouseButtonDown(0))
        {
            // Move the spawner up so that it doesn't block new blocks
            transform.position = transform.position + new Vector3(0, 0.3f, 0);
            DropBlock();
        }
    }

    // Move the block from side to side until the player drops the block
    void MoveBlock()
    {
        if (currentBlock == null) return;

        float dir = movingRight ? 1 : -1;
        currentBlock.transform.Translate(Vector3.right * dir * speed * Time.deltaTime);

        if (Mathf.Abs(currentBlock.transform.localPosition.x) > moveRange)
            movingRight = !movingRight;
    }

    // Spawn a new block
    void SpawnBlock()
    {
        currentBlock = Instantiate(blockPrefab, transform);
        currentBlock.transform.localPosition = new Vector3(0, spawnHeight, 0);
    }

    // Drop the block straight down
    void DropBlock()
    {
        Rigidbody rb = currentBlock.GetComponent<Rigidbody>();
        rb.isKinematic = false;

        currentBlock = null;

        // Spawn the next block with a small delay
        Invoke(nameof(SpawnBlock), 1f);
    }
}