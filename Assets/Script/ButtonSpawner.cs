
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ButtonSpawner : MonoBehaviour
{
    [Header("생성할 프리팹")]
    public GameObject enginePrefab;
    public GameObject sparePrefab;

    [Header("생성 위치 (월드 좌표)")]
    public Vector3 engineSpawnPosition = new Vector3(-3.5f, 5f, -0.5f);
    public Vector3 spareSpawnPosition = new Vector3(-5f, 5f, 5f);

    [Header("버튼 위치 (로컬 Y 좌표)")]
    public float startY = 0.3f;
    public float endY = -0.1f;

    [Header("버튼 움직임")]
    public float pressSpeed = 5f;
    public float returnSpeed = 5f;

    private XRBaseInteractable buttonInteractable;

    private Vector3 startLocalPosition;
    private Vector3 targetLocalPosition;

    private bool isPressed;
    private bool hasSpawned;

    private void Awake()
    {
        buttonInteractable = GetComponent<XRBaseInteractable>();

        // 현재 위치를 보존하고 Y 좌표만 버튼 시작 위치로 설정
        startLocalPosition = transform.localPosition;
        startLocalPosition.y = startY;

        transform.localPosition = startLocalPosition;
        targetLocalPosition = startLocalPosition;

        buttonInteractable.selectEntered.AddListener(OnButtonPressed);
        buttonInteractable.selectExited.AddListener(OnButtonReleased);
    }

    private void Update()
    {
        // 버튼을 누르거나 놓았을 때 목표 위치로 이동
        transform.localPosition = Vector3.MoveTowards(
            transform.localPosition,
            targetLocalPosition,
            (isPressed ? pressSpeed : returnSpeed) * Time.deltaTime
        );

        // 버튼이 아래 끝까지 내려왔을 때 한 번만 생성
        if (isPressed &&
            !hasSpawned &&
            transform.localPosition.y <= endY + 0.001f)
        {
            SpawnObjects();
            hasSpawned = true;
        }

        // 버튼이 원래 위치로 돌아오면 다음 입력 준비
        if (!isPressed &&
            Vector3.Distance(transform.localPosition, startLocalPosition) < 0.001f)
        {
            transform.localPosition = startLocalPosition;
            hasSpawned = false;
        }
    }

    private void OnButtonPressed(SelectEnterEventArgs args)
    {
        if (hasSpawned)
            return;

        isPressed = true;
        targetLocalPosition = startLocalPosition;
        targetLocalPosition.y = endY;
    }

    private void OnButtonReleased(SelectExitEventArgs args)
    {
        isPressed = false;
        targetLocalPosition = startLocalPosition;
    }

    private void SpawnObjects()
    {
        if (enginePrefab != null)
        {
            int engineCount = Random.Range(1, 3);

            for (int i = 0; i < engineCount; i++)
            {
                Instantiate(
                    enginePrefab,
                    engineSpawnPosition,
                    Quaternion.identity
                );
            }
        }

        if (sparePrefab != null)
        {
            int spareCount = Random.Range(1, 5);

            for (int i = 0; i < spareCount; i++)
            {
                Instantiate(
                    sparePrefab,
                    spareSpawnPosition,
                    Quaternion.identity
                );
            }
        }
    }

    private void OnDestroy()
    {
        if (buttonInteractable != null)
        {
            buttonInteractable.selectEntered.RemoveListener(OnButtonPressed);
            buttonInteractable.selectExited.RemoveListener(OnButtonReleased);
        }
    }
}
