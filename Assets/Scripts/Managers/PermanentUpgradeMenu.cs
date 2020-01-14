using UnityEngine;
using UnityEngine.UI;

public class PermanentUpgradeMenu : MonoBehaviour {

    public GameObject cannonMenu;
    public GameObject laserMenu;
    public GameObject bunkerMenu;

    public GameObject upgradeUI;

    public Text[] cannonPriceText;
    public Text[] bunkerPriceText;
    public Text[] laserPriceText;

    public int lengthPerValue = 75;
    public int maxSliderValue = 375;
    public RectTransform[] cannonSliderRect;
    public RectTransform[] bunkerSliderRect;
    public RectTransform[] laserSliderRect;


    //Used to increase the value of the current modifier when upgrading. The operation should be addition
    public float[] statusIncrement = new float[3];

    //0 - range; 1 - damage; 2 - fireRate
    private int[] cannonCurrentUpgradePrice = new int[3];
    private int[] bunkerCurrentUpgradePrice = new int[3];
    private int[] laserCurrentUpgradePrice = new int[3];

    public const int startingPrice = 200;
    private int price = startingPrice;
    public int priceIncrement;

    private string turretName;
    private SceneDataRetainer data;

    private void Start()
    {
        data = SceneDataRetainer.GetInstance();

        //Set the current price for all menus;
        int dataLevel;
        for(int index = 0; index < 3; index++)
        {
            dataLevel = data.bunkerCurrentStatusLevel[index];
            if (data.bunkerCurrentStatusLevel[index] <= 4)
            {
                while (dataLevel > 0)
                {
                    price += priceIncrement;
                    dataLevel--;
                }
                bunkerCurrentUpgradePrice[index] = price;
                bunkerPriceText[index].text = "" + price;
                price = startingPrice;
            }
            else
                bunkerPriceText[index].text = "Max";

            if (data.bunkerCurrentStatusLevel[index] == 0)
            {
                bunkerSliderRect[index].localScale = new Vector3(0f, 1f, 1f);
            }
            else
            {
                float width = Mathf.Clamp(((float)lengthPerValue * data.bunkerCurrentStatusLevel[index]) / maxSliderValue, 0f, 1f);
                bunkerSliderRect[index].localScale = new Vector3(width, 1f, 1f);
            }

            if (index < 2)
            {
                dataLevel = data.laserCurrentStatusLevel[index];
                if (data.laserCurrentStatusLevel[index] <= 4)
                {
                    while (dataLevel > 0)
                    {
                        price += priceIncrement;
                        dataLevel--;
                    }
                    laserCurrentUpgradePrice[index] = price;
                    laserPriceText[index].text = "" + price;
                    price = startingPrice;
                }
                else
                    laserPriceText[index].text = "Max";

                if (data.laserCurrentStatusLevel[index] == 0)
                {
                    laserSliderRect[index].localScale = new Vector3(0f, 1f, 1f);
                }
                else
                {
                    float width = Mathf.Clamp(((float)lengthPerValue * data.laserCurrentStatusLevel[index]) / maxSliderValue, 0f, 1f);
                    laserSliderRect[index].localScale = new Vector3(width, 1f, 1f);
                }
            }


            dataLevel = data.cannonCurrentStatusLevel[index];
            if(data.cannonCurrentStatusLevel[index] <= 4)
            {
                while (dataLevel > 0)
                {
                    price += priceIncrement;
                    dataLevel--;
                }
                cannonCurrentUpgradePrice[index] = price;
                cannonPriceText[index].text = "" + price;
                price = startingPrice;
            }
            else
                cannonPriceText[index].text = "Max";

            if (data.cannonCurrentStatusLevel[index] == 0)
            {
                cannonSliderRect[index].localScale = new Vector3(0f, 1f, 1f);
            }
            else
            {
                float width = ((float)lengthPerValue * data.cannonCurrentStatusLevel[index]) / maxSliderValue;
                cannonSliderRect[index].localScale = new Vector3(width, 1f, 1f);
            }
        }
    }

    public void ActivateUI(bool status)
    {
        upgradeUI.SetActive(status);
    }
    
