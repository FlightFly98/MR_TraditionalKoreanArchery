using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public float arrowWeight = 0.028125f; // 화살의 무게
    public float initialVelocity; // 화살 초기 속도
    public float launchAngle; // 화살 초기 각도

    public float kkagjHandYaw; // 깍지손 손등 회전 측정
    public float zoomHandPitch; // 줌손 팔꿈치 엎어짐 측정
    public float zoomHandRoll; // 줌손 화살 각도 측정
    public float zoomHandYaw; // 줌손 화살 좌우 각도 측정

    public float startTime = 0.0f;   // 발시 순간 측정 시작
    public float endTime = 0.0f;     // 충격 센서 작동 측정 끝 시간
    public float elapsedTime;
    public float limitDistance = 5; // 궁사 ~ 천까지 거리
    public bool isPressureHigh = false;  // 압력 센서를 눌렀는지 확인
    public bool isImpactDetected = false; // 현실에서 충격 센서 감지 확인

    // 활 & 화살 정보(Rssi)

    public float poundForce = 15.0f; // 파운드
    public float fullLength = 2.85f * 0.303030f; // 만작 길이
    public float pulledDistance; // 당긴 길이
    public float bowK; // 활 탄성 계수
    public bool checkDistanceMode = true;

    public Vector3 windDirection = new Vector3(0, 0, 0);
    public float windStrength = 5.0f;   

    public void SetGame()
    {
        SetPoundForce(StartUI.instance.poundInfo);

        SetArrowWeight(StartUI.instance.arrowWeightInfo);

        SetFullLength(StartUI.instance.arrowLengthInfo);

        SetBowK();
    }
    public float GetArrowWeight() { return arrowWeight; }
    public void SetArrowWeight(string inputArrow) 
    { 
        arrowWeight = float.Parse(inputArrow) * 0.00375f; // 돈 -> kg
    }
    public float GetInitialVelocity()
    {
        return initialVelocity;
    }
    public void SetInitialVelocity()
    {
        if(checkDistanceMode)
        {
            if(!isPressureHigh && isImpactDetected)
            {
                endTime = Time.time;
                elapsedTime = endTime - startTime;
                initialVelocity = limitDistance / elapsedTime;

                Debug.Log("InitialVelocity: " + initialVelocity);
                Debug.Log("elapsedTime: " + elapsedTime);

                startTime = 0;
                endTime = 0;

                Debug.Log(startTime + ", " + endTime);

                isImpactDetected = false;
            }
        }
    }

    public void SetInitialVelocityRssi()
    {
        if(!checkDistanceMode)
        {
            initialVelocity =
                Mathf.Sqrt((GetBowK() * Mathf.Pow(GetPulledLength(), 2)) / GetArrowWeight());
        }
    }

    public float GetlaunchAngle()
    {
        return launchAngle;
    }
    public void SetlaunchAngle()
    {
        launchAngle = zoomHandRoll; // 각도 설정
    }
    public float GetZoomHandPitch() { return zoomHandPitch; }

    public void SetZoomHandPitch(float pitch) 
    { 
        zoomHandPitch = pitch;
    }

    public float GetZoomHandRoll() { return zoomHandRoll; }

    public void SetZoomHandRoll(float Roll) 
    { 
        zoomHandRoll = Roll;
    }

    public float GetZoomHandYaw() { return zoomHandYaw; }

    public void SetZoomHandYaw(float yaw) 
    { 
        zoomHandYaw = yaw;
    }

    public float GetkkagjHandYaw() { return kkagjHandYaw; }

    public void SetkkagjHandYaw(float yaw)
    {
        kkagjHandYaw = yaw;
    }

    public float GetPoundForce()
    {
        return poundForce;
    }
    public void SetPoundForce(string inputPound)
    {
        float pound = float.Parse(inputPound);
        poundForce = (pound * 0.453592f) * 9.81f;
        //Debug.Log("PoundForce: " + poundForce);
    }

    public float GetFullLength()
    {
        return fullLength;
    }
    public void SetFullLength(string inputLength)
    {
        float arrowLength = float.Parse(inputLength);
        fullLength = arrowLength * 0.303030f;
    }
    public float GetPulledLength()
    {
        return pulledDistance;
    }
    public void SetPulledLength(float distance)
    {
        pulledDistance = distance;
        //Debug.Log("pulledDistance: " + pulledDistance);
    }
    public float GetBowK()
    {
        return bowK;
    }
    public void SetBowK()
    {
        bowK = poundForce / fullLength; // K = F / x
        //Debug.Log("bowK: " + bowK);
    }
}