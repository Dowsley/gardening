using Godot;
using System;

public partial class Drop : Node2D
{
    [Export] public float TimeOfExistenceAfterImpact = 0.2f;

    private Sprite _fallingSprite;
    private Sprite _impactSprite;

    private Vector2 _velocity = Vector2.Zero;
    private float _gravity;

    private bool _impacted = false;

    public override void _Ready()
    {
        _fallingSprite = GetNode<Sprite>("FallingSprite");
        _impactSprite = GetNode<Sprite>("ImpactSprite");

        _gravity = Globals.GravityScale; // Ensure this is defined somewhere in your project
        _velocity.y += _gravity;
    }

    public override void _Process(float delta)
    {
        if (!_impacted)
        {
            _velocity.y += Convert.ToSingle(ProjectSettings.GetSetting("physics/2d/default_gravity")) * _gravity * delta;
            Position += _velocity * delta;
        }
    }

    private void _on_CollisionArea_body_entered(Node body)
    {
        if (body.Name == "Floor")
        {
            _impacted = true;
            _fallingSprite.Visible = false;
            _impactSprite.Visible = true;

            var colorRect = body.GetNode<ColorRect>("ColorRect");
            Position = new Vector2(Position.x, colorRect.RectPosition.y - 1);

            var timer = GetTree().CreateTimer(TimeOfExistenceAfterImpact);
            timer.Connect("timeout", this, nameof(OnTimerTimeout));
        }
        if (body is Pot pot)
        {
            pot.AddWater(0.01f);

            _impacted = true;
            QueueFree();
        }
    }

    private void OnTimerTimeout()
    {
        QueueFree();
    }
}
