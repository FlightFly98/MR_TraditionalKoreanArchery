using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Cinemachine;

public class Player : MonoBehaviour
{
    public static Player instance;

    public GameObject arrow;

    public UnityEvent arrowCreated;

    public bool shock = false;
    public bool valSi = false;

    public bool nakJeon = false;
    public CinemachineVirtualCamera followCamera; // 추적 카메라

    void Awake()
    {
        instance = this;
    }
    void Start()
    {

    }

    public void Shoot()
    {
        GameManager.isHit = false;
        GameObject newArrow = Instantiate(arrow, instance.transform);
        if (followCamera != null)
        {
            followCamera.Follow = newArrow.transform;
            followCamera.LookAt = newArrow.transform;
        }
        StartCoroutine(CameraSwitchCinemachine.instance.SwitchCamera(CameraSwitchCinemachine.arrowState.isFollowing, 0));
    }
    void Update()
    {
        if(nakJeon)
        {
            Debug.Log("NakJeon detected!");
        }

        if(shock && valSi && !nakJeon)
        {
            Shoot();
            shock = false;
            valSi = false;
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
            valSi = false;
        }
    }

    void SetNakJeonFalse()
    {
        nakJeon = false;
    }
}
