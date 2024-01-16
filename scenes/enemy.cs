using Godot;
using System;
using System.Runtime.Serialization;

public partial class enemy : CharacterBody2D
{
	
	CharacterBody2D player;
	float speed = 25.0f; // Adjust the speed as needed
	float distanceThreshold = 200.0f; // Adjust the distance threshold

	public override void _Ready()
	{
		player = GetParent().GetNode<CharacterBody2D>("player");
		Area2D hitbox = this.GetNode<Area2D>("hitbox");
		hitbox.Monitoring = true;
		hitbox.AreaEntered += HandleAreaEntered;
		hitbox.AreaExited += HandleAreaExited;
	}

    public override void _PhysicsProcess(double delta)
	{
		UpdateAnimation();
		Sprite2D sprite2d = this.GetNode<Sprite2D>("Sprite2D");

		// Calculate the direction from the enemy to the player
		Vector2 direction = (player.Position - Position).Normalized();

		// Move the enemy towards the player
		Position += direction * speed * (float)delta;

		// Update the facing direction based on the angle
		UpdateFacingDirection(direction);
	}

	private void UpdateAnimation()
	{
		AnimationPlayer enemyanimations = GetNode<AnimationPlayer>("enemyanim");

		// Calculate the distance between the enemy and the player
		float distanceToPlayer = Position.DistanceTo(player.Position);

		if (distanceToPlayer > distanceThreshold)
		{
			enemyanimations.Play("moving");
			speed = 40f;
		}
		if (distanceToPlayer < distanceThreshold)
		{
			enemyanimations.Play("move");
			speed = 25f;
		}
		if (distanceToPlayer < 50f)
		{
			enemyanimations.Play("move");
			speed = 20f;
		}
	}

	private void UpdateFacingDirection(Vector2 direction)
	{
		Sprite2D sprite2d = this.GetNode<Sprite2D>("Sprite2D");

		float angle = Mathf.RadToDeg(direction.Angle());

		if (angle > -45 && angle <= 45)
		{
			sprite2d.FlipH = false;
		}
		else if ((angle > 45 && angle <= 135) || (angle > -135 && angle <= -45))
		{
			sprite2d.FlipH = true;
		}
		else
		{
			sprite2d.FlipH = true;
		}
	}
	private int areaEnteredResult = 0;

	void HandleAreaEntered(Area2D otherArea)
	{
		// Check if the entered object is a CharacterBody2D
		if (otherArea.GetParent() is CharacterBody2D characterBody2D && characterBody2D.IsInGroup("character"))
		{
			GD.Print("CharacterBody2D Entered: " + characterBody2D.Name);
			// Store the result in the class variable
			areaEnteredResult = 1; 
		}
	}

	private void HandleAreaExited(Area2D area)
    {
        if (area.GetParent() is CharacterBody2D characterBody2D && characterBody2D.IsInGroup("character"))
		{
			GD.Print("CharacterBody2D Entered: " + characterBody2D.Name);
			// Store the result in the class variable
			areaEnteredResult = 0; 
		}
    }

	public int GetAreaEnteredResult()
	{
		return areaEnteredResult;
	}

}

