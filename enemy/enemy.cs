using Godot;
using System;

public class enemy : KinematicBody2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    enum EVENT {
        VISIBLE,
        NOT_VISIBLE
    };

    player2 _player;
    Player player;
    Area2D collision_p2;
    EVENT state;
    AnimationPlayer deathAnim;
    Sprite enemySprite;
    AnimatedSprite sprite;
    CollisionShape2D collision2D;
    public AudioStreamPlayer audioPlayer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _player = this.GetParent().GetNode<player2>("Player2");
        player = this.GetParent().GetNode<YSort>("YSort").GetNode<Player>("Player");
        sprite = this.GetNode<AnimatedSprite>("Sprite");
        enemySprite = this.GetNode<Sprite>("EnemySprite");
        audioPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
        collision2D = GetNode<CollisionShape2D>("CollisionShape2D");
        _player.Connect("body_entered", this, "_on_Collision_body_entered");
        _player.Connect("body_exited", this, "_on_Collision_body_exited");
        deathAnim = this.GetNode<AnimationPlayer>("AnimationPlayer");
        state = EVENT.NOT_VISIBLE;
        sprite.Visible = true;
        enemySprite.Visible = false;
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        switch(state) 
        {
            case EVENT.VISIBLE:
                Modulate = new Color(1, 1, 1, (255-(Position.DistanceTo(_player.Position)))/255);
                break;
            case EVENT.NOT_VISIBLE:
                Modulate = new Color(1, 1, 1, 0);
                break;
        }
    }

    private void _on_Collision_body_entered(Node body)
    {
        this.state = EVENT.VISIBLE;
    }

    public void _on_Collision_body_exited(Node body)
    {
        this.state = EVENT.NOT_VISIBLE;
    }

    public void destroy()
    {
        collision2D.Disabled = true;
        if(Modulate > new Color(1, 1, 1, 0))
            deathAnim.Play("DeathAnimation");
        audioPlayer.Play();
        GD.Print(player.score);
    }

    private void _on_AnimationPlayer_animation_finished(String animName)
    {
        QueueFree();
        player.score++;
        _player.objectiveText("You gathered all.", "You need more.", player.score);
    }
}
