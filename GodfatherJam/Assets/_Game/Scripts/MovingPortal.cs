using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovingPortal : MonoBehaviour
{
    public Vector3 movingOffsetPoint;
    public float movingTime;
    public Transform movedObj;
    private Vector3 pos;
    private bool back;

    void Start()
    {
        back = false;
        pos = transform.localPosition + movingOffsetPoint;

        SetPos();
    }

    void SetPos()
    {
        movedObj.DOLocalMove(pos, movingTime).OnComplete(() => NewPos());
    }

    void NewPos()
    {
        Debug.Log("New pos");

        back = !back;

        if (back)
        {
            pos = transform.localPosition - movingOffsetPoint;
            SetPos();
        }

        else
        {
            pos = transform.localPosition + movingOffsetPoint;
            SetPos();
        }
    }


}
