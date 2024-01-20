using Godot;
using System;

public partial class gameover : Control
{
	private AudioStreamPlayer2D onstartmusic;
	private Button tryagain;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		tryagain = GetNode<Button>("TryAgainButton");
		tryagain.Pressed += again;

		onstartmusic = GetNode<AudioStreamPlayer2D>("onstart");
		onstartmusic.Finished += loop;
		onstartmusic.VolumeDb = 10;
		onstartmusic.Play();
	}

    private void loop()
    {
        onstartmusic.Play();
    }

    private void again()
	{
		GD.Print("Tried Again");
		player.Revived();
		GetTree().ChangeSceneToFile("res://scenes/node_2d.tscn");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
