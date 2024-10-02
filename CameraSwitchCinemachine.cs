using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraSwitchCinemachine : MonoBehaviour
{
    public static CameraSwitchCinemachine instance;
    public CinemachineVirtualCamera mainCamera;
    public CinemachineVirtualCamera followCamera;
    public KeyCode switchKey = KeyCode.Space; // 전환에 사용할 키
    
    private bool isFollowing = false;
    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(switchKey))
        {
            SwitchCamera();
        }
    }

    public void SwitchCamera()
    {
        isFollowing = !isFollowing;

        if (isFollowing)
        {
            mainCamera.Priority = 5;
            followCamera.Priority = 10;
        }
        else
        {
            mainCamera.Priority = 10;
            followCamera.Priority = 5;
        }
    }
}
