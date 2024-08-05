using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Attack : MonoBehaviour
{
    #region 인스텍터 변수 선언
    public Transform player; // 플레이어의 Transform
    [SerializeField] GameObject pickaxe;
    [SerializeField] HorizontalMove horizontalMove;
    [Header("공격 반경")]
    [Header("공격 X축")][Range(0.5f, 3.0f)] public float x;
    [Header("공격 y축")][Range(0.5f, 3.0f)] public float y;
    [Header("공격 지속시간")][Range(0.0f, 2.0f)] public float attackTime = 0.03f;
    [Header("공격 쿨타임")][Range(0.0f, 3.0f)] public float attackCoolDown = 0.15f;
    private bool isAttacking = false;
    public float sideAngle = 45f;
    #endregion

    /// <summary>
    /// <para>
    /// 작성자 : 이승철
    /// </para>
    /// <para>
    /// ===========================================
    /// </para>
    /// input system에서 'Attack' event invoke 시 호출 함수
    /// 'HorizontalMove'의 현재 입력 값을 받고 그 값에 따라 위/아래/우/좌 방향을 구분하고 그 위치에 검 생성
    /// </summary>
    public void PlayerAttack(InputAction.CallbackContext context)
    {
        if (!isAttacking && GameManager.isPlaying)
        {
            StartCoroutine(OnAttack());
        }
    }

    IEnumerator OnAttack()
    {
        isAttacking = true;
        pickaxe.SetActive(true);
        // YJK, 컨트롤러에서 상하 공격이 더 어려워지도록 변경
        float inputAngle = Vector2.SignedAngle(Vector2.right, horizontalMove.inputVec);
        //상단
        if (horizontalMove.inputVec.y > 0 && inputAngle > sideAngle && inputAngle < 180 - sideAngle)
        {
            MovePickAxe(0, y, 0f);
        }
        //하단
        else if (horizontalMove.inputVec.y < 0 && inputAngle < -sideAngle && inputAngle > sideAngle - 180)
        {
            MovePickAxe(0, -y, 180f);
        }
        //우
        else if (horizontalMove.inputVec.x > 0)
        {
            MovePickAxe(x, 0, -90.0f);
        }
        //좌
        else if (horizontalMove.inputVec.x < 0)
        {
            MovePickAxe(-x, 0, 90.0f);
        }

        yield return new WaitForSeconds(attackTime);
        pickaxe.SetActive(false);

        yield return new WaitForSeconds(attackCoolDown);
        isAttacking = false;
    }
    void MovePickAxe(float x, float y, float degree)
    {
        pickaxe.transform.position = new Vector3(x, y, 0) + player.transform.position;
        pickaxe.transform.rotation = Quaternion.Euler(0, 0, degree);
    }
    /*
    IEnumerator ActivateAndMoveInEllipse()
    {
        Debug.Log("실행");
        isMoving = true;
        pickaxe.SetActive(true);
        float elapsedTime = 0f;
        float totalRotationAngle = 360f * rotationPercentage;
        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            // 현재 시간의 비율 (0에서 1 사이)
            float t = elapsedTime / moveDuration;
            // 비율을 각도로 변환 (0에서 216도 사이)
            float angle = ((t * totalRotationAngle) + 30f) * Mathf.Deg2Rad;
            // 타원 궤도를 따라 움직이기
            float x = Mathf.Sin(angle) * xRadius;
            float y = Mathf.Cos(angle) * yRadius;
            Vector3 targetPosition = player.position + new Vector3(x, y, 0);
            pickaxe.transform.position = targetPosition;
            yield return null;
        }
        pickaxe.SetActive(false);
        yield return new WaitForSeconds(pickaxeCoolDown);
        isMoving = false;
    }
    */
}
