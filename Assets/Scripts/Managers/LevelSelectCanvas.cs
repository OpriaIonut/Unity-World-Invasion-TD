using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class StarsArray
{
    public GameObject[] star;
}

public class LevelSelectCanvas : MonoBehaviour {

    public Text moneyText;
    public StarsArray[] stars;

    private SceneDataRetainer data;

    private float lasuUpdateTime = 0f;

    private void Start()
    {
        data = SceneDataRetainer.GetInstance();
        for(int index = 0; index < data.starsPerLevel.Length; index++)
        {
            if (index < stars.Length)
            {
                int numOfStars = data.starsPerLevel[index];
                for(int index2 = 0; index2 < 3; index2++)
                {
                    if(index2 <= numOfStars - 1)
                        stars[index].star[index2].SetActive(true);
                    else
                        stars[index].star[index2].SetActive(false);
                }
            }
        }
    }

    private void Update()
    {
        if(Time.time - lasuUpdateTime > 0.5f)
        {
            moneyText.text = "$" + data.permanentUpgradeMoney;
        }
    }
}
