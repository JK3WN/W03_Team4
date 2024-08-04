using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Random = UnityEngine.Random;

public class BrickSpawner : MonoBehaviour
{
    public int[] brickX;
    public int[] brickY;

    public int[] additionalBrickX;
    public int[] additionalBrickY;

    public float[] percents;
    public int index;

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

    public int RandomBrickNum(int direction, int brickCount)
    {
        if (direction == 0 || direction == 1)
        {
            for (int i = 0; i < 15 - brickCount + 1; i++)
            {
                additionalBrickY[i] = 0;
                for (int j = i; j < i + brickCount; j++)
                {
                    additionalBrickY[i] += brickY[j];
                }
            }

            for (int i = 1; i <= 15 - brickCount + 1; i++)
            {
                percents[i] = percents[i - 1] + (1f / additionalBrickY[i]);
            }

            float randX = Random.Range(0, percents[15 - brickCount]);
            for (int i = 1; i <= 15 - brickCount + 1; i++)
            {
                if (randX < percents[i])
                {
                    index = i;
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

            for (int i = 1; i <= 24 - brickCount + 1; i++)
            {
                percents[i] = percents[i - 1] + (1f / additionalBrickX[i]);
            }

            float randX = Random.Range(0, percents[24 - brickCount]);
            for (int i = 1; i <= 24 - brickCount + 1; i++)
            {
                if (randX < percents[i])
                {
                    index =  i;
                }
            }
        }

        return index;
    }
}
