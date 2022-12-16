using Godot;
using System;

public class Level4_0 : Node2D
{
    // Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	Player player;
	Door door;
	int needed_score = 0;
	AnimationPlayer introExit;
	String[] animations = {"ACT1"};
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetNode<YSort>("YSort").GetNode<Player>("Player");
		door = GetNode<Door>("Door");
		introExit = GetNode<AnimationPlayer>("IntroExit");
		introExit.Play("Entry");
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if(player.score >= needed_score)
		{
			door.light.Visible = true;
		}
	}
}
