using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Coin : MonoBehaviour
{
    [SerializeField] private int value = 1;
    [SerializeField] private IntEventSO coinsChanged;

    [Header("Відгук")]
    [SerializeField] private ParticleSystem pickupEffect; // ПРЕФАБ, не об'єкт зі сцени

    private bool collected; // монета підбирається рівно один раз

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Два колайдери гравця можуть увійти в тригер одного кадру —
        // без цього прапорця монета зарахується двічі
        if (collected) return;

        // Знову не тег, а наявність компонента — як на контрольній точці
        if (!other.TryGetComponent(out PlayerController _)) return;

        collected = true;
        GameProgress.AddCoins(value);
        coinsChanged.Raise(GameProgress.Coins); // монета сама нікого не малює

        // Ефект породжуємо ОКРЕМИМ об'єктом. Якби він був дочірнім
        // до монети, то зник би разом із нею тієї ж миті
        if (pickupEffect != null)
            Instantiate(pickupEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
