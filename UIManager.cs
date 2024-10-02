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
    public enum arrowMode
    {
        ArrowTracking,
        Default
    }

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