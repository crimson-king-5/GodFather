using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimplePortalTraveller : PortalTraveller
{
    public override void Teleport(Transform fromPortal, Transform toPortal, Vector3 pos, Quaternion rot, Portal portal)
    {
        base.Teleport(fromPortal, toPortal, pos, rot, portal);
    }
}
