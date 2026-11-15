using System.Collections;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    [Header("이동할 오브젝트 연결")]
    public Transform keyLockKeyboard;
    public Transform leftDoor;
    public Transform rightDoor;

    [Header("문 이동 설정")]
    public float moveSpeed = 2.0f; // 문이 열리는 속도
    public string controllerTag = "Hand"; // VR 컨트롤러에 지정할 태그

    private bool isOpen = false;
    private bool isMoving = false;

    // 1. VR 컨트롤러가 버튼에 직접 닿았을 때 (Trigger)
    private void OnTriggerEnter(Collider other)
    {
        // 닿은 오브젝트의 태그가 설정한 컨트롤러 태그와 일치하면 작동
        if (other.CompareTag(controllerTag))
        {
            openDoor();
        }
    }

    // 2. XR Interaction Toolkit의 Interactable 이벤트(Select/Hover 등)에서 호출할 함수
    public void openDoor()
    {
        // 이미 열려있거나 움직이는 중이면 중복 실행 방지
        if (isOpen || isMoving) return;

        isOpen = true;
        StartCoroutine(SlideDoorsCoroutine());
    }

    private IEnumerator SlideDoorsCoroutine()
    {
        isMoving = true;

        // 각 오브젝트의 현재 로컬 위치 저장
        Vector3 startKeyboardPos = keyLockKeyboard != null ? keyLockKeyboard.localPosition : Vector3.zero;
        Vector3 startLeftPos = leftDoor != null ? leftDoor.localPosition : Vector3.zero;
        Vector3 startRightPos = rightDoor != null ? rightDoor.localPosition : Vector3.zero;

        // 목표 로컬 위치 계산 (X축 기준 +0.6 / -0.6)
        Vector3 targetKeyboardPos = startKeyboardPos + new Vector3(0.6f, 0f, 0f);
        Vector3 targetLeftPos = startLeftPos + new Vector3(0.6f, 0f, 0f);
        Vector3 targetRightPos = startRightPos + new Vector3(-0.6f, 0f, 0f);

        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * moveSpeed;

            // 부드럽게 이동 (SmoothStep 적용)
            float t = Mathf.SmoothStep(0f, 1f, elapsedTime);

            if (keyLockKeyboard != null)
                keyLockKeyboard.localPosition = Vector3.Lerp(startKeyboardPos, targetKeyboardPos, t);

            if (leftDoor != null)
                leftDoor.localPosition = Vector3.Lerp(startLeftPos, targetLeftPos, t);

            if (rightDoor != null)
                rightDoor.localPosition = Vector3.Lerp(startRightPos, targetRightPos, t);

            yield return null;
        }

        // 오차 방지를 위해 최종 위치 고정
        if (keyLockKeyboard != null) keyLockKeyboard.localPosition = targetKeyboardPos;
        if (leftDoor != null) leftDoor.localPosition = targetLeftPos;
        if (rightDoor != null) rightDoor.localPosition = targetRightPos;

        isMoving = false;
    }
}
