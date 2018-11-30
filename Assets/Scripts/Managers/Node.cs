using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : MonoBehaviour {

    public Color hoverColor;
    public Color selectedColor;
    public GameObject buildTurret;

    [HideInInspector]
    public Color defaultColor;
    private Renderer renderer;
    private GameManager gameManager;

    private void Start()
    {
        renderer = GetComponent<Renderer>();
        defaultColor = renderer.material.color;
        gameManager = GameManager.instance;
    }

    private void OnMouseEnter()
    {
        if (!gameManager.gameIsPaused)
        {
            SetColor(hoverColor);
        }
    }

    private void OnMouseExit()
    {
        if (!gameManager.gameIsPaused)
        {
            if (BuildManager.instance.GetSelectedNode() != this)
            {
                SetColor(defaultColor);
            }
        }
    }

    public void SetColor(Color color)
    {
        if (!gameManager.gameIsPaused)
        {
            renderer.material.color = color;
        }
    }
}
