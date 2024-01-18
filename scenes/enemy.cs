using Godot;
using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

public partial class enemy : CharacterBody2D
{
	private double enemybasehp = 50, enemybasemaxhp = 50;
	private ProgressBar hpbar;
	private AnimationPlayer enemyanimations;
	private CharacterBody2D playercharacter;
	private Sprite2D enemycharacter;
	private float enemybasespeed = 25.0f; // Adjust the speed as needed
	private float distanceThreshold = 200.0f; // Adjust the distance threshold
	private static Boolean isPlayerDead = false; // State variable to track player's life status
	private Color originalColor;
	private Timer statincrease,expiration, gametime;
	public override void _Ready()
	{
		enemyanimations = this.GetNode<AnimationPlayer>("enemyanim");
		enemyanimations.AnimationFinished += enemydead;

		enemycharacter = this.GetNode<Sprite2D>("Sprite2D");
		originalColor = enemycharacter.Modulate;

		statincrease = new Timer();
        AddChild(statincrease);
        statincrease.WaitTime = 5;
        statincrease.Timeout += increaseenemystat;
        statincrease.Start();

		gametime = GetNode<Timer>("../GameTime");
        gametime.Start();

		expiration = new Timer();
        AddChild(expiration);
        expiration.WaitTime = 60;
        expiration.Timeout += expireenemy;
        expiration.Start();

		Area2D hitbox = this.GetNode<Area2D>("hitbox");
		hitbox.Monitoring = true;
		hitbox.AreaEntered += OnAreaEntered;
		hitbox.AreaExited += OnAreaExited;

		hpbar = this.GetNode<ProgressBar>("hpbar");

		enemybasehp += stats.enemyhpincrement;
		enemybasemaxhp += stats.enemymaxhpincrement;
		enemybasespeed += stats.enemyspeedincrement;
	}

    public override void _PhysicsProcess(double delta)
	{
		hpbar.MaxValue = enemybasemaxhp;
		hpbar.Value = enemybasehp;
		playercharacter = GetParent().GetNode<CharacterBody2D>("player");
		bool playerdead = isPlayerDead;
		UpdateAnimation();

		// Check if hp is greater than 0
		if (enemybasehp > 0)
		{
			if (!playerdead)
			{
				Sprite2D sprite2d = this.GetNode<Sprite2D>("Sprite2D");

				// Calculate the direction from the enemy to the player
				Vector2 direction = (playercharacter.Position - Position).Normalized();
				KinematicCollision2D collision = MoveAndCollide(direction * stats.enemyspeed * (float)delta);

				// Move the enemy towards the player
				Position += direction * stats.enemyspeed * (float)delta;

				// Update the facing direction based on the angle
				UpdateFacingDirection(direction);
			}
		}
	}

	private void UpdateAnimation()
	{
		if (enemybasehp <= 0)
		{
			enemycharacter.Modulate = new Color(1.0f, 0.0f, 0.0f);
			enemyanimations.Play("death");

		}
		else
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
					stats.enemyspeed = enemybasespeed + 25.0f;
				}
				else if (distanceToPlayer < 50f)
				{
					enemyanimations.Play("move");
					stats.enemyspeed = enemybasespeed + 15f;
				}
				else
				{
					enemyanimations.Play("move");
					stats.enemyspeed = enemybasespeed + 5f;
				}
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
		if (enemybasehp <= 0)
		{
			// Enemy is already dead, no need to process the hit
			return;
		}

		if (otherArea.IsInGroup("bullet"))
		{
			// Play hurt animation
			enemyanimations.Play("hurt");

			// Find the player's position
			Vector2 playerPosition = playercharacter.Position;

			// Calculate the direction from the enemy to the player
			Vector2 knockbackDirection = (Position - playerPosition).Normalized();

			// Apply knockback
			Knockback(knockbackDirection);

            // Removes bullet
			enemybasehp -= (int)player.ap;
		}
		else if (otherArea.GetParent() is CharacterBody2D characterBody2D && characterBody2D.IsInGroup("character"))
		{
			player.Hit = 1;
		}
	}

    void Knockback(Vector2 direction)
	{
		// Adjust the knockback distance based on your needs
		float knockbackDistance = 20.0f;

		// Apply knockback to the enemy
		Position += direction * knockbackDistance;
	}

	private void enemydead(StringName animName)
	{
		if (animName == "death")
		{
			score.points += 1;
			Move.pts +=1;
			score.newap += 1;
			// Animation finished, queue-free the enemy
			QueueFree();
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

    private void increaseenemystat()
    {
        enemybasehp += 1;
		enemybasemaxhp += 0.1;
    }

	private void expireenemy()
    {
        QueueFree();
    }
}
