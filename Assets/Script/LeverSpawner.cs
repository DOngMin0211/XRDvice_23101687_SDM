using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(HingeJoint))]
[RequireComponent(typeof(XRGrabInteractable))]
public class LeverSpawner : MonoBehaviour
{
    [Header("생성할 프리팹 연결")]
    public GameObject sparePrefab;   // 스페어 오브젝트 프리팹
    public GameObject enginePrefab;  // Engine 오브젝트 프리팹

    [Header("생성 위치 설정")]
    public Vector3 spareSpawnPos = new Vector3(-5f, 5.7f, 5f);
    public Vector3 engineSpawnPos = new Vector3(-3.5f, 5.5f, -0.6f);
    public float spawnSpread = 0.5f; // 여러 개 생성 시 서로 겹치지 않게 퍼지는 반경

    [Header("레버 작동 설정")]
    public float triggerAngle = 40f; // 레버가 작동하는 목표 각도 (40도 이상 당기면 작동)

    private HingeJoint hinge;
    private XRGrabInteractable grabInteractable;
    private bool hasTriggered = false; // 한 번 당길 때 중복 생성 방지

    void Awake()
    {
        hinge = GetComponent<HingeJoint>();
        grabInteractable = GetComponent<XRGrabInteractable>();
    }

    void Update()
    {
        // 1. 컨트롤러로 레버를 잡고 있는 상태일 때만 체크
        if (grabInteractable.isSelected)
        {
            // 현재 레버가 꺾인 각도 확인 (절댓값으로 방향 무관하게 체크)
            float currentAngle = Mathf.Abs(hinge.angle);

            // 목표 각도 이상으로 당겼고, 아직 생성하지 않았다면 실행
            if (currentAngle >= triggerAngle && !hasTriggered)
            {
                hasTriggered = true;
                SpawnObjects();
            }
        }
        else
        {
            // 2. 손을 놓아서 레버가 제자리(5도 이하)로 돌아오면 다시 당길 수 있게 초기화
            if (Mathf.Abs(hinge.angle) < 5f)
            {
                hasTriggered = false;
            }
        }
    }

    private void SpawnObjects()
    {
        // 스페어 오브젝트 1 ~ 4개 랜덤 생성 (Random.Range의 int 최대값은 미포함이므로 5 입력)
        int spareCount = Random.Range(1, 5);
        for (int i = 0; i < spareCount; i++)
        {
            if (sparePrefab != null)
            {
                // 한 좌표에 겹쳐서 튕겨 나가지 않도록 약간씩 위치를 분산시킴
                Vector3 randomOffset = new Vector3(
                    Random.Range(-spawnSpread, spawnSpread),
                    i * 0.3f, // 위로 살짝씩 쌓이듯 생성
                    Random.Range(-spawnSpread, spawnSpread)
                );
                Instantiate(sparePrefab, spareSpawnPos + randomOffset, Quaternion.identity);
            }
        }

        // Engine 오브젝트 1 ~ 2개 랜덤 생성 (최대값 미포함이므로 3 입력)
        int engineCount = Random.Range(1, 3);
        for (int i = 0; i < engineCount; i++)
        {
            if (enginePrefab != null)
            {
                Vector3 randomOffset = new Vector3(
                    Random.Range(-spawnSpread, spawnSpread),
                    i * 0.5f,
                    Random.Range(-spawnSpread, spawnSpread)
                );
                Instantiate(enginePrefab, engineSpawnPos + randomOffset, Quaternion.identity);
            }
        }
    }
}
