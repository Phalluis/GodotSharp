using Godot;
using System;

public partial class move : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		AnimationPlayer charanimations = this.GetNode<AnimationPlayer>("charanim");
		Sprite2D child = this.GetNode<Sprite2D>("Child");
		float AMOUNT = 5;
		if (Input.IsKeyPressed(Key.W)){
			this.Position += new Vector2(0,-AMOUNT);
			child.Position = new Vector2(0,AMOUNT+15);
			charanimations.Play("walk_up");
		}
		if (Input.IsKeyPressed(Key.S)){
			this.Position += new Vector2(0,AMOUNT);
			child.Position = new Vector2(0,-AMOUNT-15);
			charanimations.Play("walk_down");
		}
		if (Input.IsKeyPressed(Key.A)){
			this.Position += new Vector2(-AMOUNT,0);
			child.Position = new Vector2(15+AMOUNT,0);
			charanimations.Play("walk_right");
		}
		if (Input.IsKeyPressed(Key.D)){
			this.Position += new Vector2(AMOUNT,0);
			child.Position = new Vector2(-15-AMOUNT,0);
			charanimations.Play("walk_right");
		}
		if (Input.IsKeyPressed(Key.Up)){
			charanimations.Play("atk_up");
			}
		if (Input.IsKeyPressed(Key.Right)){
			charanimations.Play("atk_right");
			}
		if (Input.IsKeyPressed(Key.Left)){
			charanimations.Play("atk_right");
			}
		if (Input.IsKeyPressed(Key.Down)){
			charanimations.Play("atk_down");
			}
	}
}
