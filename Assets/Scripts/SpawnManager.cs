using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor.Build;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public float TimeInterval = 1f;
    public float SpaceInterval = 1f;
    public GameObject[] MirrorList;
    public GameObject[] BrickList;

    public BrickSpawner brickSpawner;

    // Start is called before the first frame update
    void Start()
    {
        if(brickSpawner == null) brickSpawner = GetComponent<BrickSpawner>();
        StartCoroutine("Spawn");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // YJK, TimeInterval���� �� ��Ͼ� ����
    IEnumerator Spawn()
    {
        while (GameManager.isPlaying)
        {
            yield return new WaitForSeconds(TimeInterval);
            int direction = brickSpawner.RandomDirection();
            int startPos = -1;
            GameObject blockType = BrickList[Random.Range(0, BrickList.Length)];
            if(direction < 2)
            {
                startPos = brickSpawner.RandomBrickNum(direction, blockType.GetComponent<PlatformMove>().y);
            }
            else
            {
                startPos = brickSpawner.RandomBrickNum(direction, blockType.GetComponent<PlatformMove>().x);
            }
            float speed = blockType.GetComponent<PlatformMove>().MoveSpeed.magnitude;
            SpawnBrick(direction, startPos, speed, blockType);
            brickSpawner.AddBrick(blockType, startPos, direction);
        }
    }

    // YJK, ���ϴ� ����� ������Ű�� ���
    public GameObject SpawnBrick(int direction, int startPos, float speed, GameObject blockType)
    {
        // ���� X�ʺ�, Y�ʺ�
        float block_X = blockType.GetComponent<PlatformMove>().x;
        float block_Y = blockType.GetComponent<PlatformMove>().y;

        // ������ �� ������Ʈ ���� ����
        GameObject brick = null;
        
        // �� ���� ��ġ ���
        float spawnPosX = MirrorList[direction].transform.position.x;
        float spawnPosY = MirrorList[direction].transform.position.y;

        // ���⿡ ���� ���� ���� ��ġ ����
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

        // �� ����
        brick = Instantiate(blockType, new Vector3(spawnPosX, spawnPosY, 0), Quaternion.identity);
        brick.GetComponent<PlatformMove>().MoveSpeed = new Vector2();

        // �� �̵� ���� ����
        Vector2 moveVector = Vector2.zero;
        moveVector.x = moveVector.y = speed;

        moveVector.x *= (direction < 2 ? 1 : 0) * (direction == 0 ? 1 : -1);
        moveVector.y *= (direction < 2 ? 0 : 1) * (direction == 2 ? 1 : -1);

        brick.GetComponent<PlatformMove>().MoveSpeed = moveVector;

        return brick;
    }
}
