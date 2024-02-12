using Godot;
using System;

public partial class DraggableRigidBody2D : RigidBody2D
{
    protected bool IsDragging => _isDragging;

    private bool _isDragging = false;
    private Vector2 _previousMousePosition = Vector2.Zero;
    private Vector2 _velocity = Vector2.Zero;

    public override void _Ready()
    {
        AngularDamp = float.MaxValue; // Stops rotation
        Bounce = 0.0f;                // Avoids unnecessary movement

        Friction = 1.0f;
        GravityScale = Globals.GravityScale;

        LinearDamp = Globals.AirResistance;
    }

    public override void _Input(InputEvent @event)
    {
        if (!_isDragging) return;

        if (@event is InputEventMouseButton mouseButtonEvent)
        {
            if (MouseInputUtil.IsLeftMouseButtonReleased(mouseButtonEvent))
            {
                _releaseDrag(mouseButtonEvent);
            }
        }
        else if (@event is InputEventMouseMotion mouseMotionEvent && _isDragging)
        {
            _dragObject(mouseMotionEvent);
        }
    }

    /* This stops the object from rotating */
    public override void _IntegrateForces(Physics2DDirectBodyState state)
    {
        state.AngularVelocity = 0;
        var newTransform = new Transform2D(0, state.Transform.origin);
        state.Transform = newTransform;
    }
    private void _releaseDrag(InputEventMouseButton mouseButtonEvent)
    {
        _previousMousePosition = Vector2.Zero;
        _isDragging = false;
        Mode = ModeEnum.Rigid;

        var localImpulsePoint = ToLocal(mouseButtonEvent.Position).LimitLength(5);
        var isUpsideDown = Mathf.Abs(Mathf.Sin(Rotation)) > 0.707; // sin(45°) to check for upside down
        if (isUpsideDown)
        {
            localImpulsePoint.x = -localImpulsePoint.x;
        }

        var impulse = _velocity * 20;
        ApplyImpulse(localImpulsePoint, impulse.LimitLength(1000));
    }

    private void _dragObject(InputEventMouseMotion mouseMotionEvent)
    {
        var previousPosition = Position;
        Position += mouseMotionEvent.Position - _previousMousePosition;
        _velocity = Position - previousPosition;
        _previousMousePosition = mouseMotionEvent.Position;
    }

    private void _on_DraggableArea_input_event(Viewport viewport, InputEvent @event, int shapeIdx)
    {
        if (@event is InputEventMouseButton mouseButtonEvent)
        {
            if (MouseInputUtil.IsLeftMouseButtonPressed(mouseButtonEvent))
            {
                GetTree().SetInputAsHandled();
                _previousMousePosition = mouseButtonEvent.Position;
                _isDragging = true;
                Mode = ModeEnum.Static;   
            }
        }
    }
}
