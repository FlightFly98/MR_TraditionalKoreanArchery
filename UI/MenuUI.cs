using UnityEngine;
using UnityEngine.UI;

public class MenuUI : UIBase
{
    public static MenuUI instance;
    public Canvas menuUICanvas;
    public GameObject menuPanel;
    private bool isMenuActive = false;
    public Button dayButton;
    public Button nightButton;
    public Button Btn_145m;
    public Button Btn_50m;
    public Button Btn_30m;
    public Button DefaultBtn;
    public Button arrowTrackingBtn;
    public Button mainBtn;
    public Button seokHoJungBtn;
    public Button cyberBtn;
    public Button practiceBtn;
    public Button competitionBtn;
    public Button resetBtn;
    public Light sceneLight; 
    public Material daySkybox;
    public Material nightSkybox;
    public GameObject nightSpotLight;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleMenu();
        }
    }

    void ToggleMenu()
    {
        isMenuActive = !isMenuActive;
        menuPanel.SetActive(isMenuActive);

        dayButton.onClick.AddListener(SetDayMode);
        nightButton.onClick.AddListener(SetNightMode);

        Btn_145m.onClick.AddListener(Set145mMode);
        Btn_50m.onClick.AddListener(Set50mMode);
        Btn_30m.onClick.AddListener(Set30mMode);

        DefaultBtn.onClick.AddListener(SetDefaultMode);
        arrowTrackingBtn.onClick.AddListener(SetArrowTrackingMode);

        mainBtn.onClick.AddListener(SetmainScene);
        seokHoJungBtn.onClick.AddListener(SetSeokHoJungScene);
        cyberBtn.onClick.AddListener(SetCyberScene);

        practiceBtn.onClick.AddListener(SetPracticeMode);
        competitionBtn.onClick.AddListener(SetCompetitionMode);

        resetBtn.onClick.AddListener(SetReset);

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
        nightSpotLight.SetActive(false);

        InGameUI.instance.targetDistanceText.color = Color.black;
        InGameUI.instance.soonText.color = Color.black;
        InGameUI.instance.siText.color = Color.black;
        InGameUI.instance.hitTargetCountText.color = Color.black;
        
        //Debug.Log("주간 모드 활성화");
    }

    // 야간 모드 설정
    void SetNightMode()
    {
        sceneLight.color = Color.blue;      // 조명의 색상을 어두운 파란색으로 설정
        RenderSettings.ambientLight = Color.gray; // 씬의 전체 조명 색상을 회색으로
        RenderSettings.skybox = nightSkybox; // 야간 Skybox 적용
        nightSpotLight.SetActive(true);

        InGameUI.instance.targetDistanceText.color = Color.white;
        InGameUI.instance.soonText.color = Color.white;
        InGameUI.instance.siText.color = Color.white;
        InGameUI.instance.hitTargetCountText.color = Color.white;
        //Debug.Log("야간 모드 활성화");
    }

    void Set145mMode()
    {
        Player.instance.transform.position = new UnityEngine.Vector3(0, Player.instance.transform.position.y, 0);
        CameraSwitchCinemachine.instance.SetCameraPosition(CameraSwitchCinemachine.DistanceMode.M_145);
        InGameUI.instance.SetTargetDistanceText(145);
    }

    void Set50mMode()
    {
        Player.instance.transform.position = new UnityEngine.Vector3(0, Player.instance.transform.position.y, 145 - 50);
        CameraSwitchCinemachine.instance.SetCameraPosition(CameraSwitchCinemachine.DistanceMode.M_50);
        InGameUI.instance.SetTargetDistanceText(50);
    }

    void Set30mMode()
    {
        Player.instance.transform.position = new UnityEngine.Vector3(0, Player.instance.transform.position.y, 145 - 30);
        CameraSwitchCinemachine.instance.SetCameraPosition(CameraSwitchCinemachine.DistanceMode.M_30);
        InGameUI.instance.SetTargetDistanceText(30);
    }

    void SetArrowTrackingMode()
    {
        CameraSwitchCinemachine.instance.SetCameraMode(CameraSwitchCinemachine.CameraMode.ArrowTracking);
    }

    void SetDefaultMode()
    {
        CameraSwitchCinemachine.instance.SetCameraMode(CameraSwitchCinemachine.CameraMode.Default);
    }

    void SetPracticeMode()
    {
        InGameUI.instance.SetLaunchTimeText(false);
    }

    void SetCompetitionMode()
    {
        InGameUI.instance.SetLaunchTimeText(true);
    }

    void SetReset()
    {
        InGameUI.instance.SetSoonText(true);
    }

    void SetmainScene()
    {
        SceneManagement(1);
    }

    void SetSeokHoJungScene()
    {
        SceneManagement(2);
    }

    void SetCyberScene()
    {
        SceneManagement(3);
    }
}
