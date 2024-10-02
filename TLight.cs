using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TLight : MonoBehaviour
{
    public static TLight instance;
    // 신호등
    private Color originColor;
    Color tempColor;
    private int TLNumber; // 관 숫자

    private String thisTLName;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        originColor = this.GetComponent<Renderer>().material.color;
        thisTLName = this.name;
        TLNumber = int.Parse(thisTLName.Substring(thisTLName.IndexOf('_') + 1));
        Debug.Log(TLNumber);
    }

    public IEnumerator BlinkTLight(int targetNum)
    {
        if(targetNum == TLNumber)
        {
            yield return new WaitForSeconds(5.5f);
            
            this.GetComponent<Renderer>().material.color = new Color(1, 0, 0, 1);
            tempColor = this.GetComponent<Renderer>().material.color;

            int count = 0;
            while (count < 2)
            {
                while (this.GetComponent<Renderer>().material.color.r > 0f)
                {
                    tempColor.r -= 0.1f;
                    this.GetComponent<Renderer>().material.color = tempColor;
                    yield return new WaitForSeconds(0.1f);
                }

                yield return new WaitForSeconds(0.5f);

                while (this.GetComponent<Renderer>().material.color.r < 1f)
                {
                    tempColor.r += 0.1f;
                    this.GetComponent<Renderer>().material.color = tempColor;
                    yield return new WaitForSeconds(0.1f);
                }

                yield return new WaitForSeconds(0.5f);
                count++;
        }

            this.GetComponent<Renderer>().material.color = originColor;
        }
    }
}
