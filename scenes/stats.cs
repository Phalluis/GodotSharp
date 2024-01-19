using Godot;
using System;

public partial class stats : Node2D
{
	public static double enemyhpincrement = 10, enemymaxhpincrement = 1;
	public static float enemyspeed, enemyspeedincrement = 1;
	private AnimationPlayer onstart;
	public override void _Ready()
	{	
		onstart = this.GetNode<AnimationPlayer>("onstart");
		onstart.Play("onstart");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
