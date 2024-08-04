using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static bool isPlaying = true;
    private float startTime;

    public GameObject player;
    public GameObject[] DashGauge, DashSlots;

    public TMPro.TextMeshProUGUI timerText;

    // Start is called before the first frame update
    void Start()
    {
        isPlaying = true;
        startTime = Time.time;
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
        }
    }
}
