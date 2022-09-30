using UnityEngine;

[DefaultExecutionOrder(-750)]
public class Player : MonoBehaviour
{
    public static Player Instance;

    public PlayerController Controller;
    public PlayerMotor Motor;
    public Camera AimCamera;

    public int MaxHealth = 10;
    public int MaxAmmo = 3;

    [HideInInspector] public float Health { get; private set; }
    [HideInInspector] public float Ammo { get; private set; }
    [HideInInspector] public float Score;
    [HideInInspector] public bool Alive => Health > 0;

    public void Awake()
    {
        Instance = this;
        Health = MaxHealth;
    }

    public void Heal(float amount)
    {
        if (Health < MaxHealth) Health += amount;
    }
    public void Hurt(float amount)
    {
        Health -= amount;
        if (!Alive)
        {
            Controller.enabled = false;
            Motor.enabled = false;
            AimCamera.transform.localRotation = Quaternion.Euler(0, AimCamera.transform.localEulerAngles.y, 0);
        }
    }
}