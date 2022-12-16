using Godot;
using System;

public class Level4_4 : Node2D
{
	Sprite background;
	TileMap cliff;
	Position2D startPos;
	Area2D trigger;
	PackedScene worker;
	AnimationPlayer animPlayer;
	public override void _Ready()
	{
		background = GetNode<Sprite>("Background");
		cliff = GetNode<TileMap>("CliffTilemap");
		trigger = GetNode<Area2D>("Trigger");
		startPos = GetNode<Position2D>("StartPosition");
		animPlayer = GetNode<AnimationPlayer>("Director");
		worker = (PackedScene)ResourceLoader.Load("res://NPC/Worker.tscn");
		Sprite newWorker = (Sprite)worker.Instance();
		AddChild(newWorker);
		newWorker.Position = startPos.Position;
		animPlayer.Play("CREDITS");
	}

	private void _on_Trigger_body_entered(Node body)
	{
		background.Position += new Vector2(200.0f, 0.0f);
		cliff.Position += new Vector2(200.0f, 0.0f);
		trigger.Position += new Vector2(200.0f, 0.0f);
		startPos.Position += new Vector2(200.0f, 0.0f);
	}

	private void _on_Timer_timeout()
	{
		Sprite newWorker = (Sprite)worker.Instance();
		int posY = (int)GD.RandRange(-65.0, 301.0);
		AddChild(newWorker);
		newWorker.Position = new Vector2(startPos.Position.x, posY);
		GD.Print("timer");
	}

	private void _on_Director_animation_finished(String animName)
	{
		GetTree().ChangeScene("res://EndScene.tscn");
	}
}
