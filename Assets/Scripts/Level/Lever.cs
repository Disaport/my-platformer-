using UnityEngine;

public class Lever : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject target; // платформа, двері, міст
    private bool isOn;

    public void Interact()
    {
        isOn = !isOn;
        target.SetActive(isOn);
    }
}