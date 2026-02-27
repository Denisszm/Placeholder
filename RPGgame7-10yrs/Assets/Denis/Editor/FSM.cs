using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using System.Diagnostics.Eventing.Reader;

[System.Serializable]
class Connections
{
    public int startPoint;
    public int endPoint;
}
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
    private List<Connections> connections = new List<Connections>();
    private bool isDragging = false;
    private Vector2 mouseDownPosition;
#nullable enable annotations //nulls
    private string? lastCardName;
    private string? newCardName;
    private int? lastCardIndex;
    private int? newCardIndex;

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

        for (int i = 0; i < connections.Count; i++)
        {
            if (lastCardIndex != null && newCardIndex != null && Event.current.type == EventType.Repaint)
            {
                Debug.Log("Entered if statement");
                Handles.BeginGUI();
                Vector3 startPos = states[connections[i].startPoint].rect.center;
                Vector3 lastPos = states[connections[i].endPoint].rect.center;

                Handles.DrawLine(startPos, lastPos, 2f);
                Debug.Log("Created Line");

                Handles.EndGUI();
            }
        }

        for (int i = 0; i < states.Count; i++)
        {
            GUI.BeginGroup(states[i].rect);

            GUI.Box(new Rect(0, 0, states[i].rect.width, states[i].rect.height), "");
            float textHeight = 25;
            states[i].name = GUI.TextField(new Rect(10, (states[i].rect.height - textHeight) / 2, states[i].rect.width - 20, textHeight), states[i].name);
            GUI.EndGroup();
            Event e = Event.current;


            Rect CardsArea = states[i].rect;



            if (e.type == EventType.MouseDown && CardsArea.Contains(e.mousePosition))
            {
                dragIndex = i;
                dragOffset = e.mousePosition - states[i].rect.position;
                isDragging = false;

                e.Use();

            }

            if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Delete && CardsArea.Contains(e.mousePosition))
            {
                states.RemoveAt(i);
            }

            if (e.type == EventType.MouseDrag && dragIndex == i)
            {
                if ((e.mousePosition - mouseDownPosition).magnitude > 5f)
                {
                    isDragging = true;
                }
                states[i].rect.position = e.mousePosition - dragOffset;
                Repaint();
            }
            if (e.type == EventType.MouseUp && dragIndex == i)
            {
                lastCardIndex = null;
                newCardIndex = null;
                if (!isDragging)
                {
                    if (lastCardIndex == null)
                    {
                        lastCardIndex = i;
                        Debug.Log($"Selected First: {states[i].name} {i}");
                    }
                    else if (lastCardIndex != i && lastCardIndex != null)
                    {
                        newCardIndex = i;
                        Debug.Log($"Selected Second: {states[i].name} {i}");
                    }
                    connections.Add(new Connections
                    {
                        startPoint = lastCardIndex.Value,
                        endPoint = newCardIndex.Value
                    });
                    
                }
                dragIndex = -1;
            }
        }
        Repaint();
    }
    
}
