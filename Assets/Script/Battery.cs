
using UnityEngine;

public class Battery : MonoBehaviour
{
    [Header("배터리 설정")]
    public float batteryAmount = 100f;

    // 배터리량 감소
    public void DecreaseBattery(float amount)
    {
        batteryAmount -= amount;

        // 최소값 0으로 제한
        batteryAmount = Mathf.Max(0, batteryAmount);

        Debug.Log("현재 배터리량 : " + batteryAmount);
    }
}
