using Godot;
using System;

public partial class bullet : Area2D
{
    public static double cdbullet = 2;
    private double angularSpeed = 100.0;  // Angular speed in degrees per second
    private double initialSpeed = 20.0;  // Initial speed
    private double spiralSpeed = 200.0;  // Angular speed for spiraling
    private double radius = 50.0;        // Initial radius
    private double maxRadius = 100.0;    // Maximum allowed radius
    private AnimationPlayer bulletLoop;

    // Property to store the direction of the bullet
    public Vector2 Direction { get; set; } = Vector2.Right; // Assuming bullet starts moving to the right

    // Reference to the character (replace 'Sprite2D' with the actual class of your character)
    private Sprite2D sprite2D;

    private bool isMovingAway = true;

    private Timer expirationTimer, expand;  // Timer node for bullet expiration

    public override void _Ready()
    {
        bulletLoop = GetNode<AnimationPlayer>("AnimationPlayer");
        bulletLoop.Play("bulletloop");

        // Set the reference to the character
        sprite2D = GetNode<Sprite2D>("../Sprite2D");

        // Initialize the expiration timer
        expirationTimer = new Timer();
        AddChild(expirationTimer);
        expirationTimer.WaitTime = 10.0f;  // Set the expiration time to 10 seconds
        expirationTimer.OneShot = true;
        expirationTimer.Timeout += Timeout;
        expirationTimer.Start();

        expand = new Timer();
        AddChild(expand);
        expand.WaitTime = 5f;  // Set the expiration time to 5 seconds
        expand.OneShot = true;
        expand.Timeout += Expand;
        expand.Start();
    }

    public override void _Process(double delta)
    {
        // Increase the spiral speed over time
        spiralSpeed += 10.0 * delta;

        // Move the bullet in a circular motion while widening the radius
        Rotate((float)(angularSpeed * delta)); // Angular rotation

        // Calculate the new position without exceeding the maximum radius
        Vector2 newPosition = Position + Direction * (float)(initialSpeed * delta); // Outward translation
        float distanceToCharacter = Position.DistanceTo(sprite2D.Position);

        if (isMovingAway)
        {
            if (distanceToCharacter < maxRadius)
            {
                newPosition = newPosition.Rotated((float)Mathf.DegToRad(spiralSpeed * delta)); // Spiraling translation
            }
            else
            {
                isMovingAway = false;
            }
        }
        else
        {
            // Move the bullet towards the character
            newPosition = newPosition.Rotated((float)Mathf.DegToRad(-spiralSpeed * delta)); // Spiraling translation
        }

        Position = newPosition;

        // Update the direction for the next frame
        Direction = Direction.Rotated((float)Mathf.DegToRad(spiralSpeed * delta));
    }

    private void Expand()
    {
        this.Scale = new Vector2(2,2);
    }

    // Method called when the expiration timer times out
    private void Timeout()
    {
        // Queue-free the bullet after expiration
        QueueFree();
    }
}