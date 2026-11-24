using System.Collections;
using UnityEngine;

public class DoorCtrl : MonoBehaviour
{
    [Header("이동할 오브젝트")]
    public Transform keyLockKeyboard;
    public Transform leftDoor;
    public Transform rightDoor;

    [Header("문 이동 설정")]
    public float moveSpeed = 4f;

    private bool isOpen = false;
    private bool isMoving = false;

    // 버튼에서 호출할 문 열기 함수
    public void OpenDoor()
    {
        if (isOpen || isMoving)
            return;

        StartCoroutine(SlideDoorsCoroutine());
    }

    private IEnumerator SlideDoorsCoroutine()
    {
        isMoving = true;

        // 시작 위치 저장
        Vector3 startKeyboard = keyLockKeyboard != null
            ? keyLockKeyboard.localPosition : Vector3.zero;

        Vector3 startLeft = leftDoor != null
            ? leftDoor.localPosition : Vector3.zero;

        Vector3 startRight = rightDoor != null
            ? rightDoor.localPosition : Vector3.zero;


        Vector3 targetLeft =
            startLeft + new Vector3(0.6f, 0f, 0f);

        Vector3 targetRight =
            startRight + new Vector3(-0.6f, 0f, 0f);

        float elapsed = 0f;
        float duration = 1f / Mathf.Max(moveSpeed, 0.01f);

        // 부드럽게 이동
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.SmoothStep(
                0f, 1f, Mathf.Clamp01(elapsed / duration));

            if (leftDoor != null)
                leftDoor.localPosition =
                    Vector3.Lerp(startLeft, targetLeft, t);

            if (rightDoor != null)
                rightDoor.localPosition =
                    Vector3.Lerp(startRight, targetRight, t);

            yield return null;
        }

        if (leftDoor != null)
            leftDoor.localPosition = targetLeft;

        if (rightDoor != null)
            rightDoor.localPosition = targetRight;

        isOpen = true;
        isMoving = false;

        Debug.Log("양쪽 문이 열렸습니다.");
    }
}
