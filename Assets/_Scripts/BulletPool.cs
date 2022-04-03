using UnityEngine;
using System.Collections.Generic;

public class BulletPool : MonoBehaviour
{
    public static BulletPool Instance { get; private set; }
    [SerializeField] int PoolSize = 20;
    [SerializeField] GameObject BulletPrefab = null;
    [SerializeField] Queue<Bullet> Bullets = new Queue<Bullet>();

    private void Awake()
    {
        if (!Instance)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        for (int i = 0; i < PoolSize; i++)
            Bullets.Enqueue(Instantiate(BulletPrefab, transform).GetComponent<Bullet>());
    }

    public void ShootBullet(Transform target, Vector3 position)
    {
        Bullet bullet = Bullets.Dequeue();
        bullet.transform.position = position;
        bullet.Shoot(target);
    }

    public void GetBackIntoPool(Bullet bullet) => Bullets.Enqueue(bullet);

}