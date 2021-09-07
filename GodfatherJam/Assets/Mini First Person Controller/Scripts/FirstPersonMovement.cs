using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class FirstPersonMovement : PortalTraveller
{
    public float speed = 5;

    [Header("Running")]
    public bool canRun = true;
    public bool IsRunning { get; private set; }
    public float runSpeed = 9;
    public KeyCode runningKey = KeyCode.LeftShift;

    Rigidbody rigidbody;
    /// <summary> Functions to override movement speed. Will use the last added override. </summary>
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();

    public float yaw;
    public float pitch;
    float smoothYaw;
    float smoothPitch;
    public Camera cam;

    private Vector3 portalVelocity;
    private Vector3 portalAngularVelocity;
    public Vector3 maxPortalVelocity;

    public bool inPortal;

    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        //// Get the rigidbody on this.
        rigidbody = GetComponent<Rigidbody>();

        yaw = transform.eulerAngles.y;
        pitch = cam.transform.localEulerAngles.x;
        smoothYaw = yaw;
        smoothPitch = pitch;
    }

    void FixedUpdate()
    {
        // Update IsRunning from input.
        IsRunning = canRun && Input.GetKey(runningKey);

        // Get targetMovingSpeed.
        float targetMovingSpeed = IsRunning ? runSpeed : speed;
        if (speedOverrides.Count > 0)
        {
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
        }

        // Get targetVelocity from input.
        Vector2 targetVelocity = new Vector2(Input.GetAxis("Horizontal") * targetMovingSpeed, Input.GetAxis("Vertical") * targetMovingSpeed);

        // Apply movement.
        rigidbody.velocity = (transform.rotation * new Vector3(targetVelocity.x, rigidbody.velocity.y, targetVelocity.y));

        //if (portalVelocity == Vector3.zero)
        //{
            
        //    //Debug.Log("protal velo : " + portalVelocity);

        //    //if (portalVelocity.y > maxPortalVelocity.y)
        //    //    rigidbody.velocity += maxPortalVelocity;
        //    //else
        //    //    rigidbody.velocity += portalVelocity;
        //}

        ////rigidbody.angularVelocity += portalAngularVelocity;

        //if (portalVelocity != Vector3.zero)
        //{
        //    portalAngularVelocity = Vector3.zero;
        //    portalVelocity = Vector3.zero;
        //}

        _Rot();
    }

    public float mouseSensitivity = 10;
    public Vector2 pitchMinMax = new Vector2(-40, 85);
    public float rotationSmoothTime = 0.1f;
    float yawSmoothV;
    float pitchSmoothV;

    void _Rot()
    {
        float mX = Input.GetAxisRaw("Mouse X");
        float mY = Input.GetAxisRaw("Mouse Y");

        float mMag = Mathf.Sqrt(mX * mX + mY * mY);
        if (mMag > 5)
        {
            mX = 0;
            mY = 0;
        }

        yaw += mX * mouseSensitivity;
        pitch -= mY * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);
        smoothPitch = Mathf.SmoothDampAngle(smoothPitch, pitch, ref pitchSmoothV, rotationSmoothTime);
        smoothYaw = Mathf.SmoothDampAngle(smoothYaw, yaw, ref yawSmoothV, rotationSmoothTime);

        transform.eulerAngles = Vector3.up * smoothYaw;
        cam.transform.localEulerAngles = Vector3.right * smoothPitch;
    }

    public override void Teleport(Transform fromPortal, Transform toPortal, Vector3 pos, Quaternion rot, Portal portal)
    {
        Debug.Log("in portal true");
        inPortal = true;
        base.Teleport(fromPortal, toPortal, pos, rot, portal);
        GetComponent<Rigidbody>().velocity = toPortal.TransformVector(fromPortal.InverseTransformVector(GetComponent<Rigidbody>().velocity));
        GetComponent<Rigidbody>().angularVelocity = toPortal.TransformVector(fromPortal.InverseTransformVector(GetComponent<Rigidbody>().angularVelocity));

        portalVelocity = toPortal.TransformVector(fromPortal.InverseTransformVector(GetComponent<Rigidbody>().velocity));
        portalAngularVelocity = toPortal.TransformVector(fromPortal.InverseTransformVector(GetComponent<Rigidbody>().angularVelocity));

        Vector3 eulerRot = rot.eulerAngles;
        float delta = Mathf.DeltaAngle(smoothYaw, eulerRot.y);
        yaw += delta;
        //RAJOUTER AU DELTA LA ROT DU PORTAIL EN Y
        smoothYaw += delta;
        //smoothYaw += portal.linkedPortal.transform.eulerAngles.y;
        Debug.Log("portal : " + portal.transform.eulerAngles.x + " name :" + portal.linkedPortal.name);
        var angles = Vector3.zero;
        angles.x = portal.transform.eulerAngles.x;
        Debug.Log("X : " + angles.x);
        playerCam.transform.eulerAngles = angles;
        transform.eulerAngles = Vector3.up * smoothYaw;
        Physics.SyncTransforms();
    }
}