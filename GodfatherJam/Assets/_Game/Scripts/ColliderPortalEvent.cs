using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderPortalEvent : MonoBehaviour
{
    public Collider portalCollider;

    public PortalEventSystem pes;

    public LayerMask layermask;

    void Awake()
    {
        portalCollider = GetComponent<Collider>();   
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger enter");

        if (other.gameObject.layer == layermask.value)
        {
            //_TriggerEnter();
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("Trigger exit");
        
        if (other.gameObject.layer == layermask.value)
        {
            //_TriggerExit();
        }
    }

    void OnCollisionEnter(Collision col)
    {
        Debug.Log("Col enter");
    }


}
