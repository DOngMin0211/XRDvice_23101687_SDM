
using UnityEngine;
using UnityEngine.InputSystem;

public class DrillRepair : MonoBehaviour
{
    [Header("수리할 발전기")]
    public GeneratorRepair generator;

    [Header("드릴 수리 버튼")]
    public InputActionProperty repairAction;

    private void OnEnable()
    {
        repairAction.action?.Enable();
    }

    private void OnDisable()
    {
        if (generator != null)
            generator.SetRepairButtonHeld(false);

        repairAction.action?.Disable();
    }

    private void Update()
    {
        if (generator == null || repairAction.action == null)
            return;

        bool isPressed = repairAction.action.IsPressed();

        generator.SetRepairButtonHeld(isPressed);
    }
}
