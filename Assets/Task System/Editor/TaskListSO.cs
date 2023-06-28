using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Tasks", fileName = "New Task ")]
public class TaskListSO : ScriptableObject
{
    public List<string> Tasks { get; private set; } = new List<string>();

    public void AddTask(string task)
    {
        Tasks.Add(task);
        
        // Save changed file as setting it dirty
        Save();
    }

    private void Save()
    {
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    public void UpdateTasks(List<string> saved)
    {
        Tasks.Clear();
        Tasks = saved;
        Save();
    }
}