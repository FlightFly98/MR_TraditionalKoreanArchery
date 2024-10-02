using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Diagnostics;

public class CameraSwitchCinemachine : MonoBehaviour
{
    public static CameraSwitchCinemachine instance;
    public CinemachineVirtualCamera mainCamera;
    public CinemachineVirtualCamera followCamera;
    public CinemachineVirtualCamera targetCamera;
    public KeyCode switchKey = KeyCode.Space; // 전환에 사용할 키
    
    public enum arrowState
    {
        isFollowing,
        hitTarget,
        hitPlane,
        main
    }

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        //if (Input.GetKeyDown(switchKey))
        //{
        //    SwitchCamera();
        //}
    }

    public IEnumerator SwitchCamera(arrowState state, int targetNum)
    {
        switch(state)
        {
            case arrowState.isFollowing:
                mainCamera.Priority = 5;
                followCamera.Priority = 10;
                targetCamera.Priority = 5;
                break;
            
            case arrowState.hitTarget:

                UIManager.instance.SetHitCount();

                yield return new WaitForSecondsRealtime(3f);

                mainCamera.Priority = 5;
                followCamera.Priority = 5;
                targetCamera.Priority = 10;

                yield return new WaitForSecondsRealtime(3f);
                
                mainCamera.Priority = 10;
                followCamera.Priority = 5;
                targetCamera.Priority = 5;

                //UIManager.instance.SetBlinkTLight(targetNum);

                break;

            case arrowState.main:

                yield return new WaitForSecondsRealtime(3f);

                mainCamera.Priority = 10;
                followCamera.Priority = 5;
                targetCamera.Priority = 5;
                break;
        }
    }
}
