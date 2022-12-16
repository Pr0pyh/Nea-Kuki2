using Godot;
using System;

public class Player : KinematicBody2D
{
    [Export] public int speed = 5;
    [Export] public bool partner;
    [Export] public bool horizontalInputOnly;

    public enum EVENT{
        MOVE,
        STOP,
        HURT
    };
    public EVENT state = EVENT.MOVE;
    public Vector2 velocity = new Vector2(0.1f, 0.0f);
    public AnimationPlayer animPlayer;
    public AnimationPlayer animPlayer2;
    public AnimationTree animTree;
    public AnimationNodeStateMachinePlayback animState;
    public Camera2D camera;
    public Sprite sprite;
    public Timer timer;
    public player2 _player;
    Color newModulate;

    Position2D startPos;
    bool android = true;
    public int health = 4;
    public int score = 0;

    private bool invincible = false;


    public override void _Ready()
    {
        state = EVENT.MOVE;
        camera = GetParent().GetParent().GetNode<Camera2D>("CameraPlayer");
        animPlayer = this.GetNode<AnimationPlayer>("AnimationPlayer");
        animTree = this.GetNode<AnimationTree>("AnimationTree");
        animTree.Active = true;
        animState = (AnimationNodeStateMachinePlayback)animTree.Get("parameters/playback");
        animPlayer2 = this.GetNode<AnimationPlayer>("AnimationPlayer2");
        sprite = this.GetNode<Sprite>("Sprite");
        timer = this.GetNode<Timer>("Timer");
        newModulate = Modulate;
        if(partner)
            _player = this.GetParent().GetParent().GetNode<player2>("Player2");
        Modulate = new Color(1.0f, 1.0f, 1.0f, 1.0f);
    }

    public override void _PhysicsProcess(float delta)
    {
        switch(state) 
        {
            case EVENT.MOVE:
                if(android == false)
                    move_state(delta);
                else
                    MoveAndSlide(velocity * speed);
                break;
            case EVENT.STOP:
                animTree.Set("parameters/Idle/blend_position", velocity);
                animTree.Set("parameters/Run/blend_position", velocity);
                break;
            case EVENT.HURT:
                MoveAndSlide(velocity);
                break;
        }
    }

    private void move_state(float delta)
    {
        Vector2 input_vector = new Vector2();

        if (Input.IsActionPressed("move_right"))
            input_vector.x += 1;

        if (Input.IsActionPressed("move_left"))
            input_vector.x -= 1;

        if (Input.IsActionPressed("down") && !horizontalInputOnly)
            input_vector.y += 1;

        if (Input.IsActionPressed("up") && !horizontalInputOnly)
            input_vector.y -= 1;
            

        input_vector = input_vector.Normalized();

        if(input_vector != Vector2.Zero)
        {
            animTree.Set("parameters/Idle/blend_position", velocity);
            animTree.Set("parameters/Run/blend_position", velocity);
            animState.Travel("Run");
        }
        else
        {
            animState.Travel("Idle");
        }
        velocity = input_vector*speed;
        velocity = MoveAndSlide(velocity);

        for(int i = 0; i<GetSlideCount(); i++)
        {
            KinematicCollision2D collision = GetSlideCollision(i);
            if(((Node2D)collision.Collider).IsInGroup("enemies"))
            {
                ((enemy)collision.Collider).destroy();
                // if(_player != null)
                //     _player.objectiveText("Skupila si sve vile", "Fali ti jos vila", score);
            }
        }
    }

    public void inAnimationMove()
    {
        if(state == EVENT.STOP)
        {
            animTree.Set("parameters/Run/blend_position", velocity);
            animState.Travel("Run");
        }
    }

    public void inAnimationStop()
    {
        if(state == EVENT.STOP)
        {
            animState.Travel("Idle");
        }
    }

    private void _on_Director_animation_finished(String animName)
    {
        camera.Zoom = new Vector2(1.5f, 1.5f);
        state = EVENT.MOVE;
    }

    private void _on_Director_animation_started(String animName)
    {
        state = EVENT.STOP;
    }

    private void _on_IntroExit_animation_started(String animName)
    {
        state = EVENT.STOP;
    }

    private void _on_IntroExit_animation_finished(String animName)
    {
        state = EVENT.MOVE;
    }

    private void _on_Hitbox_body_entered(Node body)
    {
        checkHealth(body);
    }

    private void _on_Hitbox_area_entered(Node area)
    {
        checkHealth(area);
    }

    private void checkHealth(Node collider)
    {
        if(collider.IsInGroup("enemies_moving") && invincible == false)
        {
            if(health <= 0)
                GetTree().ReloadCurrentScene();
            health--;
            invincible = true;
            animPlayer2.Play("Hurt");
            velocity = velocity.Normalized() * (-50);
            state = EVENT.HURT;
            timer.Start();
            GD.Print(collider.Name);
        }
    }

    private void _on_AnimationPlayer2_animation_finished(String animName)
    {
        
    }

    private void _on_Timer_timeout()
    {
        animPlayer2.Stop();
        invincible = false;
        state = EVENT.MOVE;
        newModulate.g -= 0.2f;
        newModulate.b -= 0.2f;
        GD.Print(newModulate);
        Modulate = newModulate;
    }


}
