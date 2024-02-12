using Godot;
using System;

public partial class WateringCan : DraggableRigidBody2D
{
    [Export] public int DropsPerSecond = 5;
    
    private readonly PackedScene _dropScene = GD.Load<PackedScene>("res://src/core/Drop.tscn");
    private Position2D _dropSpawnPosition;
        
    private float _timeSinceLastSpawn = 0;

    public override void _Ready()
    {
        base._Ready();
        _dropSpawnPosition = GetNode<Position2D>("DropSpawnPosition");
    }
    
    public override void _Process(float delta)
    {
        _timeSinceLastSpawn += delta;

        if (IsDragging && Input.IsMouseButtonPressed((int)ButtonList.Right))
        {
            if (_timeSinceLastSpawn >= (float)(1.0f / (float)DropsPerSecond))
            {
                RotationDegrees = -45; // It will automatically be reverted to 0 due to the way DraggableRigidBody2D works.
                SpawnDrop();
                _timeSinceLastSpawn = 0;   
            }
        }
    }

    private void SpawnDrop()
    {
        var dropInstance = (Node2D)_dropScene.Instance();
        dropInstance.GlobalPosition = _dropSpawnPosition.GlobalPosition;
        GetParent().AddChild(dropInstance);
    }
}
