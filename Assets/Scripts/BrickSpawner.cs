using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class BrickSpawner : MonoBehaviour
{
    private const int rowSize = 15;
    private const int columnSize = 24;

    public int[] brickX = new int[columnSize];
    public int[] brickY = new int[rowSize];

    public int[] additionalBrickX = new int[columnSize];
    public int[] additionalBrickY = new int[rowSize];

    public float[] percents = new float[columnSize + 1];

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

    public int RandomDirection()
    {
        int index = 0;
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
        // YÃàÀÌ¶ó¸é
        if (randomXY <= 1 / sumY)
        {
            index = 0;
        }
        else
        {
            index = 2;
        }

        if (randomLR <= 0.5f)
        {
            index++;
        }

        return index;
    }

    public int RandomBrickNum(int direction, int brickCount)
    {
        int index = 0;
        if (direction == 0 || direction == 1)
        {
            for (int i = 0; i < rowSize - brickCount + 1; i++)
            {
                additionalBrickY[i] = 1;
                for (int j = i; j < i + brickCount; j++)
                {
                    additionalBrickY[i] += brickY[j];
                }
            }

            for (int i = 0; i < rowSize  - brickCount + 1; i++)
            {
                percents[i + 1] = percents[i] + (1f / (float)additionalBrickY[i]);
            }

            float randX = Random.Range(0.0f, percents[rowSize - brickCount + 1]);
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
                for (int j = i; j < i + brickCount; j++)
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
