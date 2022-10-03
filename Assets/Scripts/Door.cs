using TMPro;
using UnityEngine;

public class Door : MonoBehaviour
{
    public TMP_Text Label1;
    public TMP_Text Label2;
    public EnemySearchZone Zone1;
    public EnemySearchZone Zone2;
    public bool CustomLabel1;
    public bool CustomLabel2;
    [ColorUsage(true, true)] public Color CustomColor1 = 2 * Color.white;
    [ColorUsage(true, true)] public Color CustomColor2 = 2 * Color.white;
    public float ColorIntensity = 2.0f;

    public void OnValidate()
    {
        if (Label1)
        {
            if (Zone1)
            {
                if (!CustomLabel1) Label1.text = Zone1.gameObject.name;
                Label1.color = CustomLabel1 ? CustomColor1 : ColorIntensity * Zone1.ZoneColor;
            }
            else if (!CustomLabel1) Label1.text = "Zone 1";
        }
        if (Label2)
        {
            if (Zone2)
            {
                if (!CustomLabel2) Label2.text = Zone2.gameObject.name;
                Label2.color = CustomLabel2 ? CustomColor2 : ColorIntensity * Zone2.ZoneColor;
            }
            else if (!CustomLabel2) Label2.text = "Zone 2";
        }
    }
}
