using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Drawing.Text;
using UnityEditor.Overlays;

public class FSMVisualisation : EditorWindow
{
    private int dragIndex = -1;
    private Vector2 dragOffset;

    private List<StateBox> states = new List<StateBox>();
    private List<Connections> connections = new List<Connections>();
    public Dictionary<string, bool> stateSwitch = new Dictionary<string, bool>();

    private bool isDragging = false;
    private int selectedState = -1;

    private float lastEditedTime = 0;
    private string lastFrameText = "";
    private const float focusTimeout = 5f;

    private GameObject currentObject;
    private FSMHolder currentHolder;
    int stateCounter = 0;

    [MenuItem("Window/FSM")]
    public static void ShowExample()
    {
        GetWindow<FSMVisualisation>("Finite State Machine");
    }

    private void OnGUI()
    {
        if (Selection.activeGameObject != currentObject)
        {
            OnSelectionChanged();
        }
        HandleFocusTimeout();
        DrawToolbar();
        DrawConnections();
        DrawStates();
        HandleInput();
        CreateBools();
        SaveData();
    }
    void SaveData()
    {
        void LoadData()
        {
            states = new List<StateBox>(currentHolder.data.states);
            connections = new List<Connections>(currentHolder.data.connections);
        }
    }
    void OnSelectionChanged()
    {
        GameObject obj = Selection.activeGameObject;

        if (obj == null)
        {
            states = new List<StateBox>();
            connections = new List<Connections>();
            return;
        }

        FSMHolder holder = obj.GetComponent<FSMHolder>();

        if (holder == null)
        {
            holder = obj.AddComponent<FSMHolder>();
        }

        if (holder.data == null)
        {
            CreateNewFSMData();
        }

       
        states = holder.data.states;
        connections = holder.data.connections;

        Repaint();
    }
    void CreateNewFSMData()
    {
        string path = $"Assets/FSMData/{currentObject.name}_FSM.asset";

        if (!AssetDatabase.IsValidFolder("Assets/FSMData"))
        {
            AssetDatabase.CreateFolder("Assets", "FSMData");
        }
        Data data = ScriptableObject.CreateInstance<Data>();

        AssetDatabase.CreateAsset(data, path );
        AssetDatabase.SaveAssets();

        currentHolder.data = data;
    }
    void LoadData()
    {
        states = new List<StateBox>(currentHolder.data.states);
        connections = new List<Connections>(currentHolder.data.connections);
    }
    void HandleFocusTimeout()
    {
        if (!string.IsNullOrEmpty(GUI.GetNameOfFocusedControl()))
        {
            float timeSilent = (float)EditorApplication.timeSinceStartup - lastEditedTime;
            if (timeSilent > focusTimeout)
            {
                GUI.FocusControl(null);
                Repaint();
            }

            
        }
    }
    void DrawToolbar()
    {
        if (GUILayout.Button("Create New Empty State"))
        {
            string uniqueStateName = GetUniqueStateName("NewState");
            StateBox newState = new StateBox
            {
                rect = new Rect(200, 200, 200, 100),
                name = "NewState"
            };


            CreateStateScript(newState);
            states.Add(newState);
        }
    }
    string GetUniqueStateName(string baseName)
    {
        int counter = 1;
        string newName = baseName;

        while (states.Exists(s => s.name == newName))
        {
            newName = baseName + counter;
            counter++;
        }

        return newName;
    }

    void DrawConnections()
    {
        Handles.BeginGUI();

        for (int i = 0; i < connections.Count; i++)
        {
            Connections connection = connections[i];

            if (connection.from >= states.Count || connection.to >= states.Count)
                continue;

            Rect fromRect = states[connection.from].rect;
            Rect toRect = states[connection.to].rect;

            Vector3 startPos = fromRect.center;
            Vector3 endPos = toRect.center;
            if (startPos.x < endPos.x)
            {
                
                endPos = new Vector3(toRect.xMin, toRect.center.y, 0);
            }
            else
            {
                
                endPos = new Vector3(toRect.xMax, toRect.center.y, 0);
            }

            float distance = Math.Abs(endPos.x - startPos.x);
            float tangentOffset = Math.Clamp(distance * 0.5f, 50f, 200f);

            Vector3 startTangent;
            Vector3 endTangent;

            if (startPos.x < endPos.x)
            {
                startTangent = startPos + Vector3.right * tangentOffset;
                endTangent = endPos + Vector3.left * tangentOffset;
            }
            else
            {
                startTangent = (startPos + Vector3.left * tangentOffset);
                endTangent = (endPos + Vector3.right * tangentOffset);
            }

            Handles.color = Color.rebeccaPurple;

            Vector3 direction = (endPos - startPos).normalized;
            direction.y = 0;
            direction = direction.normalized;
            if (direction == Vector3.zero) direction = Vector3.right;
            Vector3 perpendicular = new Vector3(-direction.y, direction.x, 0);
            float arrowLength = 15f;
            float arrowWidth = 8f;
            Vector3 arrowTip = endPos;
            Vector3 baseCenter = arrowTip - direction * arrowLength;
            Vector3 left = baseCenter + perpendicular * arrowWidth;
            Vector3 right = baseCenter - perpendicular * arrowWidth;

            Handles.DrawBezier(startPos, endPos, startTangent, endTangent, Color.white, null, 10f);
            Handles.DrawAAConvexPolygon(arrowTip, left, right);

        }

        Handles.EndGUI();
    }

