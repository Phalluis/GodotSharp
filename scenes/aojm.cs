using Godot;
using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

public partial class aojm : CharacterBody2D
{
	private double enemybasehp = 300, enemybasemaxhp = 300, tpthreshold;
	private ProgressBar hpbar;
	private AnimationPlayer enemyanimations;
	private CharacterBody2D playercharacter;
	private Sprite2D enemycharacter;
	private float enemybasespeed = 50.0f; // Adjust the speed as needed
	private float distanceThreshold = 100.0f; // Adjust the distance threshold
	private static Boolean isPlayerDead = false; // State variable to track player's life status
	private Color originalColor;
	private Timer statincrease, expiration, gametime;
	private AudioStreamPlayer2D aojmchase;
	public static bool aojmchasebool;
	public override void _Ready()
	{
		enemyanimations = this.GetNode<AnimationPlayer>("enemyanim");
		enemyanimations.AnimationFinished += enemydead;

		enemycharacter = this.GetNode<Sprite2D>("Sprite2D");
		originalColor = enemycharacter.Modulate;

		statincrease = new Timer();
		AddChild(statincrease);
		statincrease.WaitTime = 1;
		statincrease.Timeout += increaseenemystat;
		statincrease.Start();

		gametime = GetNode<Timer>("../GameTime");
		gametime.Start();

		Area2D hitbox = this.GetNode<Area2D>("hitbox");
		hitbox.Monitoring = true;
		hitbox.AreaEntered += OnAreaEntered;
		hitbox.AreaExited += OnAreaExited;

		hpbar = this.GetNode<ProgressBar>("hpbar");

		enemybasehp += stats.enemyhpincrement;
		enemybasemaxhp += stats.enemymaxhpincrement;
		enemybasespeed += stats.enemyspeedincrement;

		aojmchase = this.GetNode<AudioStreamPlayer2D>("aojmchase");

		AudioStream audioStream = GD.Load<AudioStream>("res://assets/music/AoJMChase.mp3") as AudioStream;
		aojmchase.VolumeDb = 20;
		aojmchase.Play();
		aojmchasebool = true;
		aojmchase.Finished += playagain;

		tpthreshold = enemybasemaxhp;
	}

	private void playagain()
	{
		aojmchase.Play();
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
				Vector2 velocity = Velocity;
				Sprite2D sprite2d = this.GetNode<Sprite2D>("Sprite2D");

				// Calculate the direction from the enemy to the player
				Vector2 direction = (playercharacter.Position - Position).Normalized();
				KinematicCollision2D collision = MoveAndCollide(direction * enemybasespeed * (float)delta);

				// Move the enemy towards the player
				Position += direction * stats.enemyspeed * (float)delta;
				// Update the facing direction based on the angle
				Velocity = velocity;
				bool isLeft = velocity.X < 0;
				sprite2d.FlipH = isLeft;
			}
		}
	}

	private void UpdateAnimation()
	{
		if (enemybasehp <= 0)
		{
			enemycharacter.Modulate = new Color(1.0f, 0.0f, 0.0f);
			enemyanimations.Play("idle");
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
				if (enemybasehp <= tpthreshold)
				{
					playercharacter = GetNode<CharacterBody2D>("../player");
					float randomAngle = Mathf.DegToRad(GD.RandRange(0, 360));

					// Calculate the new position relative to the player
					Vector2 offset = new Vector2(1000, 1000).Rotated(randomAngle);

					this.Position = playercharacter.Position + offset;

					enemybasespeed += 10.0f;
					tpthreshold -= 100;
				}
				Vector2 direction = (playercharacter.Position - Position).Normalized();
				float angle = Mathf.RadToDeg(direction.Angle());
				// Update the facing direction based on the angle
				if (angle > -45f && angle <= 45)
				{
					enemyanimations.Play("walk_right");
				}
				else if (angle > 45 && angle <= 135f)
				{
					enemyanimations.Play("walk_down");
				}
				else
				{
					enemyanimations.Play("walk_up");
				}

				UpdateFacingDirection(Velocity);
			}
		}
	}

	private void UpdateFacingDirection(Vector2 direction)
	{
		Sprite2D sprite2d = this.GetNode<Sprite2D>("Sprite2D");

		float angle = Mathf.RadToDeg(direction.Angle());

		sprite2d.FlipH = angle > -45 && angle <= 45;
	}

	private int areaEnteredResult = 0;

	void OnAreaEntered(Area2D otherArea)
	{
		if (enemybasehp <= 0)
		{
			return;
		}
		if (otherArea.IsInGroup("hazardtoenemy"))
		{
			if (otherArea.IsInGroup("bullet"))
			{
				// Play hurt animation
				enemyanimations.Play("idle");

				// Find the player's position
				Vector2 playerPosition = playercharacter.Position;

				// Calculate the direction from the enemy to the player
				Vector2 knockbackDirection = (Position - playerPosition).Normalized();

				// Apply knockback
				Knockback(knockbackDirection);

				enemybasehp -= player.ap * bullet.bulletdamage;
			}

			if (otherArea.IsInGroup("boom"))
			{
				// Play hurt animation
				enemyanimations.Play("idle");

				// Find the player's position
				Vector2 playerPosition = playercharacter.Position;

				// Calculate the direction from the enemy to the player
				Vector2 knockbackDirection = (Position - playerPosition).Normalized();

				// Apply knockback
				Knockback(knockbackDirection);

				enemybasehp -= player.ap * boom.boomdamage;
			}
		}
		else if (otherArea.GetParent() is CharacterBody2D characterBody2D && characterBody2D.IsInGroup("character"))
		{
			player.Hit = 1;
			player.hp -= 50;
		}
	}

	void Knockback(Vector2 direction)
	{
		// Adjust the knockback distance based on your needs
		float knockbackDistance = 5.0f;

		// Apply knockback to the enemy
		Position += direction * knockbackDistance;
	}

	private void enemydead(StringName animName)
	{
		if (animName == "idle")
		{
			score.points += 100;
			Move.pts += 100;
			score.newap += 10;
			// Animation finished, queue-free the enemy
			aojmchase.Stop();
			QueueFree();
		}
	}

	private void OnAreaExited(Area2D area)
	{
		if (area.GetParent() is CharacterBody2D characterBody2D && characterBody2D.IsInGroup("character") || area.IsInGroup("bullet") || area.IsInGroup("boom"))
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
		enemybasemaxhp += 1;
	}
}
