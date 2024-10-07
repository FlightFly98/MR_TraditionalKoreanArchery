using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Numerics;

public abstract class UIBase : MonoBehaviour
{
    protected Canvas mainCanvas;

    public void SceneManagement(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum);
    }

}
