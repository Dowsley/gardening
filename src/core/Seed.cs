using Godot;
using System;

public partial class Seed : DraggableRigidBody2D
{
    public override void _Ready()
    {
        base._Ready();
    }
    
    private void _on_PotCollisionArea_area_shape_entered(int areaId, Node area, int areaShapeIndex, int localShapeIndex)
    {
        Node body = area.GetParent();
        if (body is Pot pot && !pot.HasSeed)
        {
            pot.Seed();
            QueueFree();
        }
    }
}
