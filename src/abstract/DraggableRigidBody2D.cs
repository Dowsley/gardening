using Godot;
using System;

public partial class DraggableRigidBody2D : RigidBody2D
{
    private bool isDragging = false;
    private Vector2 previousMousePosition = Vector2.Zero;
    private Vector2 velocity = Vector2.Zero;

    public override void _Input(InputEvent @event)
    {
        if (!isDragging) return;

        if (@event is InputEventMouseButton mouseButtonEvent)
        {
            if (!mouseButtonEvent.Pressed && mouseButtonEvent.ButtonIndex == (int)ButtonList.Left)
            {
                ReleaseDrag(mouseButtonEvent);
            }
        }
        else if (@event is InputEventMouseMotion mouseMotionEvent && isDragging)
        {
            DragObject(mouseMotionEvent);
        }
    }

    private void ReleaseDrag(InputEventMouseButton mouseButtonEvent)
    {
        previousMousePosition = Vector2.Zero;
        isDragging = false;
        Mode = ModeEnum.Rigid;

        var localImpulsePoint = ToLocal(mouseButtonEvent.Position).LimitLength(5);
        var isUpsideDown = Mathf.Abs(Mathf.Sin(Rotation)) > 0.707; // sin(45°) to check for upside down
        if (isUpsideDown)
        {
            localImpulsePoint.x = -localImpulsePoint.x;
        }

        var impulse = velocity * 20;
        ApplyImpulse(localImpulsePoint, impulse.LimitLength(1000));
    }

    private void DragObject(InputEventMouseMotion mouseMotionEvent)
    {
        var previousPosition = Position;
        Position += mouseMotionEvent.Position - previousMousePosition;
        velocity = Position - previousPosition;
        previousMousePosition = mouseMotionEvent.Position;
    }

    private void _on_DraggableArea_input_event(Viewport viewport, InputEvent @event, int shapeIdx)
    {
        if (@event is InputEventMouseButton mouseButtonEvent && mouseButtonEvent.Pressed && mouseButtonEvent.ButtonIndex == (int)ButtonList.Left)
        {
            GetTree().SetInputAsHandled();
            previousMousePosition = mouseButtonEvent.Position;
            isDragging = true;
            Mode = ModeEnum.Static;
        }
    }
}
