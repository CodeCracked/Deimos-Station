using UnityEngine;

[ExecuteInEditMode]
public class VisionConeTarget : MonoBehaviour
{
    public Transform[] TargetPoints;

    public void OnEnable() { VisionCone.AddTarget(this); }
    public void OnDisable() { VisionCone.RemoveTarget(this); }

    public void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        foreach (Transform targetPoint in TargetPoints) if (targetPoint) Gizmos.DrawSphere(targetPoint.transform.position, 0.1f);
    }
}
