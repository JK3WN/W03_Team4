using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// �ۼ��� : �̽�ö
/// </para>
/// <para>
/// ===========================================
/// </para>
/// �÷��̾� ���� ���� Ŭ����
/// </summary>
public class Death : MonoBehaviour
{

    /// <summary>
    /// <para>
    /// �ۼ��� : �̽�ö
    /// </para>
    /// <para>
    /// ===========================================
    /// </para>
    /// �÷��̾� ���ο� ���׸��� ĸ���� �������� ������ ��ġ�� �Ǿ� �÷��̾ ������ �� �����Ǿ�
    /// ��ԵǸ� �װ� ��
    /// </summary>
    void LateUpdate()
    {
        if (GetComponent<PlayerCollision>().IsDead)
        {
            Debug.Log("Dead!");
            GameManager.isPlaying = false;
        }
    }
}
