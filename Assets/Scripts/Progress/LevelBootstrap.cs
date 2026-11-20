using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelBootstrap : MonoBehaviour
{
    [SerializeField] private IntEventSO coinsChanged;

    private void Start()
    {
        // Рівень знає свій номер — саме він потрапить у сейв на контрольній точці
        GameProgress.LevelIndex = SceneManager.GetActiveScene().buildIndex;

        // HUD уже підписався в OnEnable, тому стартові числа дійдуть
        coinsChanged.Raise(GameProgress.Coins);
    }
}
