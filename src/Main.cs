using Godot;
using System.Collections.Generic;

public partial class Main : Node2D
{
    private List<Node2D> _availableSkyBackgrounds = new List<Node2D>();

    private int _currSkyBackground = 0;
    
    public override void _Ready()
    {
        foreach (var child in GetNode("Environment/BG").GetChildren())
        {
            _availableSkyBackgrounds.Add((Node2D)child);
        }
    }

    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("ui_accept"))
        {
            SetNextSkyBackground();
        }
    }

    private void SetNextSkyBackground()
    {
        _availableSkyBackgrounds[_currSkyBackground].Visible = false;
        _currSkyBackground += 1;
        if (_currSkyBackground >= _availableSkyBackgrounds.Count)
        {
            _currSkyBackground = 0;
        }
        _availableSkyBackgrounds[_currSkyBackground].Visible = true;
    }
}