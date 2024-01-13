using Godot;
using System;

public partial class move : CharacterBody2D
{
	public const float Speed = 400.0f;
	private bool isAttacking = false;

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
		// As good practice, you should replace UI actions with custom gameplay actions.
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
		// Check for attack command
		if (Input.IsActionPressed("attack") && !isAttacking)
		{
			isAttacking = true;
			Attack();
		}
		// Flip the sprite based on the direction.
		bool isLeft = velocity.X < 0;
		sprite2d.FlipH = isLeft;
	}
	private void UpdateAnimation()
	{
		AnimationPlayer charanimations = this.GetNode<AnimationPlayer>("charanim");
		if (Velocity.Length() > 0)
		{
			charanimations.Play("walk_" + GetDirectionName(Velocity));
		}
		else
		{
			charanimations.Play("idle");
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
	private async void Attack()
	{
		AnimationPlayer charanimations = this.GetNode<AnimationPlayer>("charanim");
		charanimations.Play("atk_" + GetDirectionName(Velocity));
		await ToSignal(charanimations, "animation_finished");
		isAttacking = false;
		charanimations.Play("idle");
	}
}

