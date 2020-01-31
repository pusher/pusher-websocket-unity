using System.Text;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class OpponentMover : MonoBehaviour
{
    private enum State
    {
        RUNLEFT,
        RUNRIGHT,
        IDLE,
        ATTACK
    }

    public float stepSize = 0.1f;
    public int direction = -1;
    public string opponendId = "";

    private GameObject background;
    private PusherManager _pusherManager;

    static Animator anim;
    private Vector2 targetPos;

    AudioSource audioData;
    State currentState = State.IDLE;
    State prevState;
    private Vector2 targPos;
    private Vector2 oldTargPos = new Vector3(-1, 1, 1);

    void Start()
    {
        anim = GetComponent<Animator>();
        if (_pusherManager == null)
        {
            background = GameObject.Find("flat_nature_art");
            _pusherManager = background.GetComponent<PusherManager>();
        }

        transform.localScale = new Vector3(-1, 1, 1);
        audioData = GetComponent<AudioSource>();
        prevState = currentState;

    }

    // Update is called once per frame
    void Update()
    {
        audioData.Pause();

        //State currentState = _pusherManager.CurrentState();
        if (opponendId != "" && _pusherManager.mbrs.ContainsKey(opponendId))
        {
           
            targPos = (Vector2)_pusherManager.mbrs[opponendId];
            //Debug.LogFormat("{0} {1}", targPos, oldTargPos);

            if (targPos != oldTargPos)
            {

                run(targPos);

                oldTargPos = targPos;

            }
            else
            {
                idle();
            }

            oldTargPos = targPos;
            
        } else
        {
            idle();
        }


        // control the character manually
        //if (Input.GetKey(KeyCode.RightArrow))
        //{
        //    currentState = State.RUNRIGHT;
        //}
        //else if (Input.GetKey(KeyCode.LeftArrow))
        //{
        //    currentState = State.RUNLEFT;
        //}
        //else if (Input.GetKey(KeyCode.Space))
        //{
        //    currentState = State.ATTACK;
        //}
        //else
        //{
        //    currentState = State.IDLE;
        //}


        //if (currentState == State.RUNRIGHT)
        //{
        //    run(1);
        //}
        //else if (currentState == State.RUNLEFT)
        //{
        //    run(-1);
        //}
        //else if (currentState == State.ATTACK)
        //{
        //    attack();
        //}
        //else if (currentState == State.IDLE)
        //{
        //    idle();
        //}

        // sync(currentState);
        // prevState = currentState;
    }

    void attack()
    {
        audioData.Play(0);
        anim.SetBool("IsRunning", false);
        anim.SetBool("IsAttacking", true);
        anim.SetBool("IsIdle", false);
    }

    void run(Vector2 tg)
    {
        int dir = 1;
        anim.SetBool("IsRunning", true);
        anim.SetBool("IsAttacking", false);
        anim.SetBool("IsIdle", false);
        if (tg.x < transform.position.x) {
            dir = - 1;
        } else
        {
            dir = 1;
        }
        transform.localScale = new Vector3(-dir, 1, 1);
        transform.position = tg;
    }

    void idle()
    {
        anim.SetBool("IsRunning", false);
        anim.SetBool("IsAttacking", false);
        anim.SetBool("IsIdle", true);
    }

    //void sync(State s)
    //{
    //    if (currentState != prevState)
    //    {
    //        _pusherManager.ClientEvent("client-position", ConvertPosToString(targetPos));
    //    }
    //}

}
