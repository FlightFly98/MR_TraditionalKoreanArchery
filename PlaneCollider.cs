using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneCollider : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Arrow")
        {
            Debug.Log("on");
            GameManager.isHit = true;

           // Destroy(other.gameObject, 2.5f);
        }
    }
}
