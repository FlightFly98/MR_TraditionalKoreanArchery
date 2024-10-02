using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Target : MonoBehaviour
{
    int arrowHashCode = 0;

    string thisTargetName;
    int thisTargetNum;

    void Start()
    {
        thisTargetName = this.gameObject.name;
        thisTargetNum = int.Parse(thisTargetName.Substring(thisTargetName.IndexOf('_') + 1));
    }
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Arrow") // && arrowHashCode != other.GetHashCode())
        {
            GameManager.isHit = true;

            arrowHashCode = other.GetHashCode();

            //Rigidbody rb = other.gameObject.GetComponent<Rigidbody>();
            //rb.constraints = RigidbodyConstraints.FreezeAll;
            //rb.isKinematic = true;

            StartCoroutine(CameraSwitchCinemachine.instance.SwitchCamera(CameraSwitchCinemachine.arrowState.hitTarget, thisTargetNum));
            StartCoroutine(TLight.instance.BlinkTLight(thisTargetNum));
            //Debug.Log("관중");
        }

        //Debug.Log(other.tag);
    }
}

