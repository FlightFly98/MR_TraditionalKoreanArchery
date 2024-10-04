using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Diagnostics;
using System;

public class CameraSwitchCinemachine : MonoBehaviour
{
    public static CameraSwitchCinemachine instance;
    public CinemachineVirtualCamera mainCamera;
    public CinemachineVirtualCamera followCamera;
    public CinemachineVirtualCamera targetCamera;
    public KeyCode switchKey = KeyCode.Space; // 전환에 사용할 키
    private bool isTargetHit = false;
    
    public enum arrowState
    {
        isFollowing,
        hitTarget,
        hitOther,
        main
    }

    public enum DistanceMode
    {
        M_145,
        M_50,
        M_30
    }

    void Awake()
    {
        instance = this;
    }

    void Update()
    {

    }

    public void PlaySwitchCamera(arrowState state, String otherTag)
    {
        switch(UIManager.instance.GetCameraMode())
        {
            case UIManager.CameraMode.ArrowTracking :
                if(otherTag == "Target")
                {
                    isTargetHit = true;
                }
                if(isTargetHit)
                {
                    StartCoroutine(SwitchCamera(state));
                }
                if(!isTargetHit)
                {
                    StartCoroutine(SwitchCamera(state));
                }
            break;

            case UIManager.CameraMode.Default:
            break;
        }
        
    }
    private IEnumerator SwitchCamera(arrowState state)
    {
        switch(state)
        {
            case arrowState.main:
                mainCamera.Priority = 10;
                followCamera.Priority = 5;
                targetCamera.Priority = 5;
                break;

            case arrowState.isFollowing:
                mainCamera.Priority = 5;
                followCamera.Priority = 10;
                targetCamera.Priority = 5;
                break;
            
            case arrowState.hitTarget:

                yield return new WaitForSecondsRealtime(0.1f);

                mainCamera.Priority = 5;
                followCamera.Priority = 5;
                targetCamera.Priority = 10;

                yield return new WaitForSecondsRealtime(3f);
                
                mainCamera.Priority = 10;
                followCamera.Priority = 5;
                targetCamera.Priority = 5;

                isTargetHit = false;
                break;

            case arrowState.hitOther:

                yield return new WaitForSecondsRealtime(3f);

                mainCamera.Priority = 10;
                followCamera.Priority = 5;
                targetCamera.Priority = 5;
                break;
        }
    }

    public void SetCameraPosition(DistanceMode distance)
    {
        switch(distance)
        {
            case DistanceMode.M_145:
            mainCamera.transform.position = new Vector3(0, Player.instance.transform.position.y, 70);
            followCamera.transform.position = Player.instance.transform.position;
            break;

            case DistanceMode.M_50:
            mainCamera.transform.position = new Vector3(Player.instance.transform.position.x, Player.instance.transform.position.y, Player.instance.transform.position.z);
            followCamera.transform.position = Player.instance.transform.position;
            break;

            case DistanceMode.M_30:
            mainCamera.transform.position = new Vector3(Player.instance.transform.position.x, Player.instance.transform.position.y, 130);
            followCamera.transform.position = Player.instance.transform.position;
            break;    
        }
    }
}
