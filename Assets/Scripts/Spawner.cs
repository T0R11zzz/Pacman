using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject cherryPrefab; 
    private float spawnInterval = 10f; 
    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        StartCoroutine(SpawnCherryRoutine());

    }

    IEnumerator SpawnCherryRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnCherry();
        }
    }

    private void SpawnCherry()
    {
        /*if (cherryPrefab == null)
        {
            Debug.LogError("Cherry Prefab is not assigned!");
            return;
        }*/

        int randomEdge = Random.Range(0, 4);
        Vector3 spawnPosition = Vector3.zero;

        switch (randomEdge)
        {
            case 0: // Generated from left side
                spawnPosition = mainCamera.ViewportToWorldPoint(new Vector3(-0.1f, Random.Range(0f, 1f), 0));
                break;
            case 1: // Generated from right side
                spawnPosition = mainCamera.ViewportToWorldPoint(new Vector3(1.1f, Random.Range(0f, 1f), 0));
                break;
            case 2: // Generated from top
                spawnPosition = mainCamera.ViewportToWorldPoint(new Vector3(Random.Range(0f, 1f), 1.1f, 0));
                break;
            case 3: // Generated from bottom
                spawnPosition = mainCamera.ViewportToWorldPoint(new Vector3(Random.Range(0f, 1f), -0.1f, 0));
                break;
        }

        spawnPosition.z = 0; 

        Instantiate(cherryPrefab, spawnPosition, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
