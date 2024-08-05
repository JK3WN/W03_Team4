using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GameObjectGrid : MonoBehaviour
{
    public GameObjectArray[] grid;

    public int GetArrayLength(int row)
    {
        if (grid == null || row >= grid.Length || row < 0)
        {
            return -1; // 유효하지 않은 행 인덱스에 대한 처리
        }

        return grid[row].array != null ? grid[row].array.Length : 0;
    }
}