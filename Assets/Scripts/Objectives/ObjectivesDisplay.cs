using System.Text;
using TMPro;
using UnityEngine;

public class ObjectivesDisplay : MonoBehaviour
{
    public TMP_Text Labels;
    public TMP_Text Scores;
    public RoomObjectiveManager[] Rooms;

    private StringBuilder _builder;

    public void Awake()
    {
        _builder = new();
        for (int i = 0; i < Rooms.Length; i++)
        {
            _builder.Append(Rooms[i].gameObject.name);
            _builder.Append(":");
            if (i < Rooms.Length - 1) _builder.Append("\n");
        }
        Labels.text = _builder.ToString();
    }

    public void Update()
    {
        _builder.Clear();
        for (int i = 0; i < Rooms.Length; i++)
        {
            RoomObjectiveManager room = Rooms[i];
            _builder.Append(room.ObjectiveCount - room.ObjectivesRemaining);
            _builder.Append("/");
            _builder.Append(room.ObjectiveCount);
            if (i < Rooms.Length - 1) _builder.Append("\n");
        }
        Scores.text = _builder.ToString();
    }
}
