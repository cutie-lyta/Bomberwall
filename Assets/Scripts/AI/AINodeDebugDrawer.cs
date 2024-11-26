
using System;
using TMPro;
using UnityEngine;

public class AINodeDebugDrawer : MonoBehaviour
{
    public void Start()
    {
        foreach (Node n in AINodeChooser.Nodes)
        {
            int index = AINodeChooser.Nodes.IndexOf(n);
            var tmp = new GameObject("Node"+index).AddComponent<TextMeshPro>();
            tmp.transform.position = n.Position + (Vector3.up * 1.5f);
            tmp.fontSize = 5;
            tmp.text = index.ToString();
            tmp.transform.rotation = Quaternion.Euler(90, 0, 0);
            tmp.alignment = TextAlignmentOptions.Center;
            tmp.verticalAlignment = VerticalAlignmentOptions.Middle;
        }
    }
}
