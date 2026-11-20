using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button continueButton;
    [SerializeField] private string firstLevelScene = "Level01";

    private void Start()
    {
        // «Продовжити» має сенс лише тоді, коли є що продовжувати
        continueButton.interactable = SaveSystem.HasSave;
    }

    public void NewGame()
    {
        SaveSystem.Delete(); // нова гра — це справді з нуля
        GameProgress.Reset();
        Flow.Go(firstLevelScene);
    }

    public void Continue()
    {
        GameProgress.Apply(SaveSystem.Load());
        // 0 у списку збірки — це головне меню. Якщо в сейві нуль,
        // починаємо з першого рівня, а не вантажимо меню саме в себе
        Flow.Go(Mathf.Max(1, GameProgress.LevelIndex));
    }

    public void Quit() => Flow.Quit();
}
