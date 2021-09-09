using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    public GameObject cube;
    public float maxCube;
    public Transform spawnPoint;
    private GameObject spawnedCube;

    public void Start()
    {
        SpawnCube();
    }

    public void SpawnCube()
    {
        if(spawnedCube != null)
            spawnedCube.SetActive(false);

        spawnedCube = Instantiate(cube, spawnPoint);
    }


    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            SpawnCube();
    }
}
