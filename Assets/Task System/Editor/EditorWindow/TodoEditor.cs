using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class TodoEditor : EditorWindow
{
    private const string Path = "Assets/Task System/Editor/EditorWindow/";

    // Our root container
    private VisualElement _rootContainer;

    // Field that represents to add tasks
    private TextField _addTaskTextField;

    // Add Task Button
    private Button _addTaskButton;

    // Scrollview that shows our current tasks
    private ScrollView _taskListScrollView;

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

        // Referencing through Query.
        _addTaskTextField = _rootContainer.Q<TextField>("taskTextField");
        _addTaskButton = _rootContainer.Q<Button>("addTaskButton");
        _taskListScrollView = _rootContainer.Q<ScrollView>("taskListScrollView");

        // Assign on click event to add tasks
        _addTaskButton.clicked += AddTask;
    }

    private void AddTask()
    {
        Debug.Log("Adding task");
    }
}