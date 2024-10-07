using UnityEngine;
using UnityEngine.UI;

public class InGameUI : UIBase
{
    public static InGameUI instance;
    public Canvas inGameUICanvas;
    public Text hitTargetCountText;
    int hitCount = 0;
    
    void Awake()
    {
        instance = this;
    }
    
    public void SetHitCount()
    {
        hitCount++;
        hitTargetCountText.text = hitCount.ToString();
    }
}