using Godot;
using System;

public partial class score : Label
{
	public static int points;
	private Label pointlabel;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		pointlabel = this.GetNode<Label>("../Label");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		pointlabel.Text = "Score: "+points;
	}
}
