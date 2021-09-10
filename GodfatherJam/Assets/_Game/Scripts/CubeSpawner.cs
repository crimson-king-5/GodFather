using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    public GameObject cube;
    public float maxCube;
    public Transform spawnPoint;
    private GameObject spawnedCube;

    public string eventTextDisplay = "You <b>spawned</b> a cube somewhere.";
    public float eventTextDisplayTime = 6;

    public void Start()
    {
        //SpawnCube();
    }

    public void SpawnCube()
    {
        if(spawnedCube != null)
            spawnedCube.SetActive(false);

        spawnedCube = Instantiate(cube, spawnPoint);
    }


    public void OnTriggerEnter(Collider other)
    {
        SpawnCube();
        EventController.instance.NewTextEvent(eventTextDisplay, eventTextDisplayTime);
    }
}
