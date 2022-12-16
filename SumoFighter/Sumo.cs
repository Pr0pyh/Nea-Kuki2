using Godot;
using System;

public class Sumo : Area2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    
    // Called when the node enters the scene tree for the first time.
    Timer damage;
    Timer noDamage;
    AnimatedSprite HitEffect;
    CollisionShape2D shape;
    public override void _Ready()
    {
        damage = GetNode<Timer>("Damage");
        noDamage = GetNode<Timer>("NoDamage");
        HitEffect = GetNode<AnimatedSprite>("HitEffect");
        shape = GetNode<CollisionShape2D>("CollisionShape2D");
        HitEffect.Visible = false;
        shape.Disabled = true;
        noDamage.Start();
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    private void _on_Timer_timeout()
    {
        shape.Disabled = false;
        HitEffect.Visible = true;
        damage.Start();
    }

    private void _on_Damage_timeout()
    {
        shape.Disabled = true;
        HitEffect.Visible = false;
        noDamage.Start();
    }
}
