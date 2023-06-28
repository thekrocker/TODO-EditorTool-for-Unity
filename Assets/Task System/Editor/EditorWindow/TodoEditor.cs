using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class TodoEditor : EditorWindow
{
    public const string Path = "Assets/Task System/Editor/EditorWindow/";
    private const string HighlightSelector = "highlight";

    #region UI Elements

    // Our root container
    private VisualElement _rootContainer;

    // Field that represents to add tasks
    private TextField _addTaskTextField;

    // Add Task Button
    private Button _addTaskButton;

    // Scrollview that shows our current tasks
    private ScrollView _taskListScrollView;

    // Object field reference to load tasks 
    private ObjectField _tasksObjectField;

    // Button to load tasks
    private Button _loadTasksButton;

    // Button to save our progress
    private Button _saveProgressButton;

    // Current progress bar to show on-going tasks
    private ProgressBar _taskProgressBar;

    // Search field for tasks
    private ToolbarSearchField _taskSearchField;

    #endregion

    private TaskListSO _taskListSOReference;

    [MenuItem("Tools/To-do List")]
    static void OpenWindow()
    {
        TodoEditor window = GetWindow<TodoEditor>();
        window.titleContent = new GUIContent("To-do List");
        window.minSize = new Vector2(350f, 500f);
    }

    private void CreateGUI()
    {
        // Set our root element
        InitializeContainer();
    }

    private T Get<T>(string refString) where T: VisualElement
    {
        return _rootContainer.Q<T>(refString);
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

        /*_addTaskTextField = _rootContainer.Q<TextField>("taskTextField");
        _addTaskButton = _rootContainer.Q<Button>("addTaskButton");
        _taskListScrollView = _rootContainer.Q<ScrollView>("taskListScrollView");
        _tasksObjectField = _rootContainer.Q<ObjectField>("tasksObjectField");
        _loadTasksButton = _rootContainer.Q<Button>("loadTasksButton");
        _saveProgressButton = _rootContainer.Q<Button>("saveProgressButton");
        _taskProgressBar = _rootContainer.Q<ProgressBar>("taskProgressBar");
        _taskSearchField = _rootContainer.Q<ToolbarSearchField>("taskSearchField");*/

        // Referencing through Query.
        _addTaskTextField = Get<TextField>("taskTextField");
        _addTaskButton = Get<Button>("addTaskButton");
        _taskListScrollView = Get<ScrollView>("taskListScrollView");
        _tasksObjectField = Get<ObjectField>("tasksObjectField");
        _loadTasksButton = Get<Button>("loadTasksButton");
        _saveProgressButton = Get<Button>("saveProgressButton");
        _taskProgressBar = Get<ProgressBar>("taskProgressBar");
        _taskSearchField = Get<ToolbarSearchField>("taskSearchField");

        // Register input event to add tasks with "return" key
        _addTaskTextField.RegisterCallback<KeyDownEvent>(AddTask);
        
        // Register search text change event
        _taskSearchField.RegisterValueChangedCallback(OnSearchTextChanged);

        // Assign on click event to add & load tasks
        _addTaskButton.clicked += AddTask;
        _loadTasksButton.clicked += LoadTasks;
        _saveProgressButton.clicked += SaveProgress;

        _tasksObjectField.objectType = typeof(TaskListSO);
    }

    private void OnSearchTextChanged(ChangeEvent<string> evt)
    {
        string searchValue = evt.newValue.ToLower();

        foreach (VisualElement visualElement in _taskListScrollView.Children())
        {
            // Simple security check for potential error
            TodoItem todoItem = (TodoItem)visualElement;
            if (todoItem == null) continue;

            string formatTodoText = todoItem.Label.text.ToLower();

            if (string.IsNullOrEmpty(searchValue))
            {
                todoItem.visible = true;
                continue;
            }
            
            if (formatTodoText.Contains(searchValue))
            {
                todoItem.visible = true;
            }
            else
            {
                todoItem.visible = false;
            }
        }
    }

    private void SaveProgress()
    {
        if (_taskListSOReference != null)
        {
            List<string> inProgressTasks = new List<string>();

            foreach (VisualElement visualElement in _taskListScrollView.Children())
            {
                // Simple security check for potential error
                TodoItem todoItem = (TodoItem)visualElement;
                if (todoItem == null) continue;

                // If item is not checked, add it to list
                if (!todoItem.Toggle.value) inProgressTasks.Add(todoItem.Label.text);
            }

            _taskListSOReference.UpdateTasks(inProgressTasks);

            // Reload tasks
            LoadTasks();
        }
    }

    // Method to load all the tasks we have
    private void LoadTasks()
    {
        _taskListSOReference = _tasksObjectField.value as TaskListSO;

        if (_taskListSOReference != null)
        {
            _taskListScrollView.Clear();

            foreach (string task in _taskListSOReference.Tasks)
            {
                _taskListScrollView.Add(CreateTodoItem(task));
            }
        }

        UpdateProgressBar();
    }

    private TodoItem CreateTodoItem(string task)
    {
        // Create to-do item within toggle and label and stguff
        TodoItem todoItem = new TodoItem(task);
        
        // Subscribe to the value OnChanged event to update progress
        todoItem.Toggle.RegisterValueChangedCallback(UpdateProgressBar);

        return todoItem;
    }

    private void UpdateProgressBar(ChangeEvent<bool> evt)
    {
        UpdateProgressBar();
    }

    private void AddTask(KeyDownEvent e)
    {
        // If user clicks on enter, simply work as a button
        if (Event.current.Equals(Event.KeyboardEvent("Return"))) AddTask();
    }

    private void AddTask()
    {
        // If the text field is empty, just return
        if (string.IsNullOrEmpty(_addTaskTextField.value)) return;

        // Add toggle to the scroll view
        _taskListScrollView.Add(CreateTodoItem(_addTaskTextField.value));

        // Add the task to the scriptable object & save it
        _taskListSOReference.AddTask(_addTaskTextField.value);

        // Set field text to empty.
        _addTaskTextField.value = "";

        _addTaskTextField.Focus();

        UpdateProgressBar();
    }

    private void UpdateProgressBar()
    {
        int completed = 0;

        foreach (var visualElement in _taskListScrollView.Children())
        {
            TodoItem todoItem = (TodoItem)visualElement;
            if (todoItem == null) continue;
            if (todoItem.Toggle.value)
            {
                completed++;
            }
        }

        int totalCount = _taskListScrollView.childCount;
        float ratio = (float)completed / (float)totalCount;
        _taskProgressBar.value = ratio;

        if (totalCount > 0)
        {
            _taskProgressBar.title = $"{(ratio * 100f):F2}%";
        }
        else
        {
            _taskProgressBar.title = $"0.0%";
        }
    }
}