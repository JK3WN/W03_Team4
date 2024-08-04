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
            // YJK, Ÿ�̸� ���ư��� �ڵ�
            float timeElapsed = Time.time - startTime;
            string min = ((int)timeElapsed / 60).ToString("00");
            string sec = (timeElapsed % 60).ToString("00.00");
            timerText.text = min + ":" + sec;

            // YJK, �뽬 �� ĭ�� MaxSlots�� �°�, �뽬 �������� Dashes�� �°�
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