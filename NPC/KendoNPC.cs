using Godot;
using System;

public class KendoNPC : KinematicBody2D
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
    AnimationPlayer animPlayer2;
    AudioStreamPlayer audioPlayer;
    int actNumber;
    int shadowActNumber;
    public bool condition;

    public String[] animations = {"ACT1"};
    public String[] animations2 = {"ACT1"};
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        state = EVENT.NOT_IN_ANIMATION;
        TextBox = GetNode<MarginContainer>("TextBox");
        textBox = TextBox.GetNode<Label>("Label");
        collisionSpace = GetNode<Area2D>("CollisionSpace");
        animPlayer = GetParent().GetNode<AnimationPlayer>("Director");
        animPlayer2 = GetParent().GetNode<AnimationPlayer>("ShadowDirector");
        audioPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
        collisionSpace.Monitoring = true;
        TextBox.Visible = false;
        actNumber = 0;
        animName = animations[actNumber];
        condition = false;
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
        if(body.Name != "Player" || animPlayer2.IsPlaying() == true)
        {
            GD.Print(body.Name);
            return;
        }
        if(condition == true || actNumber == 0)
        {
            // animPlayer.Play(animations[actNumber]);
            // if(condition == true)
            //     actNumber++;
            ((Player)body).velocity = new Vector2(1.0f, 0.5f);
            if(condition == true && actNumber != (animations.Length-1))
                actNumber++;
            state = EVENT.ANIMATION;
            animPlayer.Play(animations[actNumber]);
        }
    }

    private void _on_CollisionSpace_area_entered(Area2D area)
    {
        if(area.Name != "Player2" || animPlayer.IsPlaying() == true)
        {
            return;
        }
        if(condition == true || actNumber == 0)
        {
            animPlayer2.Play(animations[shadowActNumber]);
            if(condition == true)
                shadowActNumber++;
        }
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

    private void _on_ShadowDirector_animation_finished(String animName)
    {
        if(animations2[animations2.Length - 1] == animName && condition == true)
        {
            TextBox.Visible = false;
        }

    }
}
