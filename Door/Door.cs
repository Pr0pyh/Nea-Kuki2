using Godot;
using System;

public class Door : Area2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    [Export] public String level;
    [Export] public int objectiveScore;
    AnimationPlayer introExit;
    public Sprite light;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        light = GetNode<Sprite>("Light");
        light.Visible = false;
        introExit = GetParent().GetNode<AnimationPlayer>("IntroExit");
    }

    private void _on_Door_body_entered(PhysicsBody2D body)
    {
        if(body.Name != "Player")
            return;
        if(((Player)body).score >= objectiveScore)
        {
            introExit.Play("Exit");
        }
    }

    private void _on_IntroExit_animation_finished(String animName)
    {
        if(animName == "Exit")
            GetTree().ChangeScene(level);
    }
}
