using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Shooter : MonoBehaviour
{
    [SerializeField] float FireRate = 0.25f;
    [SerializeField] List<Enemy> Enemies = new List<Enemy>();
    [SerializeField] float CheckDelay = 0.2f;
    [SerializeField] Transform Emitter = null;
    [Space]
    [SerializeField] Transform CurrentTarget = null;
    private float FireTimer = 0f;
    private FOV fov = null;

    private void Start() => fov = GetComponent<FOV>();

    private void Update()
    {
        if (CurrentTarget)
            Shoot();
    }

    private void Shoot()
    {
        FireTimer += Time.deltaTime;
        if (FireTimer >= FireRate)
        {
            FireTimer = 0f;
            BulletPool.Instance.ShootBullet(CurrentTarget, Emitter.position);
        }
    }

    public IEnumerator CustomUpdate()
    {
        while (true)
        {
            for (int i = 0; i < Enemies.Count; i++)
            {
                CurrentTarget = Enemies[i].IsAlive ? (fov.CheckTarget(Enemies[i].transform) ? Enemies[i].transform : null) : null;
                if (CurrentTarget)
                    break;
            }
            yield return new WaitForSeconds(CheckDelay);
        }
    }

    public void AddEnemy(Enemy enemy) => Enemies.Add(enemy);

}