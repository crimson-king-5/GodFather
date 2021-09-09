using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine.UI;

public class BuildRadialMenu : MonoBehaviour
{
    public GameObject partMenu;

    [System.Serializable]
    public class Item
    {
        public Texture2D sprayTexture;
        public Sprite icon;
        public Color color = Color.white;
        public Color hoverColor = Color.white;
        public int sprayPrefabIndex;
        //public Color iconCOlor = Color.white;

    }


    public List<Item> items = new List<Item>();

    public List<Image> menuParts = new List<Image>();


    [Button]
    void BuildMenu()
    {
        menuParts.Clear();

        for (int i = 0; i < items.Count; i++)
        {
            PrefabUtility.InstantiatePrefab(partMenu, transform); 
        }

        var divide = 360 / items.Count;

        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Image>().fillAmount = 1f / items.Count;

            Vector3 myLocalEulerAngles = transform.GetChild(i).transform.localEulerAngles;
            myLocalEulerAngles.z = divide + (divide * i);
            transform.GetChild(i).transform.localEulerAngles = myLocalEulerAngles;
            transform.GetChild(i).GetComponent<Image>().color = items[i].color;

            if(items[i].icon != null)
                transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Image>().sprite = items[i].icon;

            //transform.GetChild(i).GetChild(0).GetChild(0).GetComponent<Image>().color = items[i].iconCOlor;

            menuParts.Add(transform.GetChild(i).GetComponent<Image>());

            Vector3 myLocalEulerAnglesIcon = transform.GetChild(i).GetChild(0).transform.localEulerAngles;

            var prct = 1f / items.Count;

            myLocalEulerAnglesIcon.z = (360f * prct) * .5f;

            transform.GetChild(i).GetChild(0).transform.localEulerAngles = -myLocalEulerAnglesIcon;
            transform.GetChild(i).GetChild(0).GetChild(0).transform.eulerAngles = Vector3.zero;
        }
    }

}
