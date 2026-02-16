using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class FSM : EditorWindow
{
    [MenuItem("Window/UI Toolkit/FSM")]
    public static void ShowExample()
    {
        FSM wnd = GetWindow<FSM>();
        wnd.titleContent = new GUIContent("FSM");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // VisualElements objects can contain other VisualElement following a tree hierarchy.
        VisualElement label = new Label("Hello World! From C#");
        root.Add(label);

    }
}
