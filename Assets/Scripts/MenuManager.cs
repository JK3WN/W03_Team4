using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public GameObject MainMenuPanel, ControlsPanel;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartPressed()
    {
        SceneManager.LoadScene(1);
    }

    public void ControlsPressed()
    {
        MainMenuPanel.SetActive(false);
        ControlsPanel.SetActive(true);
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
    }
}
