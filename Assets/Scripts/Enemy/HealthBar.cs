using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour {

    public Image healthFill;
    public Enemy targetScript;
    public Vector3 offset;

    private float maxHealth;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.instance;
        maxHealth = targetScript.health;
    }

    private void Update()
    {
        if (gameManager.gameIsPaused == false)
        {
            if (targetScript == null)
                Destroy(gameObject);
            else
            {
                transform.position = targetScript.transform.position + offset;
                healthFill.rectTransform.localScale = new Vector3(targetScript.health / maxHealth, 1f, 1f);
            }
        }
    }
}
