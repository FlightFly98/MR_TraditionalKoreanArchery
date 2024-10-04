using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    
    // public Rigidbody body;
    bool isThisHit = false;
    private float ID;
    float initialSpeed;
    float angleDeg;
    float angleRad;
    float kkagjHandYaw = 0;
    float zoomHandRoll;
    float elapsedTime = 0.0f;

    private Vector3 velocity;
    private const float airDensity = 1.225f; // 공기의 밀도 (kg/m³)
    private const float dragCoefficient = 0.25f; // 공기 저항 계수 (화살의 경우 약 0.1 ~ 0.3)
    private float crossSectionalArea; // 단면적 (m²)
    float arrowMass = 0.028125f; // 화살의 질량 (kg)

    public GameObject markerPrefab;

    

    void Start()
    {
        ID = this.gameObject.GetInstanceID();

        // 발사체의 단면적 계산 (예: 화살의 직경이 0.01 m)
        float diameter = 0.01f; // 화살의 직경 (m)
        crossSectionalArea = Mathf.PI * Mathf.Pow(diameter / 2f, 2f);

        arrowMass = GameManager.instance.GetArrowWeight();

        initialSpeed = GameManager.instance.GetInitialVelocity();
        angleDeg = GameManager.instance.GetlaunchAngle();
        angleRad = GameManager.instance.GetlaunchAngle() * Mathf.Deg2Rad;

        //zoomHandRoll = GameManager.instance.GetZoomHandYaw();
        kkagjHandYaw = GameManager.instance.GetkkagjHandYaw();
        float rotationRad = kkagjHandYaw * Mathf.Deg2Rad;

        Quaternion pitchRotation = Quaternion.AngleAxis(angleDeg, Vector3.right);
        //Quaternion yawRotation = Quaternion.AngleAxis(kkagjHandYaw, Vector3.up);

        transform.rotation = pitchRotation;
        //transform.rotation = yawRotation * pitchRotation;

        // Debug.Log("V0: " + v0 + " A0: " + a0);
        velocity = new Vector3(
            initialSpeed * Mathf.Cos(angleRad) * Mathf.Sin(rotationRad),
            initialSpeed * Mathf.Sin(angleRad),
            initialSpeed * Mathf.Cos(angleRad) * Mathf.Cos(rotationRad)
            //initialSpeed / (Mathf.Cos(angleRad) * Mathf.Cos(rotationRad));
        );
    }
    void ArrowCalPosition()
    {
        // 속도 크기 계산
        float speed = velocity.magnitude;

        // 바람의 상대 속도 계산 (relative wind velocity = wind speed - arrow velocity)
        Vector3 relativeWindVelocity = GameManager.instance.windDirection.normalized * GameManager.instance.windStrength - velocity;

        // 바람의 상대 속도 크기 계산
        float relativeWindSpeed = relativeWindVelocity.magnitude;

        // 공기 저항력 계산
        Vector3 dragForce = -0.5f * airDensity * dragCoefficient * crossSectionalArea * relativeWindSpeed * relativeWindSpeed * relativeWindVelocity.normalized;
        //Vector3 dragForce = -0.5f * airDensity * speed * speed * dragCoefficient * crossSectionalArea * velocity.normalized;

        // 가속도 계산
        Vector3 gravityVector = new Vector3(0f, Physics.gravity.y, 0f);
        Vector3 acceleration = gravityVector + (dragForce / arrowMass);

        // 속도 업데이트
        velocity += acceleration * Time.fixedDeltaTime;

        // 위치 업데이트
        transform.position += velocity * Time.fixedDeltaTime;

        // 회전 업데이트
        if (velocity != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(velocity);
        }
    }
    /*
    void ArrowCalVelocity()
    {
        velocity.z = initialSpeed * Mathf.Cos(angleRad) * Time.fixedDeltaTime;
        velocity.y = initialSpeed * Mathf.Sin(angleRad) * Physics.gravity.y * Time.fixedDeltaTime;

        transform.Translate(velocity);
        transform.Rotate(new Vector3(Mathf.Cos(angleRad), 0, 0));

        if (transform.position.y < 0)
        {
            Debug.Log("Time to hit the ground: " + elapsedTime + " seconds");
            Debug.Log("얼마나 날아갔는가?: " + transform.position.z);
            isThisHit = true;
        }
    }
    */

    void FixedUpdate()
    {
        if (!isThisHit)
        {
            ArrowCalPosition();
        }
    }

    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.tag);

        if(other.tag != "Arrow")
        {
            if (other.CompareTag("Target"))
            {
                Vector3 hitPoint = other.ClosestPointOnBounds(transform.position);
                
                StartCoroutine(CreateMarker(hitPoint, other));

                 // 과녁의 표면 법선 벡터 계산
                Vector3 targetNormal = -other.transform.forward;

                // 일정 범위 내에서 과녁 앞쪽으로만 랜덤한 방향 생성
                Vector3 randomDirection = GetRandomDirectionWithinAngle(targetNormal, 45f); // 법선을 기준으로 45도 범위 안에서 생성

                float bounceMultiplier = 0.5f; // 튕겨나가는 힘을 더 크게 설정
                velocity = randomDirection * velocity.magnitude * bounceMultiplier;

                transform.rotation = Quaternion.LookRotation(velocity);

                Debug.Log("randomDirection: " + randomDirection + " | Velocity: " + velocity);
                
            }
            if(other.tag != "Target")
            {
                isThisHit = true;
                CameraSwitchCinemachine.instance.PlaySwitchCamera(CameraSwitchCinemachine.arrowState.hitOther, other.tag);
            }
        }
    }
    Vector3 GetRandomDirectionWithinAngle(Vector3 normal, float maxAngle)
    {
        Quaternion rotation = Quaternion.AngleAxis(Random.Range(-maxAngle, maxAngle), Vector3.up); // 좌우 방향으로 랜덤 회전
        Vector3 randomDirection = rotation * normal;

        Quaternion tiltRotation = Quaternion.AngleAxis(Random.Range(-maxAngle / 2f, maxAngle / 2f), Vector3.right);
        randomDirection = tiltRotation * randomDirection;

        return randomDirection.normalized;
    }

    public IEnumerator CreateMarker(Vector3 point, Collider other)
    {
        GameObject marker = Instantiate(markerPrefab, point, Quaternion.identity);

        // 마커를 과녁에 맞추도록 조정
        float Ztemp = marker.transform.position.z - other.transform.position.z;
        marker.transform.position = new Vector3(marker.transform.position.x, marker.transform.position.y, marker.transform.position.z - Ztemp - 0.05f);

        yield return new WaitForSecondsRealtime(6f);

        marker.SetActive(false);
    }
    
}
