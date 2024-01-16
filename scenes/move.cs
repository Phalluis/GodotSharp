using Godot;
using System;

public partial class Move : CharacterBody2D
{	
	private Sprite2D sprite2d;
	private ProgressBar hpbar;
	private static Boolean death = false;
	private AnimationPlayer charAnimations;
	public const float Speed = 400.0f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Initialize charAnimations here if needed
		hpbar = this.GetNode<ProgressBar>("hpbar");
		sprite2d = this.GetNode<Sprite2D>("Sprite2D");
		charAnimations = this.GetNode<AnimationPlayer>("moveanim");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
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
		}
		else
		{
			charAnimations.Play("death");
			hpbar.Hide();
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

	public static void Dead()
	{
		death = true;
	}
}
