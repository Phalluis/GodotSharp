using Godot;
using System;

public partial class boolet : Sprite2D
{
	CharacterBody2D player;
	Vector2 targetPosition;

    public object BulletScene { get; private set; }

    public override void _Ready()
	{
		player = GetParent<CharacterBody2D>();
		Node parent = GetParent();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position = GetLocalMousePosition();
	}
}