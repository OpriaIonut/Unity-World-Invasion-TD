using UnityEngine;
using UnityEngine.UI;

public class PermanentUpgradeMenu : MonoBehaviour {

    public GameObject cannonMenu;
    public GameObject laserMenu;
    public GameObject bunkerMenu;

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
    private int price;
    public int priceIncrement;

    private string turretName;
    private SceneDataRetainer data;

    private void Start()
    {
        data = SceneDataRetainer.instance;

        //Set the current price for all menus;
        int dataLevel;
        for(int index = 0; index < 3; index++)
        {
            dataLevel = data.bunkerCurrentStatusLevel[index];
            while(dataLevel > 0)
            {
                price += priceIncrement;
                dataLevel--;
            }
            bunkerCurrentUpgradePrice[index] = price;
            bunkerPriceText[index].text = "" + price;
            price = startingPrice;

            if (data.bunkerCurrentStatusLevel[index] == 0)
            {
                bunkerSliderRect[index].localScale = new Vector3(0f, 1f, 1f);
            }
            else
            {
                bunkerSliderRect[index].localScale = new Vector3(maxSliderValue / (lengthPerValue * data.bunkerCurrentStatusLevel[index]), 1f, 1f);
            }
            //bunkerSliderRect[index].rect = new Rect(bunkerSliderRect[index].rect.x,
            //                                   bunkerSliderRect[index].rect.y,
            //                                   lengthPerValue * data.bunkerCurrentStatusLevel[index],
            //                                   bunkerSliderRect[index].rect.height);

            if (index < 2)
            {
                dataLevel = data.laserCurrentStatusLevel[index];
                while (dataLevel > 0)
                {
                    price += priceIncrement;
                    dataLevel--;
                }
                laserCurrentUpgradePrice[index] = price;
                laserPriceText[index].text = "" + price;
                price = startingPrice;

                if (data.bunkerCurrentStatusLevel[index] == 0)
                {
                    laserSliderRect[index].localScale = new Vector3(0f, 1f, 1f);
                }
                else
                {
                    laserSliderRect[index].localScale = new Vector3(maxSliderValue / (lengthPerValue * data.laserCurrentStatusLevel[index]), 1f, 1f);
                }
                //laserSliderRect[index] = new Rect(laserSliderRect[index].x,
                //                                   laserSliderRect[index].y,
                //                                   lengthPerValue * data.laserCurrentStatusLevel[index],
                //                                   laserSliderRect[index].height);
            }


            dataLevel = data.cannonCurrentStatusLevel[index];
            while (dataLevel > 0)
            {
                price += priceIncrement;
                dataLevel--;
            }
            cannonCurrentUpgradePrice[index] = price;
            cannonPriceText[index].text = "" + price;
            price = startingPrice;

            if (data.bunkerCurrentStatusLevel[index] == 0)
            {
                cannonSliderRect[index].localScale = new Vector3(0f, 1f, 1f);
            }
            else
            {
                cannonSliderRect[index].localScale = new Vector3(maxSliderValue / (lengthPerValue * data.cannonCurrentStatusLevel[index]), 1f, 1f);
            }
            //cannonSliderRect[index] = new Rect(cannonSliderRect[index].x,
            //                                   cannonSliderRect[index].y,
            //                                   lengthPerValue * data.cannonCurrentStatusLevel[index],
            //                                   cannonSliderRect[index].height);
        }
    }
    
    public void Upgrade(string statusToUpgrade)
    {
        int statusIndex = 0;
        switch(turretName)
        {
            case "laser":
                switch(statusToUpgrade)
                {
                    case "damage":
                        statusIndex = 1;
                        break;
                    case "range":
                        statusIndex = 0;
                        break;
                }

                if(laserCurrentUpgradePrice[statusIndex] <= data.permanentUpgradeMoney)
                {
                    data.laserMultipliers[statusIndex] += statusIncrement[statusIndex];
                    data.permanentUpgradeMoney -= laserCurrentUpgradePrice[statusIndex];
                    laserCurrentUpgradePrice[statusIndex] += priceIncrement;
                    data.laserCurrentStatusLevel[statusIndex]++;

                    laserPriceText[statusIndex].text = "" + laserCurrentUpgradePrice[statusIndex];
                    if (data.bunkerCurrentStatusLevel[statusIndex] == 0)
                    {
                        laserSliderRect[statusIndex].localScale = new Vector3(0f, 1f, 1f);
                    }
                    else
                    {
                        laserSliderRect[statusIndex].localScale = new Vector3(maxSliderValue / (lengthPerValue * data.laserCurrentStatusLevel[statusIndex]), 1f, 1f);
                    }
                    //laserSliderRect[statusIndex] = new Rect(laserSliderRect[statusIndex].x,
                    //                                   laserSliderRect[statusIndex].y,
                    //                                   lengthPerValue * data.laserCurrentStatusLevel[statusIndex],
                    //                                   laserSliderRect[statusIndex].height);
                }
                break;
            case "bunker":
                switch (statusToUpgrade)
                {
                    case "damage":
                        statusIndex = 1;
                        break;
                    case "range":
                        statusIndex = 0;
                        break;
                    case "firerate":
                        statusIndex = 2;
                        break;
                }

                if (bunkerCurrentUpgradePrice[statusIndex] <= data.permanentUpgradeMoney)
                {
                    data.bunkerMultipliers[statusIndex] += statusIncrement[statusIndex];
                    data.permanentUpgradeMoney -= bunkerCurrentUpgradePrice[statusIndex];
                    bunkerCurrentUpgradePrice[statusIndex] += priceIncrement;
                    data.bunkerCurrentStatusLevel[statusIndex]++;

                    bunkerPriceText[statusIndex].text = "" + bunkerCurrentUpgradePrice[statusIndex];
                    if (data.bunkerCurrentStatusLevel[statusIndex] == 0)
                    {
                        bunkerSliderRect[statusIndex].localScale = new Vector3(0f, 1f, 1f);
                    }
                    else
                    {
                        bunkerSliderRect[statusIndex].localScale = new Vector3(maxSliderValue / (lengthPerValue * data.bunkerCurrentStatusLevel[statusIndex]), 1f, 1f);
                    }
                    //bunkerSliderRect[statusIndex] = new Rect(bunkerSliderRect[statusIndex].x,
                    //                                   bunkerSliderRect[statusIndex].y,
                    //                                   lengthPerValue * data.bunkerCurrentStatusLevel[statusIndex],
                    //                                   bunkerSliderRect[statusIndex].height);
                }
                break;
            case "cannon":
                switch (statusToUpgrade)
                {
                    case "damage":
                        statusIndex = 1;
                        break;
                    case "range":
                        statusIndex = 0;
                        break;
                    case "firerate":
                        statusIndex = 2;
                        break;
                }

                if (cannonCurrentUpgradePrice[statusIndex] <= data.permanentUpgradeMoney)
                {
                    data.cannonMultipliers[statusIndex] += statusIncrement[statusIndex];
                    data.permanentUpgradeMoney -= cannonCurrentUpgradePrice[statusIndex];
                    cannonCurrentUpgradePrice[statusIndex] += priceIncrement;
                    data.cannonCurrentStatusLevel[statusIndex]++;

                    cannonPriceText[statusIndex].text = "" + cannonCurrentUpgradePrice[statusIndex];
                    if (data.bunkerCurrentStatusLevel[statusIndex] == 0)
                    {
                        cannonSliderRect[statusIndex].localScale = new Vector3(0f, 1f, 1f);
                    }
                    else
                    {
                        Debug.Log("Here");
                        cannonSliderRect[statusIndex].localScale = new Vector3((lengthPerValue * data.cannonCurrentStatusLevel[statusIndex]) / maxSliderValue, 1f, 1f);
                    }
                    //cannonSliderRect[statusIndex] = new Rect(cannonSliderRect[statusIndex].x,
                    //                                   cannonSliderRect[statusIndex].y,
                    //                                   lengthPerValue * data.cannonCurrentStatusLevel[statusIndex],
                    //                                   cannonSliderRect[statusIndex].height);
                }
                break;
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
