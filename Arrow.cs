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

        // 공기 저항력 계산
        Vector3 dragForce = -0.5f * airDensity * speed * speed * dragCoefficient * crossSectionalArea * velocity.normalized;

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

    void FixedUpdate()
    {
        if (!GameManager.isHit && !isThisHit)
        {
            ArrowCalPosition();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Arrow")
        {
            isThisHit = true;
            velocity = Vector3.zero;
            Debug.Log(other.tag);
        }
    }
}
