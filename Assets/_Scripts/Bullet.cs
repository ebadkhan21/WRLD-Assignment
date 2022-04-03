using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]

public class Bullet : MonoBehaviour
{
    [SerializeField] float ShootForce = 10f;
    [SerializeField] float BulletLifeTime = 3f;
    [SerializeField] float DamageToApply = 40f;

    private bool BulletShot = false;
    private Rigidbody m_rigidBody = null;

    private Transform target = null;

    private void Start()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_rigidBody.velocity = Vector3.zero;
        gameObject.SetActive(false);
    }

    public void Shoot(Transform t)
    {
        target = t;
        transform.LookAt(target.position);
        gameObject.SetActive(true);
        m_rigidBody.AddForce(transform.forward * ShootForce * 100f, ForceMode.Impulse);
        StartCoroutine(CheckLifeTime());
        BulletShot = true;
    }

    private IEnumerator CheckLifeTime()
    {
        yield return new WaitForSeconds(BulletLifeTime);
        if (gameObject.activeSelf)
            BackToPool();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!BulletShot)
            return;
        if (other.collider.TryGetComponent<IHit>(out IHit detectedObject))
            detectedObject.GetHit(DamageToApply);
        BackToPool();
    }

    private void BackToPool()
    {
        StopAllCoroutines();
        BulletShot = false;
        m_rigidBody.velocity = Vector3.zero;
        gameObject.SetActive(false);
        BulletPool.Instance.GetBackIntoPool(this);
    }

}