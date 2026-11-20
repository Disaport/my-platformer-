using UnityEngine;
using UnityEngine.SceneManagement;

// Статичний клас: об'єкта в сцені не потрібно, викликається звідусіль.
// УСІ переходи між сценами — тільки через нього
public static class Flow
{
    public const string MainMenuScene = "MainMenu"; // ім'я в сталій, а не рядком у кнопці

    public static void Go(string sceneName)
    {
        Time.timeScale = 1f; // виходимо з паузи ПЕРЕД завантаженням
        SceneManager.LoadScene(sceneName);
    }

    public static void Go(int buildIndex)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(buildIndex);
    }

    public static void Restart() => Go(SceneManager.GetActiveScene().name);

    public static void NextLevel()
    {
        int next = SceneManager.GetActiveScene().buildIndex + 1;
        if (next < SceneManager.sceneCountInBuildSettings)
        {
            GameProgress.LevelIndex = next;
            SaveSystem.Save(GameProgress.ToData()); // прогрес фіксуємо ПЕРЕД переходом
            Go(next);
        }
        else
        {
            GameProgress.LevelIndex = 0; // рівні скінчились
            SaveSystem.Save(GameProgress.ToData());
            Go(MainMenuScene);
        }
    }

    public static void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // у редакторі Quit нічого не робить
#else
        Application.Quit();
#endif
    }
}
