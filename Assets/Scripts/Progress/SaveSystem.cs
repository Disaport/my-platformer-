using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string SavePath => Path.Combine(Application.persistentDataPath, "save.json");
    private static string TempPath => Path.Combine(Application.persistentDataPath, "save.tmp");

    public static bool HasSave => File.Exists(SavePath);

    public static void Save(SaveData data)
    {
        string json = JsonUtility.ToJson(data, prettyPrint: true);

        // 1. Пишемо в ТИМЧАСОВИЙ файл. Якщо тут вимкнуть живлення —
        //    зіпсується він, а старий сейв лишиться цілим
        File.WriteAllText(TempPath, json);

        // 2. Замінюємо старий новим однією операцією файлової системи
        if (File.Exists(SavePath)) File.Replace(TempPath, SavePath, null);
        else File.Move(TempPath, SavePath);
    }

    public static SaveData Load()
    {
        if (!File.Exists(SavePath)) return new SaveData();
        try
        {
            SaveData data = JsonUtility.FromJson<SaveData>(File.ReadAllText(SavePath));
            return Migrate(data ?? new SaveData());
        }
        catch (System.Exception e)
        {
            // Зіпсований сейв — не привід падати. Починаємо спочатку,
            // але чесно пишемо в лог, що сталося
            Debug.LogWarning("Сейв не прочитано: " + e.Message);
            return new SaveData();
        }
    }

    public static void Delete()
    {
        if (File.Exists(SavePath)) File.Delete(SavePath);
    }

    // Старий сейв не викидають і не ламають — його дочитують до поточної версії
    private static SaveData Migrate(SaveData data)
    {
        if (data.version < 2)
        {
            data.levelIndex = 0; // у версії 1 поля просто не існувало
            data.version = 2;
        }
        return data;
    }
}
