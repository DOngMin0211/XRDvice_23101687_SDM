
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using TMPro;

public class BatterySocket : MonoBehaviour
{
    [Header("배터리 감소 설정")]
    public float decreaseAmount = 3f;
    public float decreaseInterval = 1f;

    [Header("3D 텍스트 설정")]
    public TextMeshPro batteryTMP;

    [Header("올바른 배터리 설정")]
    public Battery correctBattery;

    private Battery currentBattery;
    private float timer = 0f;
    private bool isBatteryActive = false;

    private void Start()
    {
        // 시작할 때는 비활성 상태
        isBatteryActive = false;

        if (batteryTMP != null)
            batteryTMP.text = "배터리 미장착";
    }

    private void Update()
    {
        // 올바른 배터리가 장착되지 않았다면 작동 중지
        if (!isBatteryActive || currentBattery == null)
            return;

        // 배터리가 방전되면 감소 중지
        if (currentBattery.batteryAmount <= 0f)
        {
            UpdateBatteryUI();
            return;
        }

        timer += Time.deltaTime;

        if (timer >= decreaseInterval)
        {
            currentBattery.DecreaseBattery(decreaseAmount);
            timer = 0f;

            UpdateBatteryUI();
        }
    }

    // 배터리 장착 이벤트
    public void OnBatteryInserted(SelectEnterEventArgs args)
    {
        Battery insertedBattery =
            args.interactableObject.transform.GetComponent<Battery>();

        // 배터리 스크립트가 없으면 무시
        if (insertedBattery == null)
            return;

        // 지정한 배터리와 다르면 무시
        if (correctBattery != null && insertedBattery != correctBattery)
        {
            Debug.Log("올바르지 않은 배터리입니다.");
            return;
        }

        currentBattery = insertedBattery;
        isBatteryActive = true;
        timer = 0f;

        UpdateBatteryUI();

        Debug.Log("올바른 배터리 장착! 시스템 활성화");
    }

    // 배터리 분리 이벤트
    public void OnBatteryRemoved(SelectExitEventArgs args)
    {
        Battery removedBattery =
            args.interactableObject.transform.GetComponent<Battery>();

        // 현재 장착된 배터리가 분리된 경우에만 비활성화
        if (removedBattery != currentBattery)
            return;

        currentBattery = null;
        isBatteryActive = false;
        timer = 0f;

        if (batteryTMP != null)
            batteryTMP.text = "No Battery";

        Debug.Log("배터리 분리! 시스템 비활성화");
    }

    // 배터리 텍스트 갱신
    private void UpdateBatteryUI()
    {
        if (batteryTMP == null || currentBattery == null)
            return;

        float amount = currentBattery.batteryAmount;

        if (amount <= 0f)
            batteryTMP.text = "배터리 방전";
        else
            batteryTMP.text = "Battery : " + amount.ToString("F0") + "%";
    }
}
