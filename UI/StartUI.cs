using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartUI : UIBase
{
    public static StartUI instance;
    public Canvas startUICanvas;
    public bool checkInput = false;
    public TMP_InputField arrowWeightInputField;
    public string arrowWeightInfo;
    public Button startButton;
    void Awake()
    {
        instance = this;
    }

    public void SettingInputField()
    {
        arrowWeightInfo = arrowWeightInputField.GetComponent<TMP_InputField>().text;

        checkInput = true;
    }
}
