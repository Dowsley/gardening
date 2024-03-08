using Godot;
using System;

public partial class Verlet : Node2D
{
    [Export] public float RopeLength = 30;
    [Export] public float Constrain = 1; // distance between points
    [Export] public Vector2 Gravity = new Vector2(0, 9.8f);
    [Export] public float Dampening = 0.9f;
    [Export] public bool StartPin = true;
    [Export] public bool EndPin = true;
    [Export] public Color RopeColor = Colors.Black;

    private Line2D _line2D;
    private Vector2[] _pos;
    private Vector2[] _posPrev;
    private int _pointCount;

    public override void _Ready()
    {
        _line2D = GetNode<Line2D>("Line2D");
        _pointCount = GetPointCount(RopeLength);
        ResizeArrays();
        InitPosition();
        _line2D.DefaultColor = RopeColor;
    }

    private int GetPointCount(float distance)
    {
        return (int)Math.Ceiling(distance / Constrain);
    }

    private void ResizeArrays()
    {
        _pos = new Vector2[_pointCount];
        _posPrev = new Vector2[_pointCount];
    }

    private void InitPosition()
    {
        for (int i = 0; i < _pointCount; i++)
        {
            _pos[i] = Position + new Vector2(Constrain * i, 0);
            _posPrev[i] = Position + new Vector2(Constrain * i, 0);
        }
        Position = Vector2.Zero;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        var globalMousePosition = GetGlobalMousePosition();
        var localMousePosition = ToLocal(globalMousePosition);
        if (@event is InputEventMouseMotion)
        {
            if (Input.IsActionPressed("click")) //Move start point
                SetStart(localMousePosition);
            if (Input.IsActionPressed("click_right")) //Move start point
                SetLast(localMousePosition);
        }
        else if (@event is InputEventMouseButton && @event.IsPressed())
        {
            if (((InputEventMouseButton)@event).ButtonIndex == (int)ButtonList.Left)
                SetStart(localMousePosition);
            else if (((InputEventMouseButton)@event).ButtonIndex == (int)ButtonList.Right)
                SetLast(localMousePosition);
        }
    }

    public override void _Process(float delta)
    {
        UpdatePoints(delta);
        UpdateConstrain();

        // Send positions to Line2D for drawing
        _line2D.Points = (Vector2[])_pos.Clone();;
    }

    private void SetStart(Vector2 p)
    {
        _pos[0] = p;
        _posPrev[0] = p;
    }

    private void SetLast(Vector2 p)
    {
        _pos[_pointCount - 1] = p;
        _posPrev[_pointCount - 1] = p;
    }

    private void UpdatePoints(float delta)
    {
        for (var i = 0; i < _pointCount; i++)
        {
            if ((i != 0 && i != _pointCount - 1) || (i == 0 && !StartPin) || (i == _pointCount - 1 && !EndPin))
            {
                var velocity = (_pos[i] - _posPrev[i]) * Dampening;
                _posPrev[i] = _pos[i];
                _pos[i] += velocity + (Gravity * delta);
            }
        }
    }

    private void UpdateConstrain()
    {
        for (var i = 0; i < _pointCount; i++)
        {
            if (i == _pointCount - 1)
                return;

            var distance = _pos[i].DistanceTo(_pos[i + 1]);
            var difference = Constrain - distance;
            var percent = difference / distance;
            var vec2 = _pos[i + 1] - _pos[i];

            if (i == 0)
            {
                if (StartPin)
                    _pos[i + 1] += vec2 * percent;
                else
                {
                    _pos[i] -= vec2 * (percent / 2);
                    _pos[i + 1] += vec2 * (percent / 2);
                }
            }
            else if (i + 1 == _pointCount - 1 && EndPin)
            {
                _pos[i] -= vec2 * percent;
            }
            else
            {
                _pos[i] -= vec2 * (percent / 2);
                _pos[i + 1] += vec2 * (percent / 2);
            }
        }
    }
}
