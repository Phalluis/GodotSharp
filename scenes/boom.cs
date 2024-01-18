using Godot;
using System;

public partial class boom : Area2D
{
    public static float cdboom;
    private float speed = 400.0f;
    private float lifetime = 2.0f;
    private Timer expirationTimer, expand;
    private Vector2 direction;

    public override void _Ready()
    {
        expirationTimer = new Timer();
        AddChild(expirationTimer);
        expirationTimer.WaitTime = 0.5f;
        expirationTimer.OneShot = true;
        expirationTimer.Timeout += Timeout;
        expirationTimer.Start();

        expand = new Timer();
        AddChild(expand);
        expand.WaitTime = 0.4f;
        expand.OneShot = true;
        expand.Timeout += Expand;
        expand.Start();
    }

    public void SetDirection(Vector2 playerDirection)
    {
        direction = playerDirection.Normalized();
    }

    public override void _PhysicsProcess(double delta)
    {
        Position += direction * speed * (float)delta;
    }

    private void Expand()
    {
        this.Scale = new Vector2(10, 10);
        speed = 0f;
    }

    // Method called when the expiration timer times out
    private void Timeout()
    {
        // Queue-free the bullet after expiration
        QueueFree();
    }
}
