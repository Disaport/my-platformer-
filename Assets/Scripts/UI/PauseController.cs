using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PauseController : MonoBehaviour
{
    [SerializeField] private GameObject pausePopup;  // оверлей + панель У ЦІЙ ЖЕ сцені
    [SerializeField] private GameObject firstButton; // фокус для клавіатури й геймпада
    [SerializeField, Range(0f, 1f)] private float pausedVolume = 0.3f;

    // Пауза — це стан ГРИ, а не прихована панель. Тому про неї мусять знати всі
    public static bool IsPaused { get; private set; }

    private PlayerControls controls;

    private void Awake()
    {
        controls = new PlayerControls();
        pausePopup.SetActive(false);
        IsPaused = false;
    }

    private void OnEnable()
    {
        controls.Player.Pause.performed += OnPausePressed;
        controls.Player.Pause.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Pause.performed -= OnPausePressed;
        controls.Player.Pause.Disable();
        // Сцена змінилась — час і звук повертаємо ЗАВЖДИ
        Time.timeScale = 1f;
        AudioListener.volume = 1f;
        IsPaused = false;
    }

    private void OnPausePressed(InputAction.CallbackContext _) => SetPaused(!IsPaused);

    public void SetPaused(bool paused)
    {
        IsPaused = paused;
        Time.timeScale = paused ? 0f : 1f;
        pausePopup.SetActive(paused);
        // Звук паузи не помічає: timeScale на нього не впливає
        AudioListener.volume = paused ? pausedVolume : 1f;
        // Без цього рядка геймпаду й клавіатурі нікуди йти
        if (EventSystem.current != null)
            EventSystem.current.SetSelectedGameObject(paused ? firstButton : null);
    }

    // Методи для кнопок меню
    public void Resume() => SetPaused(false);
    public void Restart() => Flow.Restart();
    public void ToMenu() => Flow.Go(Flow.MainMenuScene);
}
