
using UnityEngine;

public class Battery : MonoBehaviour
{
    [Header("배터리 설정")]
    public float batteryAmount = 100f;

    // 배터리량 감소
    public void DecreaseBattery(float amount)
    {
        batteryAmount -= amount;
        batteryAmount = Mathf.Max(0f, batteryAmount);
    }
}
