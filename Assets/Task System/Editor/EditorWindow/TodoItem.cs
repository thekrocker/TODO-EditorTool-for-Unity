using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class TodoItem : VisualElement
{
    public Toggle Toggle { get; private set; }
    public Label Label { get; private set; }

    public TodoItem(string taskText)
    {
        VisualTreeAsset treeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(TodoEditor.Path + "TodoItem.uxml");
        this.Add(treeAsset.Instantiate());

        Toggle = this.Q<Toggle>("taskToggle");
        Label = this.Q<Label>("taskLabel");
        Label.text = taskText;
    }
}
