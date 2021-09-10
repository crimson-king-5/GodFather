using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using DG.Tweening;
using Sirenix.OdinInspector;


public class EventController : MonoBehaviour
{
    [System.Serializable]
    public class SpawnPoint
    {
        public int id;
        public Collider spawn;
        public Vector3 position;
    }

    public static EventController instance;

    [Header("Death Events")]
    public List<Collider> deathZone;

    [Header("Spawn Events")]
    public List<SpawnPoint> spawnPoints;

    [HideInInspector]
    public SpawnPoint savedSpawnPoint;
    private GameObject player;

    [Header("Percent")]
    public float stagePercent;

    [Header("Text Event")]
    public TMPro.TextMeshProUGUI eventText;
    public Color startColor = Color.white;
    public Color endColor;
    private Vector3 originalPos;
    public float upOffsetAnimation = 350;

    [Header("Spawn Text Event Values")]
    public string spawnPointTextEventDisplay = "Spawn Point <b>Update</b>";
    public float spawnPointTextEventDisplayTime = 5;

    [Header("Death Text Event Values")]
    public string deathTextEventDisplay = "You <b>died</b>. You <b>respawned</b> on spawn point.";
    public float deathTextEventDisplayTime = 4;

    [Header("Win Text Event Values")]
    public Collider winCollider;
    public string winTextEventDisplay = "GG ! You win a beer ! -> 22-24 Allée de l'Arche, 92400 Courbevoie";
    public float winTextEventDisplayTIme = 10;


    private void Awake()
    {
        winCollider.gameObject.AddComponent<EventTrigger>().eventType = EventTrigger.EventType.WIN;

        originalPos = eventText.transform.position;
        instance = this;
        for(int i = 0; i < deathZone.Count; i++)
        {
           deathZone[i].gameObject.AddComponent<EventTrigger>().eventType = EventTrigger.EventType.DEATH;
        }
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            EventTrigger et = spawnPoints[i].spawn.gameObject.AddComponent<EventTrigger>();
            et.eventType = EventTrigger.EventType.SPAWN;
            et.id = spawnPoints[i].id;
        }


        player = FindObjectOfType<FirstPersonMovement>().gameObject;
        savedSpawnPoint.position = player.transform.position;
    }
    
    public void DeathEvent()
    {        
        player.transform.position = savedSpawnPoint.position;
        NewTextEvent(deathTextEventDisplay, deathTextEventDisplayTime);

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            DeathEvent();
    }

    public void SpawnUpdateEvent(int id,Vector3 pos)
    {
        if (id > savedSpawnPoint.id)
        {
            savedSpawnPoint.position = pos;
            savedSpawnPoint.id = id;
            stagePercent = Mathf.Round((id * spawnPoints.Count) / 100);
            NewTextEvent(spawnPointTextEventDisplay, spawnPointTextEventDisplayTime);
        }
        
    }

    [Button]
    public void NewTextEvent(string newEventText, float time)
    {
        eventText.color = startColor;

        eventText.gameObject.transform.position = originalPos;

        eventText.text = newEventText;
        eventText.DOColor(endColor, time);
        eventText.gameObject.transform.DOLocalMoveY(upOffsetAnimation, time);

        //DOTween.To(() => balance, x => balance = x, to, 2).OnUpdate(UpdateUI).OnComplete(UpdateUI);
    }

}
