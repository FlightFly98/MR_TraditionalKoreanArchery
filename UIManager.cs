using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;
    public Canvas inGameCanvas;

    public bool checkInput = false;
    public TMP_InputField arrowWeightInputField;
    public string arrowWeightInfo;

    public GameObject menuPanel;
    private bool isMenuActive = false;
    public Button dayButton;
    public Button nightButton; 
    public Light sceneLight; 
    public Material daySkybox;
    public Material nightSkybox;
    public Light[] nightSpotLight;

    public Text hitTargetCountText;
    int hitCount = 0;

    // 신호등
    public GameObject[] TLights;
    private Color originColor;
    Color tempColor;
    private int targetNumber; // 관 숫자

    void Awake()
    {
        instance = this;
    }

    public void SettingInputField()
    {
        arrowWeightInfo = arrowWeightInputField.GetComponent<TMP_InputField>().text;

        checkInput = true;
    }
    public void SetHitCount()
    {
        hitCount++;
        hitTargetCountText.text = hitCount.ToString();
    }

    public void StartMainScene()
    { 
       SceneManager.LoadScene(1);
    }

    public void SetTLights()
    {
        TLights = GameObject.FindGameObjectsWithTag("TLight");
        String TLName = "TL";
        for(int i = 0; i < 3; i++)
        {
            String checkTLName = TLName + (i + 1).ToString();
            GameObject currentTL = TLights[i];
            if(currentTL.gameObject.name != checkTLName)
            {
                for(int j = 0; j < 3; j++)
                {
                    if(checkTLName == TLights[j].gameObject.name)
                    {
                        GameObject tmp = TLights[i];
                        TLights[i] = TLights[j];
                        TLights[j] = tmp;
                    }
                }
            }
        }
        for(int i = 0; i < 3; i++) { originColor = TLights[i].GetComponent<Renderer>().material.color; }
    }
    public void SetBlinkTLight(int num)
    {
        targetNumber = num - 1;
        StartCoroutine(BlinkTLight());
    }
    IEnumerator BlinkTLight()
    {
        TLights[targetNumber].GetComponent<Renderer>().material.color = new Color(1, 0, 0, 1);
        tempColor = TLights[targetNumber].GetComponent<Renderer>().material.color;

        int count = 0;
        while (count < 2)
        {
            while (TLights[targetNumber].GetComponent<Renderer>().material.color.r > 0f)
            {
                tempColor.r -= 0.1f;
                TLights[targetNumber].GetComponent<Renderer>().material.color = tempColor;
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(0.5f);

            while (TLights[targetNumber].GetComponent<Renderer>().material.color.r < 1f)
            {
                tempColor.r += 0.1f;
                TLights[targetNumber].GetComponent<Renderer>().material.color = tempColor;
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(0.5f);
            count++;
        }

        TLights[targetNumber].GetComponent<Renderer>().material.color = originColor;
    }

     void ToggleMenu()
    {
        isMenuActive = !isMenuActive;
        menuPanel.SetActive(isMenuActive);
        dayButton.onClick.AddListener(SetDayMode);
        nightButton.onClick.AddListener(SetNightMode);

        // 메뉴가 활성화되면 마우스 커서 활성화
        if (isMenuActive)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

       void SetDayMode()
    {
        sceneLight.color = Color.white;     // 조명의 색상을 밝은 흰색으로 설정
        RenderSettings.ambientLight = Color.white; // 씬의 전체 조명 색상을 흰색으로
        RenderSettings.skybox = daySkybox;  // 주간 Skybox 적용
        hitTargetCountText.color = Color.black;
        //Debug.Log("주간 모드 활성화");
    }

    // 야간 모드 설정
    void SetNightMode()
    {
        sceneLight.color = Color.blue;      // 조명의 색상을 어두운 파란색으로 설정
        RenderSettings.ambientLight = Color.gray; // 씬의 전체 조명 색상을 회색으로
        RenderSettings.skybox = nightSkybox; // 야간 Skybox 적용
        hitTargetCountText.color = Color.white;
        //Debug.Log("야간 모드 활성화");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleMenu();
        }
    }
}