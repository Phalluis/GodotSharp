using Godot;
using System;
using System.Threading.Tasks;

public partial class player : Area2D
{
	private enemy enemyNode;
	private float hp = 100, maxhp = 100, ap = 10;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		enemyNode = GetParent().GetNode<enemy>("../enemy");
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		int result = enemyNode.GetAreaEnteredResult();
		GD.Print("Area Entered Result: ", result);
		hpcalculation(result);
		if (hp)
	}

    private async void hpcalculation(int result)
{
    if (result == 1)
    {
        hp -= 10;
        // Wait for one second before continuing
        await Task.Delay(1000);
        // Further processing can be done here
    }
}

}
