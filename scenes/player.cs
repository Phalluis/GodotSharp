using Godot;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

public partial class player : Area2D
{
	public static int Hit;
	private enemy enemyNode;
	private ProgressBar hpbar;
	private AnimationPlayer moveanim;
	private Sprite2D sprite2D;
	private Color originalColor;
	public static float hp = 300, maxhp = 300, ap = 5;
	private static Timer res;
	private Boolean restartbool = true;
	private bool hasRestarted = false, hpregen;
	private Node2D mainlevel;
	public override void _Ready()
	{
		res = GetNode<Timer>("restart");
		sprite2D = GetNode<Sprite2D>("../Sprite2D");
		originalColor = sprite2D.Modulate;
		enemyNode = GetParent().GetNode<enemy>("../enemy");
		hpbar = GetNode<ProgressBar>("../hpbar");
		sprite2D = GetNode<Sprite2D>("../Sprite2D");
		mainlevel = (Node2D)GetParent();
		mainlevel = (Node2D)mainlevel.GetParent();

		res.WaitTime = 5;
		res.OneShot = true;
		res.Timeout += Restart;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		GD.Print(hp);
		if (score.newap >= 10)
		{
			ap += 1;
			score.newap = 0; // Reset score.points to 0
		}
		if (ap >= 30)
		{
			ap = 30;
		}
		hpbar.MaxValue = maxhp;
		hpbar.Value = hp;
		Hpcalculation();
	}

	private void Hpcalculation()
	{
		if (Hit == 1)
		{
			hp -= 1;
			sprite2D.Modulate = new Color(1.0f, 0.0f, 0.0f);
			hpregen = false;
		}
		if (Hit == 0 && hp < maxhp && hp >= 0)
		{
			hp += 1;
			hpregen = true;
			sprite2D.Modulate = new Color(0.0f, 1.0f, 0.0f);
		}
		else if (hp <= 0 && hpregen is false)
		{
			Move.Dead();
			enemy.PlayerisDead();
		}
		if (hp <= 0 && restartbool is true && !hasRestarted)
		{
			res.Start();
			restartbool = false;
			hasRestarted = true;
		}
		if (hp >= maxhp)
		{
			sprite2D.Modulate = originalColor;
		}
	}

	private void Restart()
	{
		// Change the scene to the game over scene
		GetTree().ChangeSceneToFile("res://scenes/gameover.tscn");
		GD.Print("Has Restarted");
	}

	internal static void Revived()
	{
		hp = maxhp;
		GD.Print(hp);
		Move.Alive();
		enemy.PlayerisAlive();
	}

}
