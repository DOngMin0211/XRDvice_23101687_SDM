
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class ButtonSpawner : MonoBehaviour
{
    [Header("배터리 프리팹")]
    public GameObject batteryPrefab;

    [Header("생성 위치")]
    public Vector3 spawnPosition = new Vector3(4.35f, 3.5f, -1.25f);

    [Header("버튼 위치")]
    public float startY = 0.3f;
    public float endY = 0.2f;

    [Header("버튼 움직임")]
    public float pressSpeed = 5f;
    public float returnSpeed = 4f;

    private XRBaseInteractable buttonInteractable;

    private Vector3 startLocalPosition;
    private Vector3 targetLocalPosition;

    private bool isPressed;
    private float nextSpawnTime = 0f;

    private void Awake()
    {
        buttonInteractable = GetComponent<XRBaseInteractable>();

        startLocalPosition = transform.localPosition;
        startLocalPosition.y = startY;

        transform.localPosition = startLocalPosition;
        targetLocalPosition = startLocalPosition;

        buttonInteractable.selectEntered.AddListener(OnButtonPressed);
        buttonInteractable.selectExited.AddListener(OnButtonReleased);
    }

    private void Update()
    {
        transform.localPosition = Vector3.MoveTowards(
            transform.localPosition,
            targetLocalPosition,
            (isPressed ? pressSpeed : returnSpeed) * Time.deltaTime
        );
    }

    private void OnButtonPressed(SelectEnterEventArgs args)
    {
        isPressed = true;

        targetLocalPosition = startLocalPosition;
        targetLocalPosition.y = endY;

        // 20초가 지나야 배터리 생성 가능
        if (Time.time >= nextSpawnTime)
        {
            if (batteryPrefab != null)
            {
                Instantiate(
                    batteryPrefab,
                    spawnPosition,
                    Quaternion.identity
                );

                nextSpawnTime = Time.time + 20f;
            }
        }
    }

    private void OnButtonReleased(SelectExitEventArgs args)
    {
        isPressed = false;
        targetLocalPosition = startLocalPosition;
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