    public void Upgrade(string statusToUpgrade)
    {
        int statusIndex = 0;
        if (turretName == "laser")
        {
            if (statusToUpgrade == "damage")
                statusIndex = 1;
            else if (statusToUpgrade == "range")
                statusIndex = 0;

            if (laserCurrentUpgradePrice[statusIndex] <= data.permanentUpgradeMoney)
            {
                if (data.laserCurrentStatusLevel[statusIndex] <= 4)
                {
                    data.laserMultipliers[statusIndex] += statusIncrement[statusIndex];
                    data.permanentUpgradeMoney -= laserCurrentUpgradePrice[statusIndex];
                    laserCurrentUpgradePrice[statusIndex] += priceIncrement;
                    data.laserCurrentStatusLevel[statusIndex]++;

                    if (data.laserCurrentStatusLevel[statusIndex] == 5)
                        laserPriceText[statusIndex].text = "Max";
                    else
                        laserPriceText[statusIndex].text = "" + laserCurrentUpgradePrice[statusIndex];
                }

                if (data.laserCurrentStatusLevel[statusIndex] == 0)
                {
                    laserSliderRect[statusIndex].localScale = new Vector3(0f, 1f, 1f);
                }
                else
                {
                    float width = Mathf.Clamp(((float)lengthPerValue * data.laserCurrentStatusLevel[statusIndex]) / maxSliderValue, 0f, 1f);
                    laserSliderRect[statusIndex].localScale = new Vector3(width, 1f, 1f);
                }
            }
        }
        if (turretName == "bunker")
        {
            if (statusToUpgrade == "damage")
                statusIndex = 1;
            else if (statusToUpgrade == "range")
                statusIndex = 0;
            else if (statusToUpgrade == "firerate")
                statusIndex = 2;

            if (bunkerCurrentUpgradePrice[statusIndex] <= data.permanentUpgradeMoney)
            {
                if (data.bunkerCurrentStatusLevel[statusIndex] <= 4)
                {
                    data.bunkerMultipliers[statusIndex] += statusIncrement[statusIndex];
                    data.permanentUpgradeMoney -= bunkerCurrentUpgradePrice[statusIndex];
                    bunkerCurrentUpgradePrice[statusIndex] += priceIncrement;
                    data.bunkerCurrentStatusLevel[statusIndex]++;

                    if (data.bunkerCurrentStatusLevel[statusIndex] == 5)
                        bunkerPriceText[statusIndex].text = "Max";
                    else
                        bunkerPriceText[statusIndex].text = "" + bunkerCurrentUpgradePrice[statusIndex];
                }

                if (data.bunkerCurrentStatusLevel[statusIndex] == 0)
                {
                    bunkerSliderRect[statusIndex].localScale = new Vector3(0f, 1f, 1f);
                }
                else
                {
                    float width = Mathf.Clamp(((float)lengthPerValue * data.bunkerCurrentStatusLevel[statusIndex]) / maxSliderValue, 0f, 1f);
                    bunkerSliderRect[statusIndex].localScale = new Vector3(width, 1f, 1f);
                }
            }
        }
        if (turretName == "cannon")
        {
            if (statusToUpgrade == "damage")
                statusIndex = 1;
            else if (statusToUpgrade == "range")
            {
                statusIndex = 0;
            }
            else if (statusToUpgrade == "firerate")
                statusIndex = 2;

            if (cannonCurrentUpgradePrice[statusIndex] <= data.permanentUpgradeMoney)
            {
                if(data.cannonCurrentStatusLevel[statusIndex] <= 4)
                {
                    data.cannonMultipliers[statusIndex] += statusIncrement[statusIndex];
                    data.permanentUpgradeMoney -= cannonCurrentUpgradePrice[statusIndex];
                    cannonCurrentUpgradePrice[statusIndex] += priceIncrement;
                    data.cannonCurrentStatusLevel[statusIndex]++;

                    if (data.cannonCurrentStatusLevel[statusIndex] == 5)
                        cannonPriceText[statusIndex].text = "Max";
                    else
                        cannonPriceText[statusIndex].text = "" + cannonCurrentUpgradePrice[statusIndex];
                }

                    if (data.cannonCurrentStatusLevel[statusIndex] == 0)
                {
                    cannonSliderRect[statusIndex].localScale = new Vector3(0f, 1f, 1f);
                }
                else
                {
                    float width = Mathf.Clamp(((float)lengthPerValue * data.cannonCurrentStatusLevel[statusIndex]) / maxSliderValue, 0f, 1f);
                    cannonSliderRect[statusIndex].localScale = new Vector3(width, 1f, 1f);
                }
            }
        }
    }

    public void ActivateCanvas(string canvasName)
    {
        switch(canvasName)
        {
            case "cannon":
                cannonMenu.SetActive(true);
                laserMenu.SetActive(false);
                bunkerMenu.SetActive(false);
                turretName = "cannon";
                break;
            case "bunker":
                cannonMenu.SetActive(false);
                laserMenu.SetActive(false);
                bunkerMenu.SetActive(true);
                turretName = "bunker";
                break;
            case "laser":
                cannonMenu.SetActive(false);
                laserMenu.SetActive(true);
                bunkerMenu.SetActive(false);
                turretName = "laser";
                break;
            default:
                cannonMenu.SetActive(false);
                laserMenu.SetActive(false);
                bunkerMenu.SetActive(false);
                turretName = "";
                break;
        }
    }
}
