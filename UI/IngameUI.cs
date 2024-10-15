using UnityEngine;
using UnityEngine.UI;

public class InGameUI : UIBase
{
    public static InGameUI instance;
    public Canvas inGameUICanvas;
    public Text hitTargetCountText;
    public Text targetDistanceText;
    public GameObject LaunchTimeText;
    public Text soonText;
    public Text siText;
    int hitCount = 0;
    int siCount = 0;
    int soonCount = 1;
    
    void Awake()
    {
        instance = this;
    }

    public void SetTargetDistanceText(int distanceNum)
    {
        targetDistanceText.text = distanceNum.ToString() + "m";
    }

    public void SetLaunchTimeText(bool active)
    {
        LaunchTimeText.SetActive(active);
    }
    
    public void SetHitCount()
    {
        hitCount++;
        hitTargetCountText.text = hitCount.ToString() + "중";
    }

    public void SetSoonText(bool reset)
    {
        if(!reset)
        {
            siCount++;
            siText.text = siCount.ToString() + "시";
            if(siCount == 5)
            {
                soonCount += 1;
                soonText.text = soonCount.ToString() + "순";
                siCount = 0;
                siText.text = siCount.ToString() + "시";
            }
        }
        else
        {
            soonCount = 1;
            soonText.text = soonCount.ToString() + "순";
            siCount = 0;
            siText.text = siCount.ToString() + "시";
        }
    }   
}