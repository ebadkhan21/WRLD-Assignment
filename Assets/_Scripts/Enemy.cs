using UnityEngine;
using System.Threading.Tasks;

public class Enemy : MonoBehaviour, IHit
{
    [SerializeField] float Health = 100f;
    [SerializeField] float RespawnTime = 5f;
    [Space]
    [SerializeField] float CurrentHealth = 0f;

    public bool IsAlive { get; private set; }

    private void Start()
    {
        CurrentHealth = Health;
        IsAlive = true;
        gameObject.SetActive(true);
    }

    public void GetHit(float damage)
    {
        CurrentHealth -= damage;
        if (CurrentHealth <= 0f)
        {
            IsAlive = false;
            gameObject.SetActive(false);
            RespawnInTime();
        }
    }

    private async void RespawnInTime()
    {
        await Task.Delay((int)RespawnTime * 1000);
        if (Application.isPlaying)
            Start();
    }

}