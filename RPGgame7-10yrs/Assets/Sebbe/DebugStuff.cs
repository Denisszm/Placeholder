using UnityEngine;

public sealed class DebugStuff : MonoBehaviour
{
    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 25;
        style.normal.textColor = Color.white;

        string debugText = "INPUT MONITOR:\n";

        for (int i = 1; i <= 2; i++)
        {
            float h = Input.GetAxisRaw("Horizontal" + i);
            bool j = Input.GetButton("Jump" + i);
            bool a = Input.GetButton("Attack" + i);
            bool d = Input.GetButton("Dash" + i);

            debugText += $"\nPLAYER {i}:";
            debugText += $"\n  Stick: {h:F2}";
            debugText += $"\n  Jump: {(j ? "ON" : "OFF")}";
            debugText += $"\n  Attack: {(a ? "ON" : "OFF")}";
            debugText += $"\n  Dash: {(d ? "ON" : "OFF")}\n";
        }

        GUI.Label(new Rect(20, 20, 400, 600), debugText, style);
    }
}