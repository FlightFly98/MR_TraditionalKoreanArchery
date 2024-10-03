using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindVane : MonoBehaviour
{
    public Transform windPole;

    private Vector3 initialRotation;

    void Start()
    {
        if (windPole != null)
        {
            initialRotation = windPole.localRotation.eulerAngles;
        }
    }

    void Update()
    {
        UpdateWindDirection();
        UpdateWindTilt();
    }

    // 바람의 방향에 따라 기둥의 회전 설정
    void UpdateWindDirection()
    {
        if (windPole != null)
        {
            // 바람의 방향을 Y축 기준으로 회전하도록 설정 (XZ 평면상)
            Vector3 projectedDirection = new Vector3(GameManager.instance.windDirection.x, 0, GameManager.instance.windDirection.z).normalized;

            if (projectedDirection != Vector3.zero)
            {
                // 바람의 방향을 기준으로 기둥의 회전을 Y축 기준으로 설정
                windPole.rotation = Quaternion.LookRotation(projectedDirection, Vector3.up);
            }
        }
    }

    // 바람의 세기에 따라 기둥의 기울기 설정 (Scale은 유지)
    void UpdateWindTilt()
    {
        if (windPole != null)
        {
            // 바람의 세기에 따라 기둥의 기울기 변경 (바람이 강할수록 더 많이 기울어짐)
            float tiltAngle = Mathf.Clamp(GameManager.instance.windStrength * 5, 0, 45);  // 최대 기울기 각도를 45도까지 제한
            windPole.localRotation = Quaternion.Euler(-tiltAngle, windPole.localRotation.eulerAngles.y, initialRotation.z);
        }
    }
}

