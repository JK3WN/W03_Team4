using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// <para>
/// 작성자 : 이승철
/// </para>
/// <para>
/// ===========================================
/// </para>
/// 플레이어 죽음 판정 클래스
/// </summary>
public class Death : MonoBehaviour
{

    /// <summary>
    /// <para>
    /// 작성자 : 이승철
    /// </para>
    /// <para>
    /// ===========================================
    /// </para>
    /// 플레이어 내부에 조그마한 캡슐을 만들어놓고 벽돌이 겹치게 되어 플레이어가 낑겼을 때 투과되어
    /// 닿게되면 죽게 함
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
