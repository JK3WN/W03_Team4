using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SpawnManager : MonoBehaviour
{
    public int lv = 0;
    public float currTime = 0;
    public float TimeInterval = 1f;
    public float SpaceInterval = 1f;
    public GameObject[] MirrorList;

    [Header("진행 레벨")]
    public float[] lvSec;
    [Header("레벨 당 블록 속도 범위")]
    public int[] speedStartRangeNums;
    public int[] speedEndRangeNums;
    public BrickSpawner brickSpawner;
    public GameObjectGrid gameObjectGrid;

    [Header("웨이브 생성")]
    public float[] waveSec;
    public int[] waveCounts;
    [Range(0,1.0f)]
    public float waveSpawnInterval;
    // Start is called before the first frame update
    void Start()
    {
        if (brickSpawner == null) brickSpawner = GetComponent<BrickSpawner>();
        StartCoroutine("Spawn");
        StartCoroutine(Wave());
    }
    void Update()
    {
        currTime += Time.deltaTime;
        if (currTime >= lvSec[lv])
        {
            lv++;
            Debug.Log(lv);
        }
    }
    // YJK, TimeInterval마다 한 블록씩 스폰
    IEnumerator Spawn()
    {
        while (GameManager.isPlaying)
        {
            yield return new WaitForSeconds(TimeInterval);
            brickSpawn();
        }
    }

    IEnumerator Wave()
    {
        while (GameManager.isPlaying)
        {
            int index = Random.Range(0, waveSec.Length);
            for (int i = 0; i < waveCounts[index]; i++)
            {
                brickSpawn();
                yield return new WaitForSeconds(waveSpawnInterval);
            }
            yield return new WaitForSeconds(waveSec[index]);
        }
    }

    // YJK, 원하는 블록을 스폰시키는 명령
    public void SpawnBrick(int direction, int startPos, float speed, GameObject blockType)
    {
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
                break;
            case 1:
                GameObject brick1 = GameObject.Instantiate(blockType, new Vector3(MirrorList[1].transform.position.x + (float)blockType.GetComponent<PlatformMove>().x / 2, MirrorList[1].transform.position.y + MirrorList[1].transform.lossyScale.y / 2 - (float)blockType.GetComponent<PlatformMove>().y / 2 - startPos * SpaceInterval, 0), Quaternion.identity);
                brick1.GetComponent<PlatformMove>().MoveSpeed = new Vector2(-speed, 0);
                break;
            case 2:
                GameObject brick2 = GameObject.Instantiate(blockType, new Vector3(MirrorList[2].transform.position.x - MirrorList[2].transform.lossyScale.x / 2 + (float)blockType.GetComponent<PlatformMove>().x / 2 + startPos * SpaceInterval, MirrorList[2].transform.position.y - (float)blockType.GetComponent<PlatformMove>().y / 2, 0), Quaternion.identity);
                brick2.GetComponent<PlatformMove>().MoveSpeed = new Vector2(0, speed);
                break;
            case 3:
                GameObject brick3 = GameObject.Instantiate(blockType, new Vector3(MirrorList[3].transform.position.x - MirrorList[3].transform.lossyScale.x / 2 + (float)blockType.GetComponent<PlatformMove>().x / 2 + startPos * SpaceInterval, MirrorList[3].transform.position.y + (float)blockType.GetComponent<PlatformMove>().y / 2, 0), Quaternion.identity);
                brick3.GetComponent<PlatformMove>().MoveSpeed = new Vector2(0, -speed);
                break;
            default:
                break;
        }
    }

    public void brickSpawn()
    {
        int direction = brickSpawner.RandomDirection();
        int brickIndex = Random.Range(0, gameObjectGrid.GetArrayLength(lv));
        int startPos = -1;
        GameObject blockType = gameObjectGrid.grid[lv].array[brickIndex];
        if (direction < 2)
        {
            startPos = brickSpawner.RandomBrickNum(direction, blockType.GetComponent<PlatformMove>().y);
        }
        else
        {
            startPos = brickSpawner.RandomBrickNum(direction, blockType.GetComponent<PlatformMove>().x);
        }
        float speed = Random.Range(speedStartRangeNums[lv], speedEndRangeNums[lv]);
        SpawnBrick(direction, startPos, speed, blockType);
        brickSpawner.AddBrick(blockType, startPos, direction);
    }
}