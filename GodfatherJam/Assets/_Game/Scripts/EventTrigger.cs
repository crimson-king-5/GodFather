using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : MonoBehaviour
{
    public enum EventType
    {
        DEATH,
        SPAWN
    }

    public EventType eventType;
    public int id;
    private void OnTriggerEnter(Collider collision)
    {
        if(collision.transform.name == "Player")
        {
            switch (eventType)
                    {
                        case EventType.DEATH:
                            EventController.instance.DeathEvent();
                            break;
                        case EventType.SPAWN:
                            EventController.instance.SpawnUpdateEvent(id,transform.position);
                            break;
                    }
        }
        
    }
}
