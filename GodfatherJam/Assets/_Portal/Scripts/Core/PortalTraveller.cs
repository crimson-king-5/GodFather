﻿using System.Collections.Generic;
using UnityEngine;

public class PortalTraveller : MonoBehaviour {

    public GameObject graphicsObject;
    public GameObject graphicsClone { get; set; }
    public Vector3 previousOffsetFromPortal { get; set; }

    public Material[] originalMaterials { get; set; }
    public Material[] cloneMaterials { get; set; }

    public Camera playerCam;

    private Vector3 playerVector;
    private Vector3 camVector;

    public virtual void Teleport (Transform fromPortal, Transform toPortal, Vector3 pos, Quaternion rot, Portal portal) {

        //if(playerCam != null)
        //{
        //    transform.position = pos;
        //    playerVector = rot.eulerAngles;
        //    playerVector.x = 0;
        //    //Z & Y FOR PLAYER
        //    transform.eulerAngles = playerVector;

        //    //X FOR CAM
        //    camVector = rot.eulerAngles;
        //    camVector.y = 0;
        //    camVector.z = 0;
        //    playerCam.transform.eulerAngles = -camVector;
        //}
        //else
        //{

        //}

        transform.position = pos;
        transform.rotation = rot;

    }

    // Called when first touches portal
    public virtual void EnterPortalThreshold () {
        if (graphicsClone == null)
        {
            graphicsClone = Instantiate(graphicsObject);
            graphicsClone.transform.parent = graphicsObject.transform.parent;
            graphicsClone.transform.localScale = graphicsObject.transform.localScale;
            originalMaterials = GetMaterials(graphicsObject);
            cloneMaterials = GetMaterials(graphicsClone);
        }
        else
        {
            graphicsClone.SetActive(true);
        }
    }

    // Called once no longer touching portal (excluding when teleporting)
    public virtual void ExitPortalThreshold () {
        graphicsClone.SetActive (false);
        // Disable slicing
        for (int i = 0; i < originalMaterials.Length; i++) {
            originalMaterials[i].SetVector ("sliceNormal", Vector3.zero);
        }
    }

    public void SetSliceOffsetDst (float dst, bool clone) {
        for (int i = 0; i < originalMaterials.Length; i++)
        {
            if (clone)
            {
                cloneMaterials[i].SetFloat("sliceOffsetDst", dst);
            }
            else
            {
                originalMaterials[i].SetFloat("sliceOffsetDst", dst);
            }

        }
    }

    Material[] GetMaterials (GameObject g) {
        var renderers = g.GetComponentsInChildren<MeshRenderer> ();
        var matList = new List<Material> ();
        foreach (var renderer in renderers) {
            foreach (var mat in renderer.sharedMaterials) {
                matList.Add (mat);
            }
        }
        return matList.ToArray ();
    }
}