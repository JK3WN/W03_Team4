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

    public GameObject player, floor, spikeFloor, spikeCeiling, gameOverPanel, _gameOverFirst, tipText, restartButton, mainMenuButon;
    public float changeSpeed = 0.1f;
    public GameObject[] DashGauge, DashSlots;
    public int[] EXPNeeded;
    public int CurrentExp = 0;

    public TMPro.TextMeshProUGUI timerText, expText, recordText;

    // YJK, 게임패드 진동 관련
    private Gamepad pad;
    private Coroutine stopRumbleCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        isPlaying = true;
        CurrentExp = 0;
        startTime = Time.time;
        StartCoroutine("IE_Intro");
        StartCoroutine("MoveTip");
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
            StartCoroutine("ShowButton");
        }
    }

    IEnumerator IE_RiseSpike()
    {
        while (isPlaying && spikeFloor.transform.position.y < -20f)
        {
            yield return new WaitForSeconds(0.05f);
            spikeFloor.transform.position = new Vector3 (spikeFloor.transform.position.x, spikeFloor.transform.position.y + changeSpeed, 0f);
        }
    }

    IEnumerator IE_FallSpike()
    {
        while (isPlaying && spikeCeiling.transform.position.y > 20f)
        {
            yield return new WaitForSeconds(0.05f);
            spikeCeiling.transform.position = new Vector3(spikeCeiling.transform.position.x, spikeCeiling.transform.position.y - changeSpeed, 0f);
        }
    }

    IEnumerator IE_ShrinkFloor()
    {
        while (isPlaying && floor.transform.localScale.x > 0.01f)
        {
            yield return new WaitForSeconds(0.05f);
            floor.transform.localScale = new Vector3(floor.transform.localScale.x - 0.15f, floor.transform.localScale.y, floor.transform.localScale.z);
        }
        Destroy(floor);
    }

    IEnumerator IE_Intro()
    {
        yield return new WaitForSeconds(2f);
        StartCoroutine("IE_RiseSpike");
        StartCoroutine("IE_FallSpike");
        yield return new WaitForSeconds(1.5f);
        StartCoroutine("IE_ShrinkFloor");
    }

    IEnumerator MoveTip()
    {
        while (isPlaying)
        {
            yield return new WaitForSeconds(0.01f);
            tipText.transform.position = new Vector3(tipText.transform.position.x - 2f, tipText.transform.position.y, 0f);
        }
    }

    IEnumerator ShowButton()
    {
        yield return new WaitForSeconds(2f);
        restartButton.SetActive(true);
        mainMenuButon.SetActive(true);
    }

    public void RestartPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenuPressed()
    {
        SceneManager.LoadScene(0);
    }

    // YJK, 게임패드 진동 시작
    public void RumblePulse(float low, float high, float duration)
    {
        pad = Gamepad.current;
        if (pad != null)
        {
            Debug.Log(pad);
            pad.SetMotorSpeeds(low, high);
            stopRumbleCoroutine = StartCoroutine(StopRumble(duration, pad));
        }
    }

    // YJK, 게임패드 진동 끝(RumblePulse 돌리면 알아서 얘도 실행)
    private IEnumerator StopRumble(float duration, Gamepad pad)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        pad.SetMotorSpeeds(0f, 0f);
    }
}
