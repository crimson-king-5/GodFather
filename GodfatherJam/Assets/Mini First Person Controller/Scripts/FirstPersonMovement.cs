using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using DG.Tweening;
using DecalSystem;

public class FirstPersonMovement : PortalTraveller
{
    public float actualSpeed = 0;
    public float speed = 5;
    public float accelerationTime;

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
    public float smoothPitch;
    public Camera cam;

    private Vector3 portalVelocity;
    private Vector3 portalAngularVelocity;
    public Vector3 maxPortalVelocity;

    public bool inPortal;
    private float myAngle;

    [Header("Spray")]
    public float maxDistSpray;
    public Vector3 sprayScale = Vector3.one;
    public int maxTag;
    public KeyCode tagInput;
    private RaycastHit sprayHit;
    public LayerMask tagableLayer;
    public GameObject arrowDecal;

    private RaycastHit raycastHit;

    Vector3 RaycastOrigin => transform.position - Vector3.forward * maxDistSpray;

    public List<GameObject> sprays = new List<GameObject>();
    private List<MaterialPropertyBlock> _mpb = new List<MaterialPropertyBlock>();
    public float maxSprays;
    public Color actualSprayColor;
    //public Texture actualTexture;
    public List<GameObject> sprayPrefab = new List<GameObject>();

    public bool canRot;

    private void Update()
    {
        if (Input.GetKeyUp(tagInput))
            _Spray();

        Debug.DrawRay(cam.transform.position, cam.transform.forward * 10, Color.blue);

    }

    void OnDrawGizmosSelected()
    {
        Debug.DrawLine(cam.transform.position, cam.transform.forward * 10, Color.blue);
    }

    void _Spray()
    {
        Debug.Log("Spray");

        if (Physics.Raycast(cam.transform.position, cam.transform.forward * 10, out sprayHit, maxDistSpray, tagableLayer))
        {
            var go = Instantiate(arrowDecal, sprayHit.point, Quaternion.identity);
            sprays.Add(go);
            var a = new MaterialPropertyBlock();
            a.SetColor("_Color", actualSprayColor);
            go.GetComponent<Renderer>().SetPropertyBlock(a);

            //mat.SetColor("_Color", actualSprayColor);
            //mat.SetTexture("_MainTex", actualTexture);


            if (sprays.Count > maxSprays)
            {
                sprays[0].SetActive(false);
                sprays.RemoveAt(0);
            }


            go.transform.eulerAngles = cam.transform.eulerAngles;
            StartCoroutine(WaitingToBuildDecal(go));
        }
    }

    IEnumerator WaitingToBuildDecal(GameObject go)
    {
        yield return new WaitForSeconds(.1f);

        go.GetComponent<Decal>().BuildAndSetDirty();
    }

    void Awake()
    {
        canRot = true;
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

        //Debug.Log("target move speed : " + targetMovingSpeed);

        if (speedOverrides.Count > 0)
        {
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
        }

        DOTween.To(() => actualSpeed, x => actualSpeed = x, targetMovingSpeed, accelerationTime);

        //if (actualSpeed >= targetMovingSpeed)
        //    actualSpeed = accelerationForce * actualSpeed * Time.deltaTime;
        //else
        //    actualSpeed = targetMovingSpeed;

        // Get targetVelocity from input.
        Vector2 targetVelocity = Vector2.zero;
        targetVelocity = new Vector2(Input.GetAxis("Horizontal") * actualSpeed, Input.GetAxis("Vertical") * actualSpeed);


        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
                actualSpeed = 0;

        //// Apply movement.
        rigidbody.velocity = (transform.rotation * new Vector3(targetVelocity.x, rigidbody.velocity.y, targetVelocity.y));

        rigidbody.velocity += portalVelocity;

        //if (portalVelocity == Vector3.zero)
        //{
        //    if (portalVelocity.y > maxPortalVelocity.y)
        //        rigidbody.velocity += maxPortalVelocity;
        //    else
        //        rigidbody.velocity += portalVelocity;
        //}

        rigidbody.angularVelocity += portalAngularVelocity;

        if (portalVelocity != Vector3.zero)
        {
            portalAngularVelocity = Vector3.zero;
            portalVelocity = Vector3.zero;
        }

        if(canRot)
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
        //pitch += myAngle;
        //myAngle = 0;
        smoothPitch = Mathf.SmoothDampAngle(smoothPitch, pitch, ref pitchSmoothV, rotationSmoothTime);
        smoothYaw = Mathf.SmoothDampAngle(smoothYaw, yaw, ref yawSmoothV, rotationSmoothTime);

        transform.eulerAngles = Vector3.up * smoothYaw;

        //if(myAngle != 0)
        //{
        //    smoothPitch = 480;
        //    Debug.Log("smooth : " + smoothPitch);
        //    myAngle = 0;
        //    Debug.Break();
        //}

        cam.transform.localEulerAngles = Vector3.right * smoothPitch;
    }


    public override void Teleport(Transform fromPortal, Transform toPortal, Vector3 pos, Quaternion rot, Portal portal)
    {
        //Debug.Log("in portal true");
        inPortal = true;
        base.Teleport(fromPortal, toPortal, pos, rot, portal);
        GetComponent<Rigidbody>().velocity = toPortal.TransformVector(fromPortal.InverseTransformVector(GetComponent<Rigidbody>().velocity));
        GetComponent<Rigidbody>().angularVelocity = toPortal.TransformVector(fromPortal.InverseTransformVector(GetComponent<Rigidbody>().angularVelocity));

        portalVelocity = toPortal.TransformVector(fromPortal.InverseTransformVector(GetComponent<Rigidbody>().velocity));
        portalAngularVelocity = toPortal.TransformVector(fromPortal.InverseTransformVector(GetComponent<Rigidbody>().angularVelocity));

        Vector3 eulerRot = rot.eulerAngles;
        float delta = Mathf.DeltaAngle(smoothYaw, eulerRot.y);
        yaw += delta;
        smoothYaw += delta;
        var angles = Vector3.zero;
        angles.x = portal.transform.eulerAngles.x;
        //Debug.Log("X : " + angles.x);
        myAngle = angles.x;
        transform.eulerAngles = Vector3.up * smoothYaw;
        Physics.SyncTransforms();
        //graphicsObject.transform.eulerAngles = angles;

        Debug.Log("Portal : " + portal.name);

        if(PortalEventSystem.instance != null)
            PortalEventSystem.instance.PortalEvent(portal.linkedPortal);
    }
}