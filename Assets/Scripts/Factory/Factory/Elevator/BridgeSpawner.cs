using System.Collections;
using UnityEngine;
using TMPro;

public class BridgeSpawner : MonoBehaviour
{
    [Header("Bridge Settings")]
    public GameObject bridgePrefab;
    public Transform spawnPoint;
    public float bridgeDuration = 4f; 

    private bool playerInRange = false;
    private bool isSpawning = false;

    private void Start()
    {
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !isSpawning)
        {
            StartCoroutine(SpawnBridge());
        }
    }

    private IEnumerator SpawnBridge()
    {
        isSpawning = true;

        GameObject bridge = Instantiate(bridgePrefab, spawnPoint.position, spawnPoint.rotation);
        yield return new WaitForSeconds(bridgeDuration);
        Destroy(bridge);

        isSpawning = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