    void DrawStates()
    {
        for (int i = 0; i < states.Count; i++)
        {
            GUI.BeginGroup(states[i].rect);

            GUI.Box(new Rect(0, 0, states[i].rect.width, states[i].rect.height), "");

            float textHeight = 25;
            string currentName = states[i].name;

            states[i].name = GUI.TextField(
                new Rect(10, (states[i].rect.height - textHeight) / 2, states[i].rect.width - 20, textHeight),states[i].name);

            if (states[i].name != currentName)
            {
                lastEditedTime = (float)EditorApplication.timeSinceStartup;
            }

            GUI.EndGroup();

        }
    }

    void HandleInput()
    {
        Event e = Event.current;

        if (e.type == EventType.KeyDown &&
          (e.keyCode == KeyCode.Return || e.keyCode == KeyCode.KeypadEnter || e.keyCode == KeyCode.Escape))
        {
            GUI.FocusControl(null);
            e.Use();
        }

        if ( e.type == EventType.MouseDown && e.clickCount == 2)
        {
            for ( int i = 0; i < states.Count; i++ )
            {
                if (states[i].rect.Contains(e.mousePosition))
                {
                    if (states[i].stateScript != null)
                    {
                        AssetDatabase.OpenAsset(states[i].stateScript);
                    }
                    else
                    {
                        Debug.Log($"No script assigned to{states[i]}.");
                    }
                    e.Use();
                }
            }
        }

        for (int i = 0; i < states.Count; i++)
        {
            Rect cardArea = states[i].rect;

            if (e.type == EventType.MouseDown && cardArea.Contains(e.mousePosition))
            {
                dragIndex = i;
                dragOffset = e.mousePosition - states[i].rect.position;
                isDragging = false;

                e.Use();
            }

            if (e.type == EventType.MouseDrag && dragIndex == i)
            {
                states[i].rect.position = e.mousePosition - dragOffset;
                isDragging = true;
                Repaint();
            }

            if (e.type == EventType.MouseUp && dragIndex == i)
            {
                if (!isDragging)
                {
                    if (selectedState == -1)
                    {
                        selectedState = i;
                    }
                    else if (selectedState != i)
                    {
                        connections.Add(new Connections(selectedState, i));
                        selectedState = -1;
                    }
                }

                dragIndex = -1;
                isDragging = false;
            }

            if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Delete && cardArea.Contains(e.mousePosition))
            {
                DeleteState(i);
                break;
            }
        }
    }

    void DeleteState(int index)
    {
        states.RemoveAt(index);

        connections.RemoveAll(c => c.from == index || c.to == index);

        for (int i = 0; i < connections.Count; i++)
        {
            if (connections[i].from > index)
                connections[i].from--;

            if (connections[i].to > index)
                connections[i].to--;
        }
    }
    void CreateBools()
    {
        for ( int i = 0; i < connections.Count; ++i)
        {
            Connections bools = connections[i];
            string boolName = $"is{states[bools.to].name}";
            
            if ( !stateSwitch.ContainsKey(boolName))
            {
                stateSwitch.Add(boolName, false);
            }
        }
    }
    void CreateStateScript(StateBox state)
    {
        string folder = "Assets/FSMStates";

        if (!AssetDatabase.IsValidFolder(folder))
            AssetDatabase.CreateFolder("Assets", "FSMStates");

        string className = state.name;
        string path = $"{folder}/{className}.cs";

        string code = $@"using UnityEngine;

public class {className} : MonoBehaviour
{{
    public void Enter() {{ }}
    public void Tick() {{ }}
    public void Exit() {{ }}
}}";


        System.IO.File.WriteAllText(path, code);
        AssetDatabase.Refresh();
        EditorApplication.delayCall += () =>
        {
            state.stateScript = AssetDatabase.LoadAssetAtPath<MonoScript>(path);

            ApplyToSelectedGameObject(); 
        };
    }
    void ApplyToSelectedGameObject()
    {
        GameObject obj = Selection.activeGameObject;

        if (obj == null)
        {
            Debug.LogWarning("No GameObject selected");
            return;
        }

        foreach (var state in states)
        {
            if (state.stateScript == null)
            {
                continue;
            }

            
            Type type = state.stateScript.GetClass();

            if (type == null)
            {
                Debug.LogWarning($"Script not compiled yet: {state.name}");
                continue;
            }

            
            if (obj.GetComponent(type) == null)
            {
                obj.AddComponent(type);
            }
        }

        Debug.Log("FSM applied to " + obj.name);
    }
}

