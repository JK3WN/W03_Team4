using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Build;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    public int speedLV = 0;
    public float currTime = 0;

    [Header("생성 시간, 거리 간격")]
    public float TimeInterval = 1f;
    public float SpaceInterval = 1f;
    public GameObject[] MirrorList;

    [Header("벽돌 속도 배열")]
    public float[] lvSec;
    public int[] speedStartRangeNums;
    public int[] speedEndRangeNums;

    [Header("벽돌 생성 배열")] 
    public GameObject[] brickList1;
    public GameObject[] brickList2;
    public GameObject[] brickList3;
    public GameObject[] brickList4;
    public GameObject[] brickList5;

    [Header("현재 블록 생성 배열")]
    public GameObject[] currBrickList;
    public int currBrickCount;

    [Header("웨이브 생성")]
    public int[] waveTimes;
    public int[] waveSpwanCount;

    [Range(0, 3.0f)]
    public float waveSpawnInterval;
    

    public BrickSpawner brickSpawner;

    // Start is called before the first frame update
    void Start()
    {
        if(brickSpawner == null) brickSpawner = GetComponent<BrickSpawner>();
        StartCoroutine("Spawn");
        StartCoroutine("Wave");
    }

    void Update()
    {
        currTime += Time.deltaTime;
        if (currTime >= lvSec[speedLV])
        {
            speedLV++;
            switch (speedLV)
            {
                case 0:
                    Debug.Log("LV1");
                    currBrickCount = brickList1.Length;
                    for (int i = 0; i < currBrickCount; i++)
                    {
                        currBrickList[i] = brickList1[i];
                    }
                    break;
                case 1:
                    Debug.Log("LV2");
                    currBrickCount = brickList2.Length;
                    for (int i = 0; i < currBrickCount; i++)
                    {
                        currBrickList[i] = brickList2[i];
                    }
                    break;
                case 2:
                    Debug.Log("LV3");
                    currBrickCount = brickList3.Length;
                    for (int i = 0; i < currBrickCount; i++)
                    {
                        currBrickList[i] = brickList3[i];
                    }
                    break;
                case 3:
                    currBrickCount = brickList4.Length;
                    for (int i = 0; i < currBrickCount; i++)
                    {
                        currBrickList[i] = brickList4[i];
                    }
                    break;
                case 4:
                    currBrickCount = brickList5.Length;
                    for (int i = 0; i < currBrickCount; i++)
                    {
                        currBrickList[i] = brickList5[i];
                    }
                    break;
            }
        }
    }

    // YJK, TimeInterval마다 한 블록씩 스폰
    IEnumerator Spawn()
    {
        while (GameManager.isPlaying)
        {
            yield return new WaitForSeconds(TimeInterval);
            int direction = brickSpawner.RandomDirection();
            int startPos = -1;
            GameObject blockType = currBrickList[Random.Range(0, currBrickCount)];

            if (direction < 2)
            {
                startPos = brickSpawner.RandomBrickNum(direction, blockType.GetComponent<PlatformMove>().y);
            }
            else
            {
                startPos = brickSpawner.RandomBrickNum(direction, blockType.GetComponent<PlatformMove>().x);
            }

            float speed = Random.Range(speedStartRangeNums[speedLV], speedEndRangeNums[speedLV]);

            SpawnBrick(direction, startPos, speed, blockType);

            brickSpawner.AddBrick(blockType, startPos, direction);
        }
    }

    IEnumerator Wave()
    {
        while (GameManager.isPlaying)
        {
            int wave = Random.Range(0, 3);
            switch (wave)
            {
                case 1:
                    StartCoroutine(waveSpawn(waveSpwanCount[wave]));
                    break;
                case 2:
                    StartCoroutine(waveSpawn(waveSpwanCount[wave]));
                    break;
                case 3:
                    StartCoroutine(waveSpawn(waveSpwanCount[wave]));
                    break;

            }
            yield return new WaitForSeconds(waveTimes[wave]);
        }
    }

    // YJK, 원하는 블록을 스폰시키는 명령
    public GameObject SpawnBrick(int direction, int startPos, float speed, GameObject blockType)
    {
        GameObject tempBrick = null;
        /*
        // 블럭의 X너비, Y너비
        float block_X = blockType.GetComponent<PlatformMove>().x;
        float block_Y = blockType.GetComponent<PlatformMove>().y;

        // 생성된 블럭 오브젝트 저장 공간
        GameObject brick = null;
        
        // 블럭 스폰 위치 계산
        float spawnPosX = MirrorList[direction].transform.position.x;
        float spawnPosY = MirrorList[direction].transform.position.y;

        // 방향에 따라 블럭의 생성 위치 결정
        if (direction < 2)
        {
            spawnPosX -= -block_X / 2;
            spawnPosY = spawnPosY + MirrorList[direction].transform.lossyScale.y / 2 - block_Y / 2 - startPos * SpaceInterval;
        }
        else
        {
            spawnPosX = spawnPosX - MirrorList[direction].transform.lossyScale.x / 2 + block_X / 2 + startPos * SpaceInterval;
            spawnPosY -= -block_Y / 2;
        }

        // 블럭 생성
        brick = Instantiate(blockType, new Vector3(spawnPosX, spawnPosY, 0), Quaternion.identity);
        brick.GetComponent<PlatformMove>().MoveSpeed = new Vector2();

        // 블럭 이동 방향 결정
        Vector2 moveVector = Vector2.zero;
        moveVector.x = moveVector.y = speed;

        moveVector.x *= (direction < 2 ? 1 : 0) * (direction == 0 ? 1 : -1);
        moveVector.y *= (direction < 2 ? 0 : 1) * (direction == 2 ? 1 : -1);

        brick.GetComponent<PlatformMove>().MoveSpeed = moveVector;
        */

        switch (direction)
        {
            case 0:
                GameObject brick0 = GameObject.Instantiate(blockType, new Vector3(MirrorList[0].transform.position.x - (float)blockType.GetComponent<PlatformMove>().x / 2, MirrorList[0].transform.position.y + MirrorList[0].transform.lossyScale.y / 2 - (float)blockType.GetComponent<PlatformMove>().y / 2 - startPos * SpaceInterval, 0), Quaternion.identity);
                brick0.GetComponent<PlatformMove>().MoveSpeed = new Vector2(speed, 0);
                tempBrick = brick0;
                break;
            case 1:
                GameObject brick1 = GameObject.Instantiate(blockType, new Vector3(MirrorList[1].transform.position.x + (float)blockType.GetComponent<PlatformMove>().x / 2, MirrorList[1].transform.position.y + MirrorList[1].transform.lossyScale.y / 2 - (float)blockType.GetComponent<PlatformMove>().y / 2 - startPos * SpaceInterval, 0), Quaternion.identity);
                brick1.GetComponent<PlatformMove>().MoveSpeed = new Vector2(-speed, 0);
                tempBrick = brick1;
                break;
            case 2:
                GameObject brick2 = GameObject.Instantiate(blockType, new Vector3(MirrorList[2].transform.position.x - MirrorList[2].transform.lossyScale.x / 2 + (float)blockType.GetComponent<PlatformMove>().x / 2 + startPos * SpaceInterval, MirrorList[2].transform.position.y - (float)blockType.GetComponent<PlatformMove>().y / 2, 0), Quaternion.identity);
                brick2.GetComponent<PlatformMove>().MoveSpeed = new Vector2(0, speed);
                tempBrick = brick2;
                break;
            case 3:
                GameObject brick3 = GameObject.Instantiate(blockType, new Vector3(MirrorList[3].transform.position.x - MirrorList[3].transform.lossyScale.x / 2 + (float)blockType.GetComponent<PlatformMove>().x / 2 + startPos * SpaceInterval, MirrorList[3].transform.position.y + (float)blockType.GetComponent<PlatformMove>().y / 2, 0), Quaternion.identity);
                brick3.GetComponent<PlatformMove>().MoveSpeed = new Vector2(0, -speed);
                tempBrick = brick3;
                break;
        }

        return tempBrick;
    }

    public void spawnBrick()
    {
        int direction = brickSpawner.RandomDirection();
        int startPos = -1;
        GameObject blockType = currBrickList[Random.Range(0, currBrickCount)];

        if (direction < 2)
        {
            startPos = brickSpawner.RandomBrickNum(direction, blockType.GetComponent<PlatformMove>().y);
        }
        else
        {
            startPos = brickSpawner.RandomBrickNum(direction, blockType.GetComponent<PlatformMove>().x);
        }

        float speed = Random.Range(speedStartRangeNums[speedLV], speedEndRangeNums[speedLV]);

        GameObject go = SpawnBrick(direction, startPos, speed, blockType);

        brickSpawner.AddBrick(blockType, startPos, direction);
    }

    IEnumerator waveSpawn(int count)
    {
        for (int i = 0; i < count; i++)
        {
            spawnBrick();
            yield return new WaitForSeconds(waveSpawnInterval);
        }
    }
}
