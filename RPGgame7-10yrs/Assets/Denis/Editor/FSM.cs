using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

[System.Serializable]
class StateBox
{
    public Rect rect;
    public string name;
}
public class FSM : EditorWindow
{
    private int dragIndex = -1;
    private Vector2 dragOffset;
    private List<StateBox> states = new List<StateBox>();
    [MenuItem("Window/FSM")]
    public static void ShowExample()
    {
        GetWindow<FSM>("Finite State Machine");
    }

    private void OnGUI()
    {
        
        if (GUILayout.Button("Create New Empty State"))
        {
            Debug.Log("Attempting To Create New State");
            states.Add(new StateBox
            {
                rect = new Rect((Screen.width / 2) - 100, (Screen.height / 2) - 50, 200, 100),
                name = "NewState"
            });
        }

        for( int i = 0; i < states.Count; i++)
        {
            GUI.BeginGroup(states[i].rect);

                GUI.Box(new Rect(0, 0, states[i].rect.width, states[i].rect.height), "");
                float textHeight = 25;
                states[i].name = GUI.TextField(new Rect(10, (states[i].rect.height - textHeight) / 2, states[i].rect.width - 20, textHeight), states[i].name);

            GUI.EndGroup();

            Event e = Event.current;

            
            Rect CardsArea = states[i].rect;
            
            if ( e.type == EventType.MouseDown && CardsArea.Contains(e.mousePosition))
            {
                dragIndex = i;
                dragOffset = e.mousePosition - states[i].rect.position;
                e.Use();
                
            }

            if ( e.type == EventType.KeyDown && e.keyCode == KeyCode.Delete && CardsArea.Contains(e.mousePosition))
            {
                states.RemoveAt(i);
            }
            
            if ( e.type == EventType.MouseDrag && dragIndex == i)
            {
                states[i].rect.position = e.mousePosition - dragOffset;
                Repaint();
            }
            if ( e.type == EventType.MouseUp && dragIndex == i)
            {
                dragIndex = -1;

            }
        }
    }
}
