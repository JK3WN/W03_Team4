#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System;

[CustomEditor(typeof(GameObjectGrid))]
public class GameObjectGridEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        GameObjectGrid gridScript = (GameObjectGrid)target;

        if (gridScript.grid == null)
        {
            gridScript.grid = new GameObjectArray[0];
        }

        // 배열의 크기 조정
        int newRows = EditorGUILayout.IntField("Number of Rows", gridScript.grid.Length);
        if (newRows != gridScript.grid.Length)
        {
            Array.Resize(ref gridScript.grid, newRows);
        }

        for (int i = 0; i < gridScript.grid.Length; i++)
        {
            if (gridScript.grid[i] == null)
            {
                gridScript.grid[i] = new GameObjectArray();
            }

            EditorGUILayout.LabelField("Row " + i);

            // 행의 크기 조정
            int newColumns = EditorGUILayout.IntField("  Number of Columns", gridScript.grid[i].array != null ? gridScript.grid[i].array.Length : 0);
            if (gridScript.grid[i].array == null || newColumns != gridScript.grid[i].array.Length)
            {
                Array.Resize(ref gridScript.grid[i].array, newColumns);
            }

            // 각 요소 편집
            for (int j = 0; j < newColumns; j++)
            {
                gridScript.grid[i].array[j] = (GameObject)EditorGUILayout.ObjectField("    Element " + j, gridScript.grid[i].array[j], typeof(GameObject), true);
            }
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif