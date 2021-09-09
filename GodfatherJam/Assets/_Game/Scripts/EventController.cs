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
    public string spawnPointTextEventDisplay = "Spawn Point <b>Update</b>";
    public float spawnPointTextEventDisplayTime = 5;

    public string deathTextEventDisplay = "You <b>died</b>. You <b>respawned</b> on spawn point.";
    public float deathTextEventDisplayTime = 4;

    public static EventController instance;

    public List<Collider> DeathZone;

    public List<SpawnPoint> SpawnPoints;

    public SpawnPoint SavedSP;
    private GameObject Player;
    public float stagePercent;

    [Header("Text Event")]
    public TMPro.TextMeshProUGUI eventText;
    public Color startColor = Color.white;
    public Color endColor;
    //public float textFadeDuration = 7;
    private Vector3 originalPos;
    public float upOffsetAnimation = 350;

    //private Sequence sequenceText;
    //private Sequence sequenceTransform;

    private void Awake()
    {
        originalPos = eventText.transform.position;
        instance = this;
        for(int i = 0; i < DeathZone.Count; i++)
        {
           DeathZone[i].gameObject.AddComponent<EventTrigger>().eventType = EventTrigger.EventType.DEATH;
        }
        for (int i = 0; i < SpawnPoints.Count; i++)
        {
            EventTrigger et = SpawnPoints[i].spawn.gameObject.AddComponent<EventTrigger>();
            et.eventType = EventTrigger.EventType.SPAWN;
            et.id = SpawnPoints[i].id;
        }


        Player = GameObject.Find("Player");
        SavedSP.position = Player.transform.position;
    }
    
    public void DeathEvent()
    {        
        Player.transform.position = SavedSP.position;
        NewTextEvent(deathTextEventDisplay, deathTextEventDisplayTime);

    }
    public void SpawnUpdateEvent(int id,Vector3 pos)
    {
        if (id > SavedSP.id)
        {
            SavedSP.position = pos;
            SavedSP.id = id;
            stagePercent = Mathf.Round((id * SpawnPoints.Count) / 100);
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
