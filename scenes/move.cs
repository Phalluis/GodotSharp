using Godot;
using System;
using System.Threading.Tasks;

public partial class Move : CharacterBody2D
{
	public const float Speed = 400.0f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.

	public override void _Process(double delta)
	{
		UpdateAnimation();
		Vector2 velocity = Velocity;
		Sprite2D sprite2d = this.GetNode<Sprite2D>("Sprite2D");
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

	private void UpdateAnimation()
	{
		AnimationPlayer charAnimations = this.GetNode<AnimationPlayer>("moveanim");

		if (Velocity.Length() > 0)
		{
			charAnimations.Play("walk_" + GetDirectionName(Velocity));
		}
		else
		{
			charAnimations.Play("idle");
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
}