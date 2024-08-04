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
        for(int i = 0; i< 15; i++)
        {
            sumY += brickY[i];
        }

        for(int i = 0; i<24; i++)
        {
            sumX += brickX[i];
        }

        float randomXY = Random.Range(0, 1/sumX+1/sumY);
        float randomLR = Random.Range(0.0f, 1.0f);
  
        // Y축이라면
        if (randomXY <= 1/sumY)
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
                percents[i + 1] = percents[i] + (1f / (float)additionalBrickY[i]);
            }

            float randX = Random.Range(0.0f, percents[15 - brickCount + 1]);
            for (int i = 15 - brickCount + 1; i >= 1; i--)
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
            for (int i = 0; i < 24 - brickCount + 1; i++)
            {
                additionalBrickX[i] = 1;
                for (int j = i; j < i + brickCount; j++)
                {
                    additionalBrickX[i] += brickX[j];
                }
            }

            for (int i = 0; i < 24 - brickCount + 1; i++)
            {
                percents[i + 1] = percents[i] + (1f / additionalBrickX[i]);
            }

            float randX = Random.Range(0, percents[24 - brickCount + 1]);
            for (int i = 24 - brickCount + 1; i >= 1; i--)
            {
                if (randX > percents[i])
                {
                    index =  i;
                    break;
                }
            }
        }

        return index;
    }
}
