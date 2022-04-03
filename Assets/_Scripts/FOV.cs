using UnityEngine;

public class FOV : MonoBehaviour
{
    [SerializeField] float AngleRange = 30f;
    [SerializeField] float MinDistance = 30f;

    public bool CheckTarget(Transform target)
    {
        float angle = Vector3.Angle(transform.forward, new Vector3(target.position.x, transform.position.y, target.position.z) - transform.position);
        if (Mathf.Abs(angle) < AngleRange)
        {
            if (Vector3.Distance(transform.position, target.position) < MinDistance)
                return true;
        }
        return false;
    }

}

