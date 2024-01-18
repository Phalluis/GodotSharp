using Godot;
using System;

public partial class Move : CharacterBody2D
{
	public static double pts;
	int cdreduceinterval = 1;

	private Sprite2D sprite2d;
	private ProgressBar hpbar;
	private static Boolean death = false;
	private AnimationPlayer charAnimations;
	public const float Speed = 400.0f;
	private enemy enemyScene;
	private bullet bulletspawn;
	private Timer timer, bulletcd;




	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		timer = this.GetNode<Timer>("spawntimer");
		timer.WaitTime = 1;
		timer.Timeout += Spawn;
		timer.Start();

		bulletcd = GetNode<Timer>("bulletcd");
		bulletcd.Timeout += buulet;
		bulletcd.Start();

		hpbar = this.GetNode<ProgressBar>("hpbar");
		sprite2d = this.GetNode<Sprite2D>("Sprite2D");
		charAnimations = this.GetNode<AnimationPlayer>("moveanim");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		bulletcd.WaitTime = bullet.cdbullet;
		UpdateAnimation();
		bool isDead = death; // Store the death status to avoid potential race conditions
		if (!isDead)
		{
			Vector2 velocity = Velocity;
			// Get the input direction and handle the movement/deceleration.
			float direction = Input.GetAxis("left", "right");
			if (direction != 0)
			{
				velocity.X = direction * Speed;
			}
			else
			{
				velocity.X = Mathf.MoveToward(Velocity.X, 0, 20);
			}

			float direction1 = Input.GetAxis("up", "down");
			if (direction1 != 0)
			{
				velocity.Y = direction1 * Speed;
			}
			else
			{
				velocity.Y = Mathf.MoveToward(velocity.Y, 0, 20);
			}

			Velocity = velocity;
			MoveAndSlide();

			// Flip the sprite based on the direction.
			bool isLeft = velocity.X < 0;
			sprite2d.FlipH = isLeft;

			if (pts == cdreduceinterval)
			{
				GD.Print(cdreduceinterval);
				GD.Print(pts);
				ReduceBulletCD();
				pts = 0; // Reset the counter after reducing the cooldown
				cdreduceinterval += 1; // Increment the counter for every 10 points increase
			}
		}
		else
		{
			charAnimations.Play("death");
			hpbar.Hide();
		}
	}
	public void Spawn()
	{
		if (death == false)
		{
			enemyScene = (enemy)GD.Load<PackedScene>("res://scenes/enemy.tscn").Instantiate();
			AddSibling(enemyScene);

			// Calculate a random angle in radians
			float randomAngle = (float)GD.RandRange(0, 2 * Mathf.Pi);

			// Calculate the new position relative to the player
			Vector2 offset = new Vector2(1000, 1000).Rotated(randomAngle);
			enemyScene.Position = sprite2d.Position + offset;
		}
		else
		{
		}
	}
	private void UpdateAnimation()
	{
		if (death)
		{
			charAnimations.Play("death");
		}
		else
		{
			if (Velocity.Length() > 0)
			{
				charAnimations.Play("walk_" + GetDirectionName(Velocity));
			}
			else
			{
				charAnimations.Play("idle");
			}
		}
	}

	private string GetDirectionName(Vector2 direction)
	{
		float angle = Mathf.RadToDeg(direction.Angle());

		if (angle > -45 && angle <= 45)
		{
			return "right";
		}
		else if (angle > 45 && angle <= 135)
		{
			return "down";
		}
		else if (angle > -135 && angle <= -45)
		{
			return "up";
		}
		else
		{
			return "right";
		}
	}

	private void ReduceBulletCD()
	{
		float reductionAmount = 0.05f;

		// Ensure the cooldown doesn't go below a minimum value
		bullet.cdbullet = Mathf.Max(bullet.cdbullet - reductionAmount, 0.1f);

		// Declare a local variable to avoid modifying the class-level variable
		double adjustedInterval = Mathf.Max(cdreduceinterval - 1, 0.5);
	}

	private void buulet()
	{
		if (!death)
		{
			bulletspawn = (bullet)GD.Load<PackedScene>("res://scenes/bullet.tscn").Instantiate();
			AddChild(bulletspawn);
		}
	}


	public static void Dead()
	{
		death = true;
	}

	internal static void Alive()
	{
		death = false;
	}

}
