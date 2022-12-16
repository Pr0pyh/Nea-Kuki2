using Godot;
using System;

public class Joystick : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    TouchScreenButton touchButton;
    [Signal]
    delegate void touchMove(Vector2 moveVector);
    Player player;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        touchButton = GetNode<TouchScreenButton>("TouchScreenButton");
        player = (Player)GetParent().GetParent();
    }

    public override void _Input(InputEvent @event)
    {
        if(@event is InputEventScreenTouch buttonEvent)
        {
            GD.Print("working");
            if(touchButton.IsPressed())
            {
                Vector2 move_vector = calculate_vector(buttonEvent.Position);
                player.velocity = move_vector;
                EmitSignal(nameof(touchMove));
            }

        }
        else if(@event is InputEventScreenDrag dragEvent)
        {
            if(touchButton.IsPressed())
            {
                Vector2 move_vector = calculate_vector(dragEvent.Position);
                player.velocity = move_vector;
                EmitSignal(nameof(touchMove));
            }
        }
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    // public override void _Process(float delta)
    // {
    //     GD.Print("tick");
    // }
    Vector2 calculate_vector(Vector2 eventPos)
    {
        Vector2 textureCenter = touchButton.Position + new Vector2(20, 20);
        return (eventPos - textureCenter).Normalized();
    }
}
