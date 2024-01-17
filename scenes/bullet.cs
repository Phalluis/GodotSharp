using Godot;
using System;

public partial class bullet : Area2D
{
	private double angularSpeed = 90.0;  // Angular speed in degrees per second
	private double initialSpeed = 10.0;  // Initial speed
	private double spiralSpeed = 200.0;  // Angular speed for spiraling
	private double radius = 50.0;        // Initial radius
	private AnimationPlayer booletloop;

	// Property to store the direction of the bullet
	public Vector2 Direction { get; set; } = Vector2.Right; // Assuming bullet starts moving to the right

	// Reference to the character (replace 'CharacterBody2D' with the actual class of your character)
	private Sprite2D sprite2D;

	public override void _Ready()
	{
		booletloop = GetNode<AnimationPlayer>("AnimationPlayer");
		booletloop.Play("bulletloop");

		// Set the reference to the character
		sprite2D = GetNode<Sprite2D>("../Sprite2D");
	}

	public override void _Process(double delta)
	{
		// Increase the spiral speed over time
		spiralSpeed += 10.0 * delta;

		// Move the bullet in a circular motion while widening the radius
		Rotate((float)(angularSpeed * delta)); // Angular rotation
		Vector2 newPosition = Position + Direction * (float)(initialSpeed * delta); // Outward translation
		newPosition = newPosition.Rotated((float)Mathf.DegToRad(spiralSpeed * delta)); // Spiraling translation
		Position = newPosition;

		// Update the direction for the next frame
		Direction = Direction.Rotated((float)Mathf.DegToRad(spiralSpeed * delta));

		// Check if the bullet is too far from the character
		if (Position.DistanceTo(sprite2D.Position) > 1000)
		{
			Dissolve(); // or QueueFree() if you want to delete immediately
		}
	}

	private void Dissolve()
	{
		QueueFree();
	}
}
