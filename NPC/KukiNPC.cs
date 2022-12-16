using Godot;
using System;

public class KukiNPC : KinematicBody2D
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
    [Export] public bool condition;

    public String[] animations = {"ACT1"};
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        state = EVENT.NOT_IN_ANIMATION;
        TextBox = GetNode<MarginContainer>("TextBox");
        textBox = TextBox.GetNode<Label>("Label");
        collisionSpace = GetNode<Area2D>("CollisionSpace");
        animPlayer = GetParent().GetNode<AnimationPlayer>("Director");
        audioPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
        collisionSpace.Monitoring = true;
        TextBox.Visible = false;
        condition = true;
        actNumber = 0;
        animName = animations[actNumber];
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
    }
    private void _on_CollisionSpace_body_entered(PhysicsBody2D body)
    {
        if(body.Name != "Player")
            return;
        if(condition == true || actNumber == 0)
        {
            ((Player)body).velocity = new Vector2(0.0f, -1.0f);
            animPlayer.Play(animations[actNumber]);
            if(condition == true && actNumber != (animations.Length-1))
                actNumber++;
            state = EVENT.ANIMATION;
        }
        GD.Print("collision");
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
        if(animations[animations.Length - 1] == animName && condition == true)
        {
            collisionSpace.Monitoring = false;
        }
        state = EVENT.NOT_IN_ANIMATION;
    }
}
