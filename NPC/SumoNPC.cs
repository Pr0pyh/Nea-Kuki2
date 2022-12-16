using Godot;
using System;

public class SumoNPC : KinematicBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    enum EVENT {
        ANIMATION,
        NOT_IN_ANIMATION,
        STOP
    };

    EVENT state;
    MarginContainer TextBox;
    Label textBox;
    Area2D collisionSpace;
    String animName;
    AnimationPlayer animPlayer;
    AudioStreamPlayer audioPlayer;
    int actNumber;
    public bool condition;

    String[] animations = {"ACT1", "ACT2"};
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        state = EVENT.NOT_IN_ANIMATION;
        TextBox = GetNode<MarginContainer>("TextBox");
        textBox = TextBox.GetNode<Label>("Label");
        collisionSpace = GetNode<Area2D>("CollisionSpace");
        audioPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
        collisionSpace.Monitoring = true;
        TextBox.Visible = false;
        actNumber = 0;
        animName = animations[actNumber];
        condition = false;
        animPlayer = GetParent().GetNode<AnimationPlayer>("Director");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        switch(state)
        {
            case EVENT.ANIMATION:
                if(animPlayer.IsPlaying() == false)
                    animPlayer.Play();
                break;
            case EVENT.NOT_IN_ANIMATION:
                break;
            case EVENT.STOP:
                if(Input.IsActionPressed("ui_accept"))
                {
                    state = EVENT.ANIMATION;
                    TextBox.Visible = false;
                }
                break;
        }

        if(condition == true)
            Position = new Vector2(-86.0f, 298.0f);
    }
    private void _on_CollisionSpace_body_entered(PhysicsBody2D body)
    {
        if(body.Name != "Player")
            return;
        ((Player)body).velocity = new Vector2(-1.0f, 0.5f);
        if(condition == true && actNumber != (animations.Length-1))
            actNumber++;
        state = EVENT.ANIMATION;
        animPlayer.Play(animations[actNumber]);
    }

    public void insertText(String text)
    {
        animPlayer.Stop(false);
        state = EVENT.STOP;
        TextBox.Visible = true;
        textBox.Text = text;
        audioPlayer.Play();
    }

    private void _on_Director_animation_finished(String animName)
    {
        // if(animations[animations.Length - 1] == animName)
        // {
        //     collisionSpace.Monitoring = false;
        // }
        // else
        // {
        //     actNumber++;
        // }
        if(animations[animations.Length - 1] == animName && condition == true)
        {
            collisionSpace.Monitoring = false;
        }
        state = EVENT.NOT_IN_ANIMATION;
    }
}
