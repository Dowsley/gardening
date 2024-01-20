using Godot;
using System;

public class Cup : RigidBody2D
{
    private bool _isDragging = false;

    private Vector2 _previousMousePosition = new Vector2();
    private Vector2 _velocity = Vector2.Zero;

    public override void _Ready()
    {
        // Initialization code if needed
    }
    
    public override void _Input(InputEvent @event)
    {
        if (!_isDragging)
        {
            return;
        }

        var previousPosition = Position;

        // Release
        if (@event is InputEventMouseButton mouseButtonEvent && !mouseButtonEvent.Pressed && mouseButtonEvent.ButtonIndex == (int)ButtonList.Left)
        {
            _previousMousePosition = new Vector2();
            _isDragging = false;
            Mode = ModeEnum.Rigid;

            var localImpulsePoint = ToLocal(mouseButtonEvent.Position);
            localImpulsePoint = localImpulsePoint.LimitLength(5);

            bool isUpsideDown = Mathf.Abs(Mathf.Sin(Rotation)) > 0.707; // Approximately checks for upside down -> sin(45°)
            if (isUpsideDown)
            {
                localImpulsePoint.x = -localImpulsePoint.x;
            }

            var impulse = _velocity * 20;
            ApplyImpulse(localImpulsePoint, impulse.LimitLength(1000));
        }

        if (_isDragging && @event is InputEventMouseMotion mouseMotionEvent)
        {
            Position += mouseMotionEvent.Position - _previousMousePosition;
            _velocity = Position - previousPosition;
            _previousMousePosition = mouseMotionEvent.Position;
        }
    }

    private void _on_Draggable_input_event(Viewport viewport, InputEvent @event, int shapeIdx)
    {
        if (@event is InputEventMouseButton mouseButtonEvent && mouseButtonEvent.Pressed && mouseButtonEvent.ButtonIndex == (int)ButtonList.Left)
        {
            GetTree().SetInputAsHandled();
            _previousMousePosition = mouseButtonEvent.Position;
            _isDragging = true;
            Mode = ModeEnum.Static;
        }
    }
}
