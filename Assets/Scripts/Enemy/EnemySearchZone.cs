using UnityEngine;

[ExecuteInEditMode]
public class EnemySearchZone : MonoBehaviour
{
    [Header("Zone Config")]
    public int SearchLengthMin = 3;
    public int SearchLengthMax = 7;
    public Color ZoneColor = Color.black;
    public bool RebuildZone;

    [Header("Read Only - Search Points")]
    public EnemySearchPoint[] SearchPoints;
    public Bounds ZoneBounds;

#if UNITY_EDITOR
    public void Update()
    {
        if (RebuildZone)
        {
            FindSearchPoints();
            RebuildZone = false;
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = ZoneColor;
        Gizmos.DrawWireCube(ZoneBounds.center, ZoneBounds.size);
    }
#endif

    public void FindSearchPoints()
    {
        SearchPoints = GetComponentsInChildren<EnemySearchPoint>();
        if (SearchPoints.Length > 0)
        {
            ZoneBounds = new Bounds(SearchPoints[0].transform.position, Vector3.zero);
        }

        foreach (EnemySearchPoint searchPoint in SearchPoints)
        {
            ZoneBounds.Encapsulate(searchPoint.transform.position);
            searchPoint.Zone = this;
        }
        ZoneBounds.Encapsulate(ZoneBounds.min - new Vector3(1, 0, 1));
        ZoneBounds.Encapsulate(ZoneBounds.max + new Vector3(1, 0, 1));
    }
}
