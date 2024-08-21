using UnityEngine;
using System.Collections;
using System;

public class NPC : MonoBehaviour
{
    private Animator animator;
    public event Action<int> ReachedQueuePosition; // �������� �������, ����� ���������� ������

    public int CurrentTargetIndex { get; private set; } // ������� ������ ����

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void StartMovingToQueue(Transform queuePosition, int targetIndex)
    {
        CurrentTargetIndex = targetIndex;
        StartCoroutine(MoveTo(queuePosition.position));
    }

    private IEnumerator MoveTo(Vector3 targetPosition)
    {
        animator.SetBool("IsWalking", true);
        while (Vector3.Distance(transform.position, targetPosition) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, 7f * Time.deltaTime);
            yield return null;
        }
        animator.SetBool("IsWalking", false);
        ReachedQueuePosition?.Invoke(CurrentTargetIndex); // ��������� ������� ������
    }

    public void LeaveQueue()
    {
        Destroy(gameObject);
    }
}