using UnityEngine;

public class LightTrigger : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        VisionConeTarget target = other.GetComponentInChildren<VisionConeTarget>();
        if (target) target.OnEnterLight(this);
    }
    public void OnTriggerExit(Collider other)
    {
        VisionConeTarget target = other.GetComponentInChildren<VisionConeTarget>();
        if (target) target.OnExitLight(this);
    }
}
