using Godot;
using System;

public partial class attackHitbox : Area2D
{
    private CharacterBody2D player;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        player = GetParent<CharacterBody2D>();
        Hide(); // Make the hitbox initially invisible
    }

    public override void _Process(double delta)
    {
        if (move.IsAttacking())
        {            
            Show();
        }
        else
        {
            // If not attacking, hide the hitbox
            Hide();
        }
    }

    private void _on_body_entered(Node body)
    {
        // Handle the logic when a body enters the area
    }
}
