using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


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

    public List<Collider> DeathZone;

    public List<SpawnPoint> SpawnPoints;

    public SpawnPoint SavedSP;
    private GameObject Player;


    private void Awake()
    {
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

    }
    public void SpawnUpdateEvent(int id,Vector3 pos)
    {
        if (id > SavedSP.id)
            {
                SavedSP.position = pos;
                SavedSP.id = id;
            }
        
    }
}
