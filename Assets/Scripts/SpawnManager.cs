using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public float TimeInterval = 1f;
    public float SpaceInterval = 1f;
    public GameObject[] MirrorList;
    public GameObject[] BrickList;

    // Start is called before the first frame update
    void Start()
    {
        //SpawnBrick(0, 0, 5f, BrickList[2]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // YJK, ���ϴ� ������ ������Ű�� ����
    public void SpawnBrick(int direction, int startPos, float speed, GameObject blockType)
    {
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
}