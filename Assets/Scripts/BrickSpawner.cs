using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

/// <summary>
/// <para>
/// 작성자 : 이승철
/// </para>
/// <para>
/// ===========================================
/// </para>
/// 블록 균일생성 관련 클래스
/// </summary>
public class BrickSpawner : MonoBehaviour
{
    private const int rowSize = 15;
    private const int columnSize = 24;

    public int[] brickX = new int[columnSize];
    public int[] brickY = new int[rowSize];

    public int[] additionalBrickX = new int[columnSize];
    public int[] additionalBrickY = new int[rowSize];

    public float[] percents = new float[columnSize+1];

    public void Awake()
    {
        for (int i = 0; i < brickX.Length; i++)
        {
            brickX[i] = 1;
        }

        for (int i = 0; i < brickY.Length; i++)
        {
            brickY[i] = 1;
        }
    }

    /// <summary>
    /// <para>
    /// 작성자 : 이승철
    /// </para>
    /// <para>
    /// ===========================================
    /// </para>
    /// 블록 추가시 메소드, 블록 추가를 하고 추가된 상태를 현재 블록 배열에다가 갱신함
    /// 시작지점(startPos) 배열부터 해서 블록크기만큼 번째 배열까지 크기를 1 더함
    /// </summary>
    public void AddBrick(GameObject brick, int startPos, int direction)
    {
        PlatformMove platformMove = brick.GetComponent<PlatformMove>();
        if (direction == 0 || direction == 1)
        {
            for (int i = startPos; i < startPos + platformMove.y; i++)
            {
                brickY[i]++;
            }
        }
        else
        {
            for (int i = startPos; i < startPos + platformMove.x; i++)
            {
                brickX[i]++;
            }
        }
    }

    /// <summary>
    /// <para>
    /// 작성자 : 이승철
    /// </para>
    /// <para>
    /// ===========================================
    /// </para>
    /// 블록 추가시 메소드, 블록 추가를 하고 추가된 상태를 현재 블록 배열에다가 갱신함
    /// 시작지점(startPos) 배열부터 해서 블록크기만큼 번째 배열까지 크기를 1 뺌
    /// </summary>
    public void DeleteBrick(GameObject brick, int startPos, int direction)
    {
        PlatformMove platformMove = brick.GetComponent<PlatformMove>();
        if (direction == 0 || direction == 1)
        {
            for (int i = startPos; i < startPos + platformMove.y; i++)
            {
                brickY[i]--;
            }
        }
        else
        {
            for (int i = startPos; i < startPos + platformMove.x; i++)
            {
                brickX[i]--;
            }
        }
    }

    /// <summary>
    /// <para>
    /// 작성자 : 이승철, 임재균
    /// </para>
    /// <para>
    /// ===========================================
    /// </para>
    /// X축의 합과 Y축의 합을 구하고 역수를 취한 확률이 그 확률이 되게 하여
    /// 작은 값일수록 나올 확률이 증가하고 큰 값일수록 나올 확률이 줄어들게 하는 랜덤 방향 메소드
    /// </summary>
    public int RandomDirection()
    {
        // 리턴 할 배열
        int index = 0;
        // X, Y 합
        float sumX = 0;
        float sumY = 0;
        for (int i = 0; i < rowSize; i++)
        {
            sumY += brickY[i];
        }

        for (int i = 0; i < columnSize; i++)
        {
            sumX += brickX[i];
        }

        float randomXY = Random.Range(0, 1 / sumX + 1 / sumY);
        float randomLR = Random.Range(0.0f, 1.0f);
        // Y���̶��
        if (randomXY <= 1/sumY)
        {
            index = 0;
        }
        else
        {
            //X축이면 2
            index = 2;
        }

        // 50%확률로 +1을 하여 위 > 아래/왼 > 오로 되게
        if(randomLR <= 0.5f)
        {
            index++;
        }

        return index;
    }

    /// <summary>
    /// <para>
    /// 작성자 : 이승철, 임재균
    /// </para>
    /// <para>
    /// ===========================================
    /// </para>
    /// 블록의 크기가 들어갈 크기만큼 합쳐서 그 값이 크면 나올 확률이 적게, 그 값이 작으면 나올 확률이 크게 값을 지정해주는 메소드
    /// </summary>
    public int RandomBrickNum(int direction, int brickCount)
    {
        int index = 0;
        if (direction == 0 || direction == 1)
        {
            for (int i = 0; i < rowSize - brickCount + 1; i++)
            {
                // 이전에 값이 남아 있을 수 있으므로 초기값 1로 설정
                additionalBrickY[i] = 1;
                for (int j = i; j < i + brickCount; j++)
                {
                    // 블록 크기 만큼 합치기
                    additionalBrickY[i] += brickY[j];
                }
            }

            for (int i = 0; i < rowSize  - brickCount + 1; i++)
            {
                // 피보나치 수열처럼 각 확률의 합을 구함
                percents[i + 1] = percents[i] + (1f / (float)additionalBrickY[i]);
            }

            // 그 확률 크기 값 범위에서 랜덤으로 값을 뽑음
            float randX = Random.Range(0.0f, percents[rowSize - brickCount + 1]);
            // 거꾸로 그 값 범위에 있나 점검하며 뽑힌 값 확인하고 그 값으로 설정
            for (int i = rowSize - brickCount + 1; i >= 1; i--)
            {
                if (randX > percents[i])
                {
                    index = i;
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < columnSize - brickCount + 1; i++)
            {
                additionalBrickX[i] = 1;
                for (int j  = i; j < i + brickCount; j++)
                {
                    additionalBrickX[i] += brickX[j];
                }
            }

            for (int i = 0; i < columnSize - brickCount + 1; i++)
            {
                percents[i + 1] = percents[i] + (1f / additionalBrickX[i]);
            }

            float randX = Random.Range(0, percents[columnSize - brickCount + 1]);
            for (int i = columnSize - brickCount + 1; i >= 1; i--)
            {
                if (randX > percents[i])
                {
                    index = i;
                    break;
                }
            }
        }

        return index;
    }
}
