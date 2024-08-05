using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static bool isPlaying = true;
    private float startTime;

    public GameObject player, floor, spikeFloor, gameOverPanel, _gameOverFirst;
    public InputActionMap uiActionMap;
    public float changeSpeed = 0.1f;
    public GameObject[] DashGauge, DashSlots;
    public int[] EXPNeeded;
    public int CurrentExp = 0;

    public TMPro.TextMeshProUGUI timerText, expText, recordText;

    // Start is called before the first frame update
    void Start()
    {
        isPlaying = true;
        CurrentExp = 0;
        startTime = Time.time;
        StartCoroutine("RiseSpike");
        EventSystem.current.SetSelectedGameObject(_gameOverFirst);
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying)
        {
            // YJK, 타이머 돌아가는 코드
            float timeElapsed = Time.time - startTime;
            string min = ((int)timeElapsed / 60).ToString("00");
            string sec = (timeElapsed % 60).ToString("00.00");
            timerText.text = min + ":" + sec;

            // YJK, 대쉬 빈 칸은 MaxSlots에 맞게, 대쉬 게이지는 Dashes에 맞게
            for(int i = 0; i < player.GetComponent<PlayerDash>().MaxDashes; i++)
            {
                if (!DashSlots[i].activeSelf) DashSlots[i].SetActive(true);
            }
            for(int i = 0; i < 5; i++)
            {
                DashGauge[i].GetComponent<Image>().fillAmount = Mathf.Clamp(player.GetComponent<PlayerDash>().Dashes - (float)i, 0f, 1f);
            }

            // YJK, EXPNeeded 달성할 때마다 EXPNeeded만큼 감소 및 MaxDashes 증가
            if (player.GetComponent<PlayerDash>().MaxDashes < 5 && CurrentExp >= EXPNeeded[player.GetComponent<PlayerDash>().MaxDashes - 1])
            {
                CurrentExp -= EXPNeeded[player.GetComponent<PlayerDash>().MaxDashes - 1];
                player.GetComponent<PlayerDash>().AddMaxDash();
            }

            // YJK, EXPText 갱신
            if (player.GetComponent<PlayerDash>().MaxDashes < 5) expText.text = "EXP: " + CurrentExp + " / " + EXPNeeded[player.GetComponent<PlayerDash>().MaxDashes - 1];
            else expText.text = "Max Level";
        }
        else
        {
            gameOverPanel.SetActive(true);
            recordText.text = timerText.text;
            Destroy(player);
        }
    }

    IEnumerator RiseSpike()
    {
        while (isPlaying && spikeFloor.transform.position.y < -19.5f)
        {
            yield return new WaitForSeconds(1f);
            spikeFloor.transform.position = new Vector3 (spikeFloor.transform.position.x, spikeFloor.transform.position.y + changeSpeed, 0f);
            floor.transform.position = new Vector3(floor.transform.position.x, floor.transform.position.y - changeSpeed, 0f);
        }
        Destroy(floor);
    }

    public void RestartPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenuPressed()
    {
        SceneManager.LoadScene(0);
    }
}
