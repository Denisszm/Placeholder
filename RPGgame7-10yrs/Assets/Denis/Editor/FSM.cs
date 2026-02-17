using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using System;
using Unity.VisualScripting;

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
    private Rect? lastCard;
    private Rect? newCard;
    private Vector2 lastCardPosition;
    private Vector2 newCardPosition;
#nullable enable annotations //string null
    private string? lastCardName;
    private string? newCardName;

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
            void OnDrawGizmos()
            {
                Gizmos.color = Color.white;
                Gizmos.DrawLine(lastCardPosition, newCardPosition);
            }
            Event e = Event.current;

            
            Rect CardsArea = states[i].rect;

            
            
            if ( e.type == EventType.MouseDown && CardsArea.Contains(e.mousePosition))
            {
                dragIndex = i;
                dragOffset = e.mousePosition - states[i].rect.position;
                if (lastCard != null && lastCard != newCard)
                {
                    newCard = states[i].rect;
                    newCardName = states[i].name;
                    newCardPosition = states[i].rect.position;
                    Debug.Log($"Last Card: {states[i].name} {lastCard} New Card: {states[i].name} {newCard}");
                    lastCard = null;
                    lastCardName = null;
                }
                else
                {
                    
                    lastCard = states[i].rect;
                    lastCardName = states[i].name;
                    lastCardPosition = states[i].rect.position;
                    newCard = null;
                    newCardName = null;
                    Debug.Log($"Last Card: {states[i].name} {lastCard}");
                }
                
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
