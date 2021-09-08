using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class PortalEventSystem : MonoBehaviour
{
    public static PortalEventSystem instance;

    public bool enablePortal;

    [EnableIf("@this.enablePortal")]
    public Collider triggerA;

    [EnableIf("@this.enablePortal")]
    public Portal[] enabledPortals;
    public bool timerA;
    [EnableIf("@this.timerA && this.enablePortal")]
    [Header("Time Before Enable The Portal After The Event Call")]
    public float enableTimer;
    //public LayerMask triggerWithA;


    [Space(33)]

    public bool disablePortal;

    [EnableIf("@this.disablePortal")]
    public Collider triggerB;

    [EnableIf("@this.disablePortal")]
    public Portal[] disabledPortals;
    public bool timerB;
    [EnableIf("@this.timerB && this.disablePortal")]
    [Header("Time Before Disable The Portal After The Event Call")]
    public float disableTimer;
    //public LayerMask triggerWithB;

    void Awake()
    {
        instance = this;
    }

    public void PortalEvent(Portal portal)
    {
        if (enablePortal)
            _TriggerA(portal);

        if (disablePortal)
            _TriggerB(portal);
        


    }

    void _TriggerA(Portal portal)
    {
        if (portal == triggerA.GetComponent<Portal>())
        {
            Debug.Log("Event A");

            StartCoroutine(EnablePortal());

            for (int i = 0; i < enabledPortals.Length; i++)
            {
                enabledPortals[i].gameObject.SetActive(true);
            }
        }
    }

    IEnumerator DisablePortal()
    {
        yield return new WaitForSeconds(disableTimer);

        for(int i = 0; i < disabledPortals.Length; i++)
        {
            disabledPortals[i].gameObject.SetActive(false);
        }
    }

    IEnumerator EnablePortal()
    {
        yield return new WaitForSeconds(enableTimer);
    }

    void _TriggerB(Portal portal)
    {
        if (portal == triggerB.GetComponent<Portal>())
        {
            Debug.Log("Event B");

            StartCoroutine(DisablePortal());
        }

    }
}
