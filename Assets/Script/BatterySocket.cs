
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

    private Battery currentBattery;

    private float timer = 0f;

    private void Update()
    {
        if (currentBattery == null)
            return;

        timer += Time.deltaTime;

        if (timer >= decreaseInterval)
        {
            currentBattery.DecreaseBattery(decreaseAmount);

            timer = 0f;
        }

        UpdateBatteryUI();
    }

    // 배터리 장착
    public void OnBatteryInserted(SelectEnterEventArgs args)
    {
        currentBattery = args.interactableObject.transform
            .GetComponent<Battery>();

        timer = 0f;

        UpdateBatteryUI();

        Debug.Log("배터리 장착 완료");
    }

    // 배터리 분리
    public void OnBatteryRemoved(SelectExitEventArgs args)
    {
        currentBattery = null;

        timer = 0f;

        batteryTMP.text = "배터리 미장착";
    }

    // 텍스트 갱신
    private void UpdateBatteryUI()
    {
        if (currentBattery == null)
            return;

        float amount = currentBattery.batteryAmount;

        if (amount <= 0)
        {
            batteryTMP.text = "배터리 방전";
        }
        else
        {
            batteryTMP.text = "Battery : " + amount.ToString("F0") + "%";
        }
    }
}
