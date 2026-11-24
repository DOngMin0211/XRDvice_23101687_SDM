
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRSimpleInteractable))]
public class OpenDoor : MonoBehaviour
{
    public DoorCtrl doorCtrl;

    private XRSimpleInteractable interactable;

    private void Awake()
    {
        interactable = GetComponent<XRSimpleInteractable>();
    }

    private void OnEnable()
    {
        interactable.selectEntered.AddListener(OnButtonClicked);
    }

    private void OnDisable()
    {
        interactable.selectEntered.RemoveListener(OnButtonClicked);
    }

    private void OnButtonClicked(SelectEnterEventArgs args)
    {
        Debug.Log("문 버튼이 클릭되었습니다.");

        if (doorCtrl != null)
        {
            doorCtrl.OpenDoor();
        }
        else
        {
            Debug.LogError("DoorCtrl이 연결되지 않았습니다.");
        }
    }
}
