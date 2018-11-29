using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMenuCanvas : MonoBehaviour {

    public GameObject[] menus;

    public void HideMenus(string exceptionTag)
    {
        for(int index = 0; index < menus.Length; index++)
        {
            if(menus[index].tag == exceptionTag)
            {
                menus[index].SetActive(true);
            }
            else
            {
                menus[index].SetActive(false);
            }
        }
    }
}
