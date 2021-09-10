using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BuildRadialMenu))]
public class SelectionRadialMenu : MonoBehaviour
{

    public Vector2 normalisedMousePos;
    public float currentAngle;
    public int selection;
    private int previousSelection;
    public float angleExactOffset;

    private BuildRadialMenu _brm;

    public KeyCode tagInput;

    public float timePressed;
    public float timeLengthInputMenu;

    private bool menuEnable;
    private bool timer;

    public Transform radialMenuHolder;

    public FirstPersonMovement fpm;

    void Start()
    {
        _brm = GetComponent<BuildRadialMenu>();
        menuEnable = false;
        radialMenuHolder.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(tagInput) && menuEnable)
        {
            Cursor.lockState = CursorLockMode.Locked;
            menuEnable = false;
            fpm.canRot = true;
            radialMenuHolder.gameObject.SetActive(false);
            timePressed = 0;
        }

        if (Input.GetKey(tagInput))
        {
            timePressed += Time.deltaTime;

            if (timePressed >= timeLengthInputMenu)
                menuEnable = true;
        }
        if (Input.GetKeyUp(tagInput))
            timePressed = 0;

        if (!menuEnable) return;

        if (Input.GetKeyDown(PauseController.instance.pauseInput))
        {
            menuEnable = false;
            fpm.canRot = true;
            radialMenuHolder.gameObject.SetActive(false);
            timePressed = 0;
            Debug.Log("Pause while player is in tag menu");
            PauseController.instance.Pause();
        }

        Debug.Log("Menu enable");

        Cursor.lockState = CursorLockMode.None;
        fpm.canRot = false;

        radialMenuHolder.gameObject.SetActive(true);

        normalisedMousePos = new Vector2(Input.mousePosition.x - Screen.width / 2, Input.mousePosition.y - Screen.height / 2);
        currentAngle = Mathf.Atan2(normalisedMousePos.y, normalisedMousePos.x) * Mathf.Rad2Deg;

        currentAngle = (currentAngle + 360) % 360;

        currentAngle += angleExactOffset;

        if (currentAngle > 360)
            currentAngle -= 360;


        selection = (int)currentAngle / (360 / _brm.items.Count);

        if (selection != previousSelection && selection < _brm.items.Count)
            SelectPart();

        if (Input.GetKeyUp(KeyCode.Mouse0))
            OnClickMenu();

        //if (Input.GetKeyUp(KeyCode.Mouse1))
        //    PreviousMenu();
    }

    void SelectPart()
    {
        DeselectPart();

        previousSelection = selection;

        if (_brm.menuParts[selection] != null)
            _brm.menuParts[selection].color = _brm.items[selection].hoverColor;

    }

    void DeselectPart()
    {
        if(_brm.menuParts[previousSelection] != null)
            _brm.menuParts[previousSelection].color = _brm.items[previousSelection].color;
    }


    void OnClickMenu()
    {
        menuEnable = false;
        Debug.Log("Selected : " + selection);
        radialMenuHolder.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        fpm.canRot = true;
        timePressed = 0;


        if (selection >= 2 && selection <= 7)
        {
            Debug.Log("Color");
            fpm.actualSprayColor = _brm.items[selection].color;
        }
        else
        {
            Debug.Log("changed spray");
            fpm.arrowDecal = fpm.sprayPrefab[_brm.items[selection].sprayPrefabIndex];
            //fpm.actualTexture = _brm.items[selection].sprayTexture;
        }

        if (_brm.items[selection].useWhiteColor)
            fpm.actualSprayColor = Color.white;
    }
}
