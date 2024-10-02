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
    public static bool isHit = false; // 게임에서 화살이 어딘가 맞았을 때

    public float kkagjHandYaw; // 깍지손 손등 회전 측정
    public float zoomHandRoll; // 줌손 화살 각도 측정
    public float zoomHandPitch; // 줌손 팔꿈치 엎어짐 측정

    public float zoomHandYaw; // 줌손 팔꿈치 엎어짐 측정

    public float startTime = 0.0f;   // 발시 순간 측정 시작
    public float endTime = 0.0f;     // 충격 센서 작동 측정 끝 시간
    public bool isPressureHigh = false;  // 압력 센서를 눌렀는지 확인
    public bool isImpactDetected = false; // 현실에서 충격 센서 감지 확인
    public float limitDistance = 7; // 궁사 ~ 천까지 거리

    // 활 & 화살 정보(Rssi)

    public float poundForce; // 파운드
    public float fullLength; // 만작 길이
    public float pulledDistance; // 당긴 길이
    public float bowK; // 활 탄성 계수

    public void SetGame()
    {
        SetArrowWeight(UIManager.instance.arrowWeightInfo);
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
        if(!isPressureHigh && isImpactDetected)
        {
            endTime = Time.time;
            float elapsedTime = endTime - startTime;
            initialVelocity = limitDistance / elapsedTime;

            Debug.Log("InitialVelocity: " + initialVelocity);
            Debug.Log("elapsedTime: " + elapsedTime);

            startTime = 0;
            endTime = 0;

            Debug.Log(startTime + ", " + endTime);

            isImpactDetected = false;
        }
    }

    public void SetInitialVelocityRssi()
    {
        //SetPulledLength(distance);
        SetBowK();

        initialVelocity =
            Mathf.Sqrt((GetBowK() * Mathf.Pow(GetPulledLength(), 2)) / GetArrowWeight());
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
        float pound = 0f;
        pound = float.Parse(inputPound);
        poundForce = (pound * 0.453592f) * 9.81f;
        //Debug.Log("PoundForce: " + poundForce);
    }

    public float GetFullLength()
    {
        return fullLength;
    }
    public void SetFullLength(string inputBowType)
    {
        switch(inputBowType)
        {
            case "특장":
                fullLength = 2.85f * 0.303030f; // 자 -> m
                Debug.Log("fullLength: " + fullLength);
                break;
            case "장장":
                fullLength = 2.65f * 0.303030f;
                Debug.Log("fullLength: " + fullLength);
                break;
            default:
                fullLength = 0.81f;
                break;
        }
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