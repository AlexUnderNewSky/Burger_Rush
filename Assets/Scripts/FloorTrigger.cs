using System.Collections;
using UnityEngine;

public class FloorTrigger : MonoBehaviour
{
    private Vector3 originalScale;
    public bool isScalingEnabled = true;
    public float scalingDuration = 0.5f;

    private Coroutine scalingCoroutine;

    [SerializeField] MachineBase Machine;

    PlayerTouchMovement playerMovement;


	void Start()
    {
        originalScale = transform.localScale;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerMovement = other.GetComponent<PlayerTouchMovement>();
            if (isScalingEnabled)
            {
                if (scalingCoroutine != null)
                {
                    StopCoroutine(scalingCoroutine);
                }
                scalingCoroutine = StartCoroutine(ScaleOverTime(transform.localScale, originalScale * 1.5f, scalingDuration));
            }
            if (playerMovement != null)
            {
                playerMovement.OnLoseFinger += HandleLoseFinger;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (scalingCoroutine != null)
            {
                StopCoroutine(scalingCoroutine);
                Machine.StopLevelUpdate();
			}
            scalingCoroutine = StartCoroutine(ScaleOverTime(transform.localScale, originalScale, scalingDuration));
            if (playerMovement != null)
            {
                playerMovement.OnLoseFinger -= HandleLoseFinger;
            }
        }
    }

    private IEnumerator ScaleOverTime(Vector3 startScale, Vector3 targetScale, float duration)
    {
        float startTime = Time.time;
        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            transform.localScale = Vector3.Lerp(startScale, targetScale, t);
            yield return null;
        }
        transform.localScale = targetScale;
    }

    private void HandleLoseFinger()
    {
        Machine.StartLevelUpdate(playerMovement.gameObject.GetComponent<CoinCollection>());
		if (playerMovement != null)
		{
			playerMovement.OnLoseFinger -= HandleLoseFinger;
		}
	}
}