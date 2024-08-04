using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class BrickSpawner : MonoBehaviour
{
    public int[] brickX = new int[24];
    public int[] brickY = new int[15];

    public int[] additionalBrickX = new int[24];
    public int[] additionalBrickY = new int[15];

    public float[] percents = new float[25];

    public void AddBrick(GameObject brick, int direction)
    {
        PlatformMove platformMove = brick.GetComponent<PlatformMove>();
        if (direction == 0 || direction == 1)
        {
            for (int i = platformMove.start; i < platformMove.start+platformMove.y; i++)
            {
                brickY[i]++;
            }
        }
        else
        {
            for (int i = platformMove.start; i < platformMove.start + platformMove.y; i++)
            {
                brickX[i]++;
            }
        }
    }

    public void DeleteBrick(GameObject brick, int direction)
    {
        PlatformMove platformMove = brick.GetComponent<PlatformMove>();
        if (direction == 0 || direction == 1)
        {
            for (int i = platformMove.start; i < platformMove.start + platformMove.y; i++)
            {
                brickY[i]--;
            }
        }
        else
        {
            for (int i = platformMove.start; i < platformMove.start + platformMove.y; i++)
            {
                brickX[i]--;
            }
        }
    }

    public int RandomDirection()
    {
        int index = 0;
        int sumX = 0;
        int sumY = 0;
        for(int i = 0; i< 15; i++)
        {
            sumY += brickY[i];
        }

        for(int i = 0; i<24; i++)
        {
            sumX += brickX[i];
        }

        Debug.Log(sumX + "," + sumY);
        float randomXY = Random.Range(0, sumX+sumY);
        float randomLR = Random.Range(0, 1);

        // Y축이라면
        if (randomXY <= sumY)
        {
            index = 0;
        }
        else
        {
            index = 2;
        }

        if(randomLR <= 0.5f)
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
            for (int i = 0; i < 15 - brickCount + 1; i++)
            {
                additionalBrickY[i] = 1;
                for (int j = i; j < i + brickCount; j++)
                {
                    additionalBrickY[i] += brickY[j];
                }
            }

            for (int i = 0; i < 15 - brickCount + 1; i++)
            {
                percents[i + 1] = percents[i] + (1f / additionalBrickY[i]);
            }

            float randX = Random.Range(0, percents[15 - brickCount]);
            for (int i = 15 - brickCount + 1; i >= 1; i--)
            {
                if (randX < percents[i+1])
                {
                    index = i;
                    break;
                }
            }
        }
        else
        {
            for (int i = 0; i < 24 - brickCount + 1; i++)
            {
                for (int j = i; j < i + brickCount; j++)
                {
                    additionalBrickX[i] += brickX[j];
                }
            }

            for (int i = 0; i < 24 - brickCount + 1; i++)
            {
                percents[i + 1] = percents[i] + (1f / additionalBrickX[i]);
            }

            float randX = Random.Range(0, percents[24 - brickCount]);
            for (int i = 24 - brickCount + 1; i >= 1; i--)
            {
                if (randX < percents[i+1])
                {
                    index =  i;
                    break;
                }
            }
        }

        return index;
    }
}
