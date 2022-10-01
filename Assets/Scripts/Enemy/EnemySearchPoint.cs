using UnityEngine;

public class EnemySearchPoint : MonoBehaviour
{
    [Range(1, 5.0f)] public int Weight = 1;
    public float LingerTimeMin = 3.0f;
    public float LingerTimeMax = 5.0f;
    public EnemySearchZone Zone;

    public void OnDrawGizmos()
    {
        Gizmos.color = Zone ? Zone.ZoneColor : Color.red;
        Gizmos.DrawSphere(transform.position, 0.5f);
    }
}
