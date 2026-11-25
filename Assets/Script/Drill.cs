
using UnityEngine;

public class Drill : MonoBehaviour
{
    [Header("수리할 발전기")]
    public GeneratorRepair generator;

    private void OnTriggerEnter(Collider other)
    {
        if (generator == null)
            return;

        if (other.transform == generator.transform ||
            other.transform.IsChildOf(generator.transform))
        {
            generator.SetDrillTouching(true);
            Debug.Log("드릴 접촉: 수리 가능");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (generator == null)
            return;

        if (other.transform == generator.transform ||
            other.transform.IsChildOf(generator.transform))
        {
            generator.SetDrillTouching(false);
            Debug.Log("드릴 분리: 수리 중단");
        }
    }
}
