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
	private float Speed = 200.0f;
	private enemy enemyScene;
	private aojm aojmscene;
	private bullet bulletspawn;
	private boom boomspawn;
	private bool isBoomOnCooldown = false;

	private Timer timer, slimetimer, aojmtimer, bulletcd;
 

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		timer = new Timer();
		AddChild(timer);
		timer.WaitTime = 1f;
		timer.Start();

		slimetimer = new Timer();
		AddChild(slimetimer);
		slimetimer.WaitTime = 0.5f;
		slimetimer.Timeout += Spawn;
		slimetimer.Start();

		bulletcd = new Timer();
		AddChild(bulletcd);
		bulletcd.WaitTime = 1f;
		bulletcd.Timeout += buulet;
		bulletcd.Start();

		aojmtimer = new Timer();
		AddChild(aojmtimer);
		aojmtimer.WaitTime = 30;
		aojmtimer.Timeout += aojmspawn;
		aojmtimer.Start();

		hpbar = this.GetNode<ProgressBar>("hpbar");
		sprite2d = this.GetNode<Sprite2D>("Sprite2D");
		charAnimations = this.GetNode<AnimationPlayer>("moveanim");
	}

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
	{
		bulletcd.WaitTime = bullet.cdbullet;
		UpdateAnimation();
		bool isDead = death;

		if (!isDead)
		{
			Vector2 velocity = Velocity;
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

			bool isLeft = velocity.X < 0;
			sprite2d.FlipH = isLeft;

			if (Input.IsActionPressed("attack"))
			{
				if (!isBoomOnCooldown)
				{
					StartBoomCooldown();

					for (int i = 0; i < 3; i++)
					{
						boomspawn = (boom)GD.Load<PackedScene>("res://scenes/boom.tscn").Instantiate();
						Vector2 playerDirection = GetPlayerFacingDirection();
						boomspawn.SetDirection(playerDirection);
						AddChild(boomspawn);
					}
				}
			}

			if (pts == cdreduceinterval)
			{
				ReduceBulletCD();
				ReduceBoomCD();
				pts = 0;
				cdreduceinterval += 1;
			}
		}
		else
		{
			charAnimations.Play("death");
			hpbar.Hide();
		}
	}

	private async void StartBoomCooldown()
	{
		isBoomOnCooldown = true;
		await ToSignal(timer, "timeout");
		isBoomOnCooldown = false;
	}

	private void ReduceBoomCD()
	{
		float reductionAmount = 1.0f;
		boom.cdboom = Mathf.Max(boom.cdboom - reductionAmount, 1.0f);
	}
	public void Spawn()
	{
		Speed += (float)0.05;
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
	}

	    private void aojmspawn()
    {
        Speed += (float)0.05;
		if (death == false)
		{
			aojmscene = (aojm)GD.Load<PackedScene>("res://scenes/aojm.tscn").Instantiate(); 
			AddSibling(aojmscene);

			// Calculate a random angle in radians
			float randomAngleforjm = (float)GD.RandRange(0, 2 * Mathf.Pi);
			// Calculate the new position relative to the player
			Vector2 offsetjm = new Vector2(1000, 1000).Rotated(randomAngleforjm);
			
			aojmscene.Position = sprite2d.Position + offsetjm;
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

	public static string GetDirectionName(Vector2 direction)
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

	private Vector2 GetPlayerFacingDirection()
	{
		// Assuming 'charAnimations' is the AnimationPlayer for the player
		string currentAnimation = charAnimations.CurrentAnimation;
		if (currentAnimation == "idle")
		{
			return Vector2.Down;
		}
		if (currentAnimation == "walk_up")
		{
			return Vector2.Up;
		}
		else if (currentAnimation == "walk_down")
		{
			return Vector2.Down;
		}
		else
		{
			// Assuming 'sprite2d' is the player's sprite
			return sprite2d.FlipH ? Vector2.Left : Vector2.Right;
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
