using Godot;
using System;

public class Cup : Node2D
{
    private Vector2 _previousMousePosition = new Vector2();
    private bool _isDragging = false;
    

    // public override void _Ready()
    // {
    //     Connect("input_event", this, nameof(_on_Draggable_input_event));
    // }

    private void _on_Draggable_input_event(Viewport viewport, InputEvent @event, int shapeIdx)
    {
        // Check if the event is a touch or mouse press inside the collision shape.
        if (@event is InputEventMouseButton mouseButtonEvent && mouseButtonEvent.Pressed && mouseButtonEvent.ButtonIndex == (int)ButtonList.Left)
        {
            GetTree().SetInputAsHandled();
            _previousMousePosition = mouseButtonEvent.Position;
            _isDragging = true;
        }
    }

    public override void _Input(InputEvent @event)
    {
        // Return early if not dragging.
        if (!_isDragging)
        {
            return;
        }

        // Check if the event is a mouse button release.
        if (@event is InputEventMouseButton mouseButtonEvent && !mouseButtonEvent.Pressed && mouseButtonEvent.ButtonIndex == (int)ButtonList.Left)
        {
            _previousMousePosition = new Vector2();
            _isDragging = false;
        }

        // Check if the event is a mouse motion.
        if (_isDragging && @event is InputEventMouseMotion mouseMotionEvent)
        {
            Position += mouseMotionEvent.Position - _previousMousePosition;
            _previousMousePosition = mouseMotionEvent.Position;
        }
    }
}