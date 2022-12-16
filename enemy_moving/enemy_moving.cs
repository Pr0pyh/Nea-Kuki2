using Godot;
using System;

public class enemy_moving : KinematicBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    enum EVENT {
        MOVING,
        NOT_MOVING
    };

    Area2D player2;
    EVENT state;
    [Export] public bool vertical = false;
    Vector2 moveDir;
    float speed = 0.0f;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        player2 = this.GetParent().GetNode<Area2D>("Player2");
        state = EVENT.NOT_MOVING;
        if(vertical)
            moveDir = Vector2.Up;
        else
            moveDir = Vector2.Left;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        switch(state) 
        {
            case EVENT.MOVING:
                if(vertical)
                    moveDir.y = (Position - player2.Position).Normalized().y;
                else
                    moveDir.x = (Position - player2.Position).Normalized().x;
                speed = 50.0f;
                break;
            case EVENT.NOT_MOVING:
                if(this.IsOnWall())
                    moveDir *= -1;
                speed = 50.0f;
                break;
        }

        if(Position.DistanceTo(player2.Position) < 50)
        {
            state = EVENT.MOVING;
        }
        else
            state = EVENT.NOT_MOVING;

        MoveAndSlide(moveDir*speed);
    }
}
