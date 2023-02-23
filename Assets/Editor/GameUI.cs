using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;


public class GameUI : EditorWindow
{
    [MenuItem("Window/UI Toolkit/GameUI")]
    public static void ShowExample()
    {
        GameUI wnd = GetWindow<GameUI>();
        wnd.titleContent = new GUIContent("GameUI");
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