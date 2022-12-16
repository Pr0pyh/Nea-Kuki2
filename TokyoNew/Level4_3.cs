using Godot;
using System;

public class Level4_3 : Node2D
{
   // Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";
	Player player;
	player2 _player;
	Door door;
	AnimationPlayer introExit;
	AudioStreamPlayer audioPlayer;
	int needed_score = 0;
	String[] animations = {"ACT1"};
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		player = GetNode<YSort>("YSort").GetNode<Player>("Player");
		_player = GetNode<player2>("Player2");
		door = GetNode<Door>("Door");
		introExit = GetNode<AnimationPlayer>("IntroExit");
		audioPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
		_player.objective = needed_score;
		_player.Visible = true;
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

	private void _on_Trigger_body_entered(Node body)
	{
		if(body.Name == "Player")
		{
			_player.Visible = false;
			String path = "res://Music and Sounds/X2Download.app - 16-Bit Fantasy RPG OST - Nocturne (Night_Camping Music) (128 kbps).mp3";
			File file = new File();
			if(file.FileExists(path))
			{
				file.Open(path, File.ModeFlags.Read);
				byte[] buffer = file.GetBuffer((long)file.GetLen());
				AudioStreamMP3 streamMusic = new AudioStreamMP3();
				streamMusic.Data = buffer;
				audioPlayer.Stream = streamMusic;
				audioPlayer.Play();
			}
		}
	}
}
