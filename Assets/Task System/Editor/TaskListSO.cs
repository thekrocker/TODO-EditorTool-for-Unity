using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Tasks", fileName = "New Task ")]
public class TaskListSO : ScriptableObject
{
    [field: SerializeField] public List<string> Tasks { get; private set; } = new List<string>();

    public void Save(List<string> saved)
    {
        Tasks.Clear();
        Tasks = saved;
    }
}
