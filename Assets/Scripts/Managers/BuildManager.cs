using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BuildManager : MonoBehaviour {

    //Singleton
    public static BuildManager instance;

    private void Awake()
    {
        if(instance != null)
        {
            Debug.LogWarning("Error! more than one instance for build manager.");
            Destroy(this.gameObject);
            return;
        }
        instance = this;
    }
    //End singleton

    public int levelMoney = 350;
    public Text walletText;
    public Transform turretParentTransform;
    public BuildMenuCanvas buildMenuCanvas;

    private Vector3 offset = new Vector3(0f, 1.1f, 0f);
    private GameManager gameManager;
    private GameObject buildTurretUI;
    private Node selectedNodeScript;

    private TurretBunker bunkerScript;
    private TurretCannon cannonScript;
    private TurretLaser laserScript;

    private void Start()
    {
        buildTurretUI = GameObject.FindWithTag("TurretSpawnerUI");
        gameManager = GameManager.instance;
    }

    private void Update()
    {
        if (!gameManager.gameIsPaused)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100))
                {
                    if (hit.transform.gameObject.tag == "Node" && !EventSystem.current.IsPointerOverGameObject())
                    {
                        SelectNode(hit.transform.gameObject.GetComponent<Node>());
                        return;
                    }
                }
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    DeselectAll();
                }
            }
        }
    }

    // Bring everything back to the initial state
    private void DeselectAll()
    {
        if (selectedNodeScript != null)
        {
            selectedNodeScript.SetColor(selectedNodeScript.defaultColor);
        }
        selectedNodeScript = null;

        if (laserScript != null)
        {
            laserScript.rangeUI.enabled = false;
        }
        if (cannonScript != null)
        {
            cannonScript.rangeUI.enabled = false;
        }
        if (bunkerScript != null)
        {
            bunkerScript.rangeUI.enabled = false;
        }
        DeactivateUI();
    }

    private void DeactivateUI()
    {
        buildMenuCanvas.HideMenus(null);
    }

    private void ActivateUI(string tag)
    {
        buildMenuCanvas.HideMenus(tag);
    }

    // Add money to our wallet (hopefully this game will do the same)
    public void AddMoney(int value)
    {
        levelMoney += value;
        walletText.text = "$" + levelMoney;
    }

    public void SelectNode(Node node)
    {
        //If we clicked a node that we already had selected, then we want to deselect it
        if (selectedNodeScript == node)
        {
            DeselectAll();
        }
        else
        {
            //If we had a node that had a turret already build on it, then we want to hide the rangeUI before selecting a new node
            if (selectedNodeScript != null && selectedNodeScript.buildTurret != null)
            {
                laserScript = selectedNodeScript.buildTurret.GetComponent<TurretLaser>();
                if(laserScript != null)
                    laserScript.rangeUI.enabled = false;

                cannonScript = selectedNodeScript.buildTurret.GetComponent<TurretCannon>();
                if(cannonScript != null)
                    cannonScript.rangeUI.enabled = false;

                bunkerScript = selectedNodeScript.buildTurret.GetComponent<TurretBunker>();
                if(bunkerScript != null)
                    bunkerScript.rangeUI.enabled = false;
            }

            //Position the build UI
            selectedNodeScript = node;
            selectedNodeScript.SetColor(selectedNodeScript.selectedColor);
            buildTurretUI.transform.position = new Vector3(selectedNodeScript.transform.position.x, 0f, selectedNodeScript.transform.position.z);

            //If we have a turret already placed on the selected node, then we want to show it's range
            if (selectedNodeScript.buildTurret != null)
            {
                laserScript = selectedNodeScript.buildTurret.GetComponent<TurretLaser>();
                if (laserScript != null)
                {
                    ActivateUI(laserScript.turretName);
                    laserScript.rangeUI.enabled = true;
                }

                cannonScript = selectedNodeScript.buildTurret.GetComponent<TurretCannon>();
                if (cannonScript != null)
                {
                    ActivateUI(cannonScript.turretName);
                    cannonScript.rangeUI.enabled = true;
                }

                bunkerScript = selectedNodeScript.buildTurret.GetComponent<TurretBunker>();
                if (bunkerScript != null)
                {
                    ActivateUI(bunkerScript.turretName);
                    bunkerScript.rangeUI.enabled = true;
                }
            }
            else
            {
                ActivateUI("BuildMenuDefault");
            }
        }
    }

    public Node GetSelectedNode()
    {
        return selectedNodeScript;
    }

    //Called by clicking buttons in the gameView
    public void BuildTurret(GameObject turretPrefab)
    {
        if (!gameManager.gameIsPaused)
        {
            laserScript = turretPrefab.GetComponent<TurretLaser>();
            if (laserScript != null)
            {
                if (laserScript.status.price <= levelMoney)
                {
                    if (selectedNodeScript.buildTurret != null)
                    {
                        Destroy(selectedNodeScript.buildTurret);
                    }

                    AddMoney(-laserScript.status.price);

                    GameObject clone = Instantiate(turretPrefab, turretParentTransform);
                    clone.transform.position = selectedNodeScript.transform.position + offset;
                    selectedNodeScript.buildTurret = clone;

                    DeselectAll();
                }
            }

            cannonScript = turretPrefab.GetComponent<TurretCannon>();
            if (cannonScript != null)
            {
                if (cannonScript.status.price <= levelMoney)
                {
                    if (selectedNodeScript.buildTurret != null)
                    {
                        Destroy(selectedNodeScript.buildTurret);
                    }

                    AddMoney(-cannonScript.status.price);

                    GameObject clone = Instantiate(turretPrefab, turretParentTransform);
                    clone.transform.position = selectedNodeScript.transform.position + offset;
                    selectedNodeScript.buildTurret = clone;

                    DeselectAll();
                }
            }

            bunkerScript = turretPrefab.GetComponent<TurretBunker>();
            if (bunkerScript != null)
            {
                if (bunkerScript.status.price <= levelMoney)
                {
                    if (selectedNodeScript.buildTurret != null)
                    {
                        Destroy(selectedNodeScript.buildTurret);
                    }

                    AddMoney(-bunkerScript.status.price);

                    GameObject clone = Instantiate(turretPrefab, turretParentTransform);
                    clone.transform.position = selectedNodeScript.transform.position + offset;
                    selectedNodeScript.buildTurret = clone;

                    DeselectAll();
                }
            }
        }
    }

    public void SellTurret()
    {
        if (!gameManager.gameIsPaused)
        {
            //Destroy the build turret; deselect everything related to the selected node
            laserScript = selectedNodeScript.buildTurret.GetComponent<TurretLaser>();
            if (laserScript != null)
            {
                AddMoney(laserScript.status.price / 2);
            }

            cannonScript = selectedNodeScript.buildTurret.GetComponent<TurretCannon>();
            if (cannonScript != null)
            {
                AddMoney(cannonScript.status.price / 2);
            }

            bunkerScript = selectedNodeScript.buildTurret.GetComponent<TurretBunker>();
            if (bunkerScript != null)
            {
                AddMoney(bunkerScript.status.price / 2);
            }

            Destroy(selectedNodeScript.buildTurret);
            DeselectAll();
        }
    }
}
