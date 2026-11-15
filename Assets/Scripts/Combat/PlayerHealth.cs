using UnityEngine;

public class PlayerHealth : Health
{
    [SerializeField] private PlayerRespawn respawn; // з лабораторної 4

    protected override void Die()
    {
        // Гравець не знищується — повертається на контрольну точку
        respawn.Respawn();
        ResetHealth();
    }
}
