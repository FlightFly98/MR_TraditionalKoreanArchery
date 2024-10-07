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
    public Light sceneLight; 
    public Material daySkybox;
    public Material nightSkybox;
    public Light[] nightSpotLight;

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
        InGameUI.instance.hitTargetCountText.color = Color.black;
        //Debug.Log("주간 모드 활성화");
    }

    // 야간 모드 설정
    void SetNightMode()
    {
        sceneLight.color = Color.blue;      // 조명의 색상을 어두운 파란색으로 설정
        RenderSettings.ambientLight = Color.gray; // 씬의 전체 조명 색상을 회색으로
        RenderSettings.skybox = nightSkybox; // 야간 Skybox 적용
        InGameUI.instance.hitTargetCountText.color = Color.white;
        //Debug.Log("야간 모드 활성화");
    }

    void Set145mMode()
    {
        Player.instance.transform.position = new UnityEngine.Vector3(0, Player.instance.transform.position.y, 0);
        CameraSwitchCinemachine.instance.SetCameraPosition(CameraSwitchCinemachine.DistanceMode.M_145);
    }

    void Set50mMode()
    {
        Player.instance.transform.position = new UnityEngine.Vector3(0, Player.instance.transform.position.y, 145 - 50);
        CameraSwitchCinemachine.instance.SetCameraPosition(CameraSwitchCinemachine.DistanceMode.M_50);
    }

    void Set30mMode()
    {
        Player.instance.transform.position = new UnityEngine.Vector3(0, Player.instance.transform.position.y, 145 - 30);
        CameraSwitchCinemachine.instance.SetCameraPosition(CameraSwitchCinemachine.DistanceMode.M_30);
    }

    void SetArrowTrackingMode()
    {
        CameraSwitchCinemachine.instance.SetCameraMode(CameraSwitchCinemachine.CameraMode.ArrowTracking);
    }

    void SetDefaultMode()
    {
        CameraSwitchCinemachine.instance.SetCameraMode(CameraSwitchCinemachine.CameraMode.Default);
    }

}
