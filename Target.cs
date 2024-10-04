using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    public static Target instance;
    int arrowHashCode = 0;

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
            GameManager.isHit = true;

            arrowHashCode = other.GetHashCode();

            CameraSwitchCinemachine.instance.PlaySwitchCamera(CameraSwitchCinemachine.arrowState.hitTarget, other.tag);
            StartCoroutine(TL.BlinkTLight(thisTargetNum));

        }
    }
}

