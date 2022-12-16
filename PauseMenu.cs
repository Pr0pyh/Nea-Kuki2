using Godot;
using System;

public class PauseMenu : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    bool is_vis;
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Visible = false;
        is_vis = false;
    }

    public override void _Input(InputEvent @event)
    {
        if(Input.IsActionPressed("ui_cancel"))
        {
            is_vis = !Visible;
            Visible = is_vis;
        }
    }

    private void _on_RESUME_pressed()
    {
        if(Visible)
            is_vis = !Visible;
        Visible = is_vis;
    }

    private void _on_EXIT_pressed()
    {
        if(Visible)
            GetTree().ChangeScene("res://MainMenu.tscn");
    }

}
