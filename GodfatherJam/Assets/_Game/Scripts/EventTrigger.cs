using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTrigger : MonoBehaviour
{
    public enum EventType
    {
        DEATH,
        SPAWN,
        WIN,
        QUIT
    }

    public EventType eventType;
    public int id;

    private Transform player;

    private void Awake()
    {
        player = FindObjectOfType<FirstPersonMovement>().transform;
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.transform == player.transform)
        {
        switch (eventType)
            {
            case EventType.DEATH:
                EventController.instance.DeathEvent();
                break;
            case EventType.SPAWN:
                EventController.instance.SpawnUpdateEvent(id,transform.position);
                break;
            case EventType.WIN:
                EventController.instance.NewTextEvent(EventController.instance.winTextEventDisplay, EventController.instance.winTextEventDisplayTIme);
                break;
                case EventType.QUIT:
                    EventController.instance.NewTextEvent(EventController.instance.quitTextEventDisplay, EventController.instance.quitTextEventDisplayTime);
                    EventController.instance.QuitEvent(EventController.instance.quitTextEventDisplayTime);
                    break;
            }
        }
        
    }
}
