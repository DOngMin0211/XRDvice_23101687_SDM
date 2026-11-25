
using UnityEngine;
using TMPro;

public class GeneratorRepair : MonoBehaviour
{
    [Header("발전기 수치")]
    [Range(20f, 100f)]
    public float generatorPower = 100f;

    public float minPower = 20f;
    public float maxPower = 100f;

    [Header("전력 감소 설정")]
    public float decreaseAmount = 8f;
    public float decreaseInterval = 2f;

    [Header("수리 설정")]
    public float repairAmount = 4f;
    public float repairInterval = 0.5f;

    [Header("3D 텍스트 설정")]
    public TextMeshPro powerTMP1;
    public TextMeshPro powerTMP2;

    private float decreaseTimer = 0f;
    private float repairTimer = 0f;

    private bool isDrillTouching = false;
    private bool isRepairButtonHeld = false;

    private void Start()
    {
        generatorPower = Mathf.Clamp(
            generatorPower, minPower, maxPower);

        UpdatePowerUI();
    }

    private void Update()
    {
        // 2초마다 전력 8% 감소
        decreaseTimer += Time.deltaTime;

        if (decreaseTimer >= decreaseInterval)
        {
            decreaseTimer -= decreaseInterval;

            generatorPower -= decreaseAmount;
            generatorPower = Mathf.Clamp(
                generatorPower, minPower, maxPower);

            UpdatePowerUI();
        }

        // 드릴 접촉 + 버튼 누름 상태에서 수리
        if (isDrillTouching && isRepairButtonHeld)
        {
            repairTimer += Time.deltaTime;

            if (repairTimer >= repairInterval)
            {
                repairTimer -= repairInterval;

                generatorPower += repairAmount;
                generatorPower = Mathf.Clamp(
                    generatorPower, minPower, maxPower);

                UpdatePowerUI();
            }
        }
        else
        {
            repairTimer = 0f;
        }
    }

    public void SetDrillTouching(bool touching)
    {
        isDrillTouching = touching;

        if (!touching)
            repairTimer = 0f;
    }

    public void SetRepairButtonHeld(bool held)
    {
        isRepairButtonHeld = held;

        if (!held)
            repairTimer = 0f;
    }

    private void UpdatePowerUI()
    {
        string powerText =
            "Generator : " + generatorPower.ToString("F0") + "%";

        if (powerTMP1 != null)
            powerTMP1.text = powerText;

        if (powerTMP2 != null)
            powerTMP2.text = powerText;
    }
}
