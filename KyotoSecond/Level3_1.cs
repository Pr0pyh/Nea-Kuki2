using Godot;
using System;

public class Level3_1 : Node2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	Player player;
	player2 _player;
	KendoNPC kendoMaster;
	Door door;
	AnimationPlayer introExit;
	int needed_score = 0;
	String[] animations = {"ACT1"};
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetNode<YSort>("YSort").GetNode<Player>("Player");
		kendoMaster = GetNode<KendoNPC>("KendoNPC");
		_player = GetNode<player2>("Player2");
		door = GetNode<Door>("Door");
		introExit = GetNode<AnimationPlayer>("IntroExit");
		_player.objective = needed_score;
		kendoMaster.animations = animations;
		introExit.Play("Entry");
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if(player.score >= needed_score)
		{
			kendoMaster.condition = true;
			door.light.Visible = true;
		}
	}
}
