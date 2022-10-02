using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class EnemySearchPoint : MonoBehaviour
{
    [Range(1, 5.0f)] public int Weight = 1;
    public float LingerTimeMin = 3.0f;
    public float LingerTimeMax = 5.0f;
    public SearchArc[] SearchArcs = new SearchArc[] { new(0.0f, 360.0f) };
    public EnemySearchZone Zone;

    public Color Color => Zone ? Zone.ZoneColor : Color.red;

#if UNITY_EDITOR
    public void OnEnable()
    {
        if (!Zone)
        {
            Zone = GetComponentInParent<EnemySearchZone>();
            if (Zone) Zone.RebuildZone = true;
        }
    }

    public void OnDrawGizmos()
    {
        if (Zone && Zone.AlwaysDraw) Draw();
    }

    public void OnDrawGizmosSelected()
    {
        if (!Zone || !Zone.AlwaysDraw) Draw();
    }

    private void Draw()
    {
        Gizmos.color = Color;
        Gizmos.DrawSphere(transform.position, 0.5f);

        Handles.color = new(Color.r, Color.g, Color.b, 0.25f);
        foreach (SearchArc searchArc in SearchArcs)
        {
            if (searchArc.Span >= 360.0f)
            {
                Handles.DrawSolidDisc(transform.position, Vector2.up, 2.0f);
            }
            else
            {
                Vector3 start = new(Mathf.Sin(Mathf.Deg2Rad * searchArc.AngleMinimum), 0, Mathf.Cos(Mathf.Deg2Rad * searchArc.AngleMinimum));
                if (searchArc.Span == 0) Handles.DrawLine(transform.position, transform.position + start);
                else
                {
                    float angle = searchArc.AngleMaximum - searchArc.AngleMinimum;
                    Handles.DrawSolidArc(transform.position, Vector3.up, start, angle, 2.0f);
                }
            }
        }
    }
#endif

    public void OnValidate()
    {
        foreach (SearchArc arc in SearchArcs) arc.Span = arc.AngleMaximum - arc.AngleMinimum;
    }
}

[Serializable]
public class SearchArc
{
    public float AngleMinimum = 0.0f;
    public float AngleMaximum = 360.0f;
    public float Span;

    public SearchArc(float min, float max)
    {
        AngleMinimum = min;
        AngleMaximum = max;
        Span = AngleMaximum - AngleMinimum;
    }
}
