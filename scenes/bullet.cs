using Godot;
using System;

public partial class bullet : Area2D
{
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

    public override void _Ready()
    {
        bulletLoop = GetNode<AnimationPlayer>("AnimationPlayer");
        bulletLoop.Play("bulletloop");

        // Set the reference to the character
        sprite2D = GetNode<Sprite2D>("../Sprite2D");
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
}
