using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class TodoEditor : EditorWindow
{
    private const string Path = "Assets/Task System/Editor/EditorWindow/";

    private VisualElement _rootContainer;

    [MenuItem("Tools/To-do List")]
    static void OpenWindow()
    {
        TodoEditor window = GetWindow<TodoEditor>();
        window.titleContent = new GUIContent("To-do List");
        window.minSize = new Vector2(500f, 500f);
    }

    private void CreateGUI()
    {
        // Set our root element
        InitializeContainer();
    }

    private void InitializeContainer()
    {
        _rootContainer = rootVisualElement;

        // Get and load our UXML we've created.
        VisualTreeAsset treeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(Path + "TodoEditor.uxml");

        // Add it to the container by instantiating the tree asset
        _rootContainer.Add(treeAsset.Instantiate());

        // Get and load our Stylesheet
        StyleSheet styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(Path + "TodoEditor.uss");

        // Add it our container's stylesheet set.
        _rootContainer.styleSheets.Add(styleSheet);
    }
}