[System.Serializable]
public class SaveData
{
    public const int CurrentVersion = 2;

    // ЗАВЖДИ перше поле. Без нього старий сейв неможливо ані впізнати,
    // ані полагодити: ви просто не знаєте, що саме в ньому лежить
    public int version = CurrentVersion;

    public int coins;      // був уже у версії 1
    public int levelIndex; // з'явився у версії 2, коли рівнів стало більше ніж один
}
