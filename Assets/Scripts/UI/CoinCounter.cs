using TMPro;
using UnityEngine;

public class CoinCounter : MonoBehaviour
{
    [SerializeField] private IntEventSO coinsChanged;
    [SerializeField] private TMP_Text label;

    private void OnEnable() => coinsChanged.Register(Redraw);
    private void OnDisable() => coinsChanged.Unregister(Redraw);

    private void Redraw(int value) => label.text = value.ToString();
}
