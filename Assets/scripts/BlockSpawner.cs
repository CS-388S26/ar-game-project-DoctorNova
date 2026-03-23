using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class BlockSpawner : MonoBehaviour
{
    public GameObject blockPrefab;
    public GameObject platform;
    public float moveRange = 1.5f;
    public float speed = 2f;
    public float spawnHeight = 3f;

    private GameObject currentBlock;
    private bool movingRight = true;

    private float initialY = 0.4f;

    private void Awake()
    {
        initialY = transform.position.y;
    }

    public void Reset()
    {
        transform.position = new Vector3(transform.position.x, initialY, transform.position.z);
        Block.blocks.ForEach(block => Destroy(block.gameObject));
        Block.blocks.Clear();
    }

    void Start()
    {
        Reset();
    }

    void Update()
    {
        if (!GameManager.Instance.gameStarted || GameManager.Instance.gameOver)
            return;

        MoveBlock();

        if (Touchscreen.current.primaryTouch.press.wasPressedThisFrame)
        {
            // Move the spawner up so that it doesn't block new blocks
            transform.position = new Vector3(transform.position.x, initialY + GameManager.Instance.currentHeight, transform.position.z);
            DropBlock();
        }
    }

    // Move the block from side to side until the player drops the block
    void MoveBlock()
    {
        if (currentBlock == null) return;

        float dir = movingRight ? 1 : -1;
        currentBlock.transform.position = currentBlock.transform.position + (Vector3.right * dir * speed * Time.deltaTime);

        if (movingRight && currentBlock.transform.localPosition.x > moveRange || 
            !movingRight && currentBlock.transform.localPosition.x < -moveRange)
            movingRight = !movingRight;
    }

    // Spawn a new block
    public void SpawnBlock()
    {
        if (currentBlock != null)
        {
            Block.blocks.Remove(currentBlock.GetComponent<Block>());
            Destroy(currentBlock);
        }

        currentBlock = Instantiate(blockPrefab, transform);
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