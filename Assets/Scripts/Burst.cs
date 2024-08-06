using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;

public class Burst : MonoBehaviour
{
    private int clickTime;
    void Start()
    {
        clickTime = 0;
    }

    public void OnBurstBtnClicked()
    {
        clickTime++;
        if (clickTime == 5)
        {
            SceneManager.LoadScene(2);
            EventSystem.current.SetSelectedGameObject(null);
        }
    }
}
