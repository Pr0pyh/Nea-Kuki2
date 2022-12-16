using Godot;
using System;

public class CameraPlayer : Camera2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    Position2D topLeft;
    Position2D bottomRight;

    Position2D startPos;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        topLeft = GetNode<Node>("Node").GetNode<Position2D>("TopLeft");
        bottomRight = GetNode<Node>("Node").GetNode<Position2D>("BottomRight");
        if(GetTree().CurrentScene.Name == "Level4_4")
            startPos = GetParent().GetNode<Position2D>("StartPosition");
        LimitTop = (int)topLeft.Position.y;
        LimitLeft = (int)topLeft.Position.x;
        LimitBottom = (int)bottomRight.Position.y;
        LimitRight = (int)bottomRight.Position.x;
    }

    private void _on_Trigger_body_entered(Node body)
    {
        bottomRight.Position += new Vector2(200.0f, 0.0f);
        LimitRight = (int)bottomRight.Position.x;
    }
}
