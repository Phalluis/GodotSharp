using Godot;
using System;

public partial class stats : Node2D
{
	public static double enemyhpincrement = 10, enemymaxhpincrement = 1;
	public static float enemyspeed, enemyspeedincrement = 1;
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
