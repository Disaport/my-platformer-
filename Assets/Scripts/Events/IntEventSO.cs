using System;
using UnityEngine;

// Канал подій як ассет: одна сторона надсилає число, інша слухає.
// Жодна з них не має посилання на іншу — лише на цей ассет
[CreateAssetMenu(menuName = "Game/Events/Int Event", fileName = "NewIntEvent")]
public class IntEventSO : ScriptableObject
{
    // private event, а не public Action: ззовні можна тільки підписатись
    // і відписатись, але не можна ані очистити список, ані викликати його
    private event Action<int> listeners;

    public void Raise(int value) => listeners?.Invoke(value);
    public void Register(Action<int> callback) => listeners += callback;
    public void Unregister(Action<int> callback) => listeners -= callback;

    // Ассет переживає зупинку гри в редакторі. Без цього рядка підписки
    // з попереднього запуску лишаються в списку й стріляють у мертві об'єкти
    private void OnDisable() => listeners = null;
}
