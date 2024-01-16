using Godot;
using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

public partial class player : Area2D
{
	private enemy enemyNode;
	private ProgressBar hpbar;
	private AnimationPlayer moveanim;
	private Sprite2D sprite2D;
	private Color originalColor;
	private float hp = 1000, maxhp = 1000, ap = 10;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		enemyNode = GetParent().GetNode<enemy>("../enemy");
		hpbar = GetNode<ProgressBar>("../hpbar");
		sprite2D = GetNode<Sprite2D>("../Sprite2D");
		originalColor = sprite2D.Modulate;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		int result = enemyNode.GetAreaEnteredResult();

		hpbar.MaxValue = maxhp;
		hpbar.Value = hp;
		Hpcalculation(result);
		GD.Print(hp);
		if (hp == 0)
		{
		}
	}

	private void Hpcalculation(int result)
	{
		if (result == 1)
		{
			hp -= 10;
			sprite2D.Modulate = new Color(1.0f, 0.0f, 0.0f);
		}
		if (result == 0 && hp <= maxhp)
		{
			hp += 1;
			sprite2D.Modulate = originalColor;
		}
		if (hp <= 0)
		{
			Move.Dead();
			enemy.PlayerisDead();
		}
	}

}
