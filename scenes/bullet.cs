using Godot;
using System;

public partial class bullet : Area2D
{
    private double speed = 200.0;
    private double angularSpeed = 180.0;  // Angular speed in degrees per second
    private Area2D hitbox;

    // Property to store the direction of the bullet
    public Vector2 Direction { get; set; } = Vector2.Zero;

    public override void _Ready()
    {
        hitbox = new Area2D(); // Initialize hitbox if not already initialized
        hitbox.AreaEntered += OnBulletAreaEntered;
        hitbox.AreaExited += OnBulletAreaExited;
    }

    public override void _Process(double delta)
    {
        // Move the bullet in a circular motion
        Rotate((float)(angularSpeed * delta));
        Translate(Direction * (float)(speed * delta));
    }

    // Additional logic for what happens when the bullet enters another area
    private void OnBulletAreaEntered(Area2D otherArea)
    {
        // Handle interactions with other objects (if needed)
    }

    // Additional logic for what happens when the bullet exits another area
    private void OnBulletAreaExited(Area2D otherArea)
    {
        // Handle interactions when the bullet exits an area (if needed)
    }
}
