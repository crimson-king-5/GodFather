using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DecalSystem;

public class AutoBuildDecal : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Decal>().BuildAndSetDirty();
    }

}
