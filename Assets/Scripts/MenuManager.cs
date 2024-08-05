using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public GameObject MainMenuPanel, ControlsPanel, _mainMenuFirst, _controlsMenuFirst;

    // Start is called before the first frame update
    void Start()
    {
        EventSystem.current.SetSelectedGameObject(_mainMenuFirst);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartPressed()
    {
        SceneManager.LoadScene(1);
        EventSystem.current.SetSelectedGameObject(null);
    }

    public void ControlsPressed()
    {
        MainMenuPanel.SetActive(false);
        ControlsPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_controlsMenuFirst);
    }

    public void QuitPressed()
    {
        Application.Quit();
        Debug.Log("Application Quit");
    }

    public void BackPressed()
    {
        ControlsPanel.SetActive(false);
        MainMenuPanel.SetActive(true);
        EventSystem.current.SetSelectedGameObject(_mainMenuFirst);
    }
}
