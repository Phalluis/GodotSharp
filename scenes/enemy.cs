using Godot;
using System;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;

public partial class enemy : CharacterBody2D
{
	private int enemyInstanceId;
	private AnimationPlayer enemyanimations;
	private CharacterBody2D playercharacter;
	private float speed = 25.0f; // Adjust the speed as needed
	private float distanceThreshold = 200.0f; // Adjust the distance threshold
	private static Boolean isPlayerDead = false; // State variable to track player's life status
	private int enemyId;

	public override void _Ready()
	{
		enemyanimations = this.GetNode<AnimationPlayer>("enemyanim");
		Area2D hitbox = this.GetNode<Area2D>("hitbox");
		hitbox.Monitoring = true;
		hitbox.AreaEntered += OnAreaEntered;
		hitbox.AreaExited += OnAreaExited;

		enemyId = (int)GD.Randi();
	}

	public override void _PhysicsProcess(double delta)
	{
		playercharacter = GetParent().GetNode<CharacterBody2D>("player");
		bool playerdead = isPlayerDead;
		UpdateAnimation();
		if (!playerdead)
		{
			Sprite2D sprite2d = this.GetNode<Sprite2D>("Sprite2D");

			// Calculate the direction from the enemy to the player
			Vector2 direction = (playercharacter.Position - Position).Normalized();
			KinematicCollision2D collision = MoveAndCollide(direction * speed * (float)delta);
			// Move the enemy towards the player
			Position += direction * speed * (float)delta;

			// Update the facing direction based on the angle
			UpdateFacingDirection(direction);
		}
	}

	private void UpdateAnimation()
	{
		// Calculate the distance between the enemy and the player
		float distanceToPlayer = Position.DistanceTo(playercharacter.Position);
		if (isPlayerDead)
		{
			enemyanimations.Play("idle");
		}
		else
		{
			if (distanceToPlayer > distanceThreshold)
			{
				enemyanimations.Play("moving");
				speed = 40f;
			}
			else if (distanceToPlayer < 50f)
			{
				enemyanimations.Play("move");
				speed = 20f;
			}
			else
			{
				enemyanimations.Play("move");
				speed = 25f;
			}
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

	void OnAreaEntered(Area2D otherArea)
	{
		if (otherArea.IsInGroup("bullet"))
		{
			// Trigger death logic for this specific enemy
			enemyanimations.Play("death");
			// Remove the enemy from the scene
			otherArea.QueueFree();
			QueueFree();
		}
		else if (otherArea.GetParent() is CharacterBody2D characterBody2D && characterBody2D.IsInGroup("character"))
		{
			player.Hit = 1;
		}
	}

	private void OnAreaExited(Area2D area)
	{
		if (area.GetParent() is CharacterBody2D characterBody2D && characterBody2D.IsInGroup("character"))
		{
			player.Hit = 0;
		}
	}

	internal static void PlayerisDead()
	{
		isPlayerDead = true;
	}

	internal static void PlayerisAlive()
	{
		isPlayerDead = false;
	}
	internal int GetEnemyId()
	{
		return enemyId;
	}
}
