using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class Attack : MonoBehaviour
{
    #region �ν����� ���� ����
    public Transform player; // �÷��̾��� Transform
    [SerializeField] GameObject pickaxe;
    [SerializeField] HorizontalMove horizontalMove;
    [Header("���� �ݰ�")]
    [Header("���� X��")][Range(0.5f, 3.0f)] public float x;
    [Header("���� y��")][Range(0.5f, 3.0f)] public float y;
    [Header("���� ��Ÿ��")][Range(0.5f, 3.0f)] public float attackCoolDown = 0.15f;
    private bool isAttacking = false;
    #endregion

    /// <summary>
    /// <para>
    /// �ۼ��� : �̽�ö
    /// </para>
    /// <para>
    /// ===========================================
    /// </para>
    /// input system���� 'Attack' event invoke �� ȣ�� �Լ�
    /// 'HorizontalMove'�� ���� �Է� ���� �ް� �� ���� ���� ��/�Ʒ�/��/�� ������ �����ϰ� �� ��ġ�� �� ����
    /// </summary>
    public void PlayerAttack(InputAction.CallbackContext context)
    {
        if (!isAttacking)
        {
            StartCoroutine(OnAttack());
        }
    }

    IEnumerator OnAttack()
    {
        isAttacking = true;
        pickaxe.SetActive(true);
        //���
        if (horizontalMove.inputVec.y > 0)
        {
            MovePickAxe(0, y, 0f);
        }
        //�ϴ�
        else if (horizontalMove.inputVec.y < 0)
        {
            MovePickAxe(0, -y, 0f);
        }
        //��
        else if (horizontalMove.inputVec.x > 0)
        {
            MovePickAxe(x, 0, 90.0f);
        }
        //��
        else if (horizontalMove.inputVec.x < 0)
        {
            MovePickAxe(-x, 0, 90.0f);
        }
        yield return new WaitForSeconds(attackCoolDown);
        pickaxe.SetActive(false);
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
        Debug.Log("����");
        isMoving = true;
        pickaxe.SetActive(true);
        float elapsedTime = 0f;
        float totalRotationAngle = 360f * rotationPercentage;
        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            // ���� �ð��� ���� (0���� 1 ����)
            float t = elapsedTime / moveDuration;
            // ������ ������ ��ȯ (0���� 216�� ����)
            float angle = ((t * totalRotationAngle) + 30f) * Mathf.Deg2Rad;
            // Ÿ�� �˵��� ���� �����̱�
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
