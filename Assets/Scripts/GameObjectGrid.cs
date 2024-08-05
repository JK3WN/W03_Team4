using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GameObjectGrid : MonoBehaviour
{
    public GameObjectArray[] grid;

    public int GetArrayLength(int row)
    {
        if (grid == null || row >= grid.Length || row < 0)
        {
            return -1; // ��ȿ���� ���� �� �ε����� ���� ó��
        }

        return grid[row].array != null ? grid[row].array.Length : 0;
    }
}