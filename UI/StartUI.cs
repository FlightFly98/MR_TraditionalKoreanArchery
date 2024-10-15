using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StartUI : UIBase
{
    public static StartUI instance;
    public Canvas startUICanvas;
    public bool checkInput = false;
    public TMP_InputField poundInputField;
    public TMP_InputField arrowWeightInputField;
    public TMP_InputField arrowLengthInputField;
    public string poundInfo;
    public string arrowWeightInfo;
    public string arrowLengthInfo;
    public Button startButton;
    void Awake()
    {
        instance = this;
    }

    public void SettingInputField()
    {
        arrowWeightInfo = arrowWeightInputField.GetComponent<TMP_InputField>().text.Trim();

        poundInfo = poundInputField.GetComponent<TMP_InputField>().text.Trim();

        arrowLengthInfo = arrowLengthInputField.GetComponent<TMP_InputField>().text.Trim();

        checkInput = true;
    }
}
