using Godot;
using System;

public class MainMenu : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }

    private void _on_Button_pressed()
    {
        GetTree().ChangeScene("res://Tokyo-Old/Level1_0.tscn");
    }
}
