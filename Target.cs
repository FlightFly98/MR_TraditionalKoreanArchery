using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    public static Target instance;

    string thisTargetName;
    int thisTargetNum;
    public TLight TL;

    void Start()
    {
        instance = this;
        thisTargetName = this.gameObject.name;
        thisTargetNum = int.Parse(thisTargetName.Substring(thisTargetName.IndexOf('_') + 1));
    }
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Arrow")
        {
            CameraSwitchCinemachine.instance.PlaySwitchCamera(CameraSwitchCinemachine.arrowState.hitTarget, other.tag);
            InGameUI.instance.SetHitCount();
            StartCoroutine(TL.BlinkTLight(thisTargetNum));
        }
    }
}

