using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class VisionCone : MonoBehaviour
{
    private static List<VisionConeTarget> _targets = new();
    public static void AddTarget(VisionConeTarget target) { if (!_targets.Contains(target)) _targets.Add(target); }
    public static void RemoveTarget(VisionConeTarget target) { if (_targets.Contains(target)) _targets.Remove(target); }

    public float BlindsightRadius = 3.0f;
    [Range(0.0f, 360.0f)] public float Angle = 50.0f;
    public float Range = 50.0f;
    public LayerMask LayerMask = int.MaxValue;

    private List<VisionConeHit> _resultsCache = new();

#if UNITY_EDITOR
    public void OnDrawGizmosSelected()
    {
        float radius = Range * Mathf.Tan(Mathf.Deg2Rad * Angle * 0.5f);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, BlindsightRadius);
        Gizmos.DrawLine(transform.position, transform.position + transform.up * radius + transform.forward * Range);
        Gizmos.DrawLine(transform.position, transform.position - transform.up * radius + transform.forward * Range);
        Gizmos.DrawLine(transform.position, transform.position + transform.right * radius + transform.forward * Range);
        Gizmos.DrawLine(transform.position, transform.position - transform.right * radius + transform.forward * Range);

        Handles.color = Color.cyan;
        Handles.DrawWireDisc(transform.position + transform.forward * Range, transform.forward, radius);

        CalculateResults(true);
        Gizmos.color = Color.red;
        foreach (VisionConeTarget target in _targets) foreach (Transform targetPoint in target.TargetPoints) Gizmos.DrawLine(transform.position, targetPoint.position);
        Gizmos.color = Color.green;
        foreach (VisionConeHit hit in _resultsCache) Gizmos.DrawLine(transform.position, hit.TargetPoint.position);
    }
#endif

    public List<VisionConeHit> GetResults() { return _resultsCache; }
    public List<VisionConeHit> CalculateResults(bool checkMultiplePoints = true)
    {
        _resultsCache.Clear();

        foreach (VisionConeTarget target in _targets)
        {
            foreach (Transform targetPoint in target.TargetPoints)
            {
                Vector3 heading = (targetPoint.position - transform.position).normalized;
                float angle = Vector3.Angle(transform.forward, heading);
                if (angle <= Angle * 0.5f)
                {
                    Ray ray = new(transform.position, heading);
                    if (Physics.Raycast(ray, out RaycastHit hit, Range, LayerMask.value))
                    {
                        _resultsCache.Add(new VisionConeHit(target, targetPoint, hit, transform.position, transform.forward));
                        if (!checkMultiplePoints) break;
                    }
                }
            }
        }

        return _resultsCache;
    }
}

public class VisionConeHit
{
    public VisionConeTarget Target;
    public GameObject GameObject;
    public Transform TargetPoint;
    public RaycastHit Raycast;
    public float Distance;
    public float Angle;

    public VisionConeHit(VisionConeTarget target, Transform targetPoint, RaycastHit raycast, Vector3 conePosition, Vector3 coneHeading)
    {
        Vector3 heading = (targetPoint.position - conePosition).normalized;

        Target = target;
        GameObject = target.gameObject;
        TargetPoint = targetPoint;
        Raycast = raycast;
        Distance = raycast.distance;
        Angle = Vector3.Angle(coneHeading, heading);
    }
}
