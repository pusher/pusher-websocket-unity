using System.Text;
using UnityEngine;



public class PusherMover : MonoBehaviour
{
    private enum State
    {
        RUNLEFT,
        RUNRIGHT,
        IDLE,
        ATTACK
    }

    private GameObject background;
    private PusherManager _pusherManager;
 
    static Animator anim;
    private Vector2 targetPos;


    public float stepSize = 0.1f;
    public int direction = -1;
    AudioSource audioData;
    State currentState = State.IDLE;
    State prevState;


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


        // control the character manually
        if (Input.GetKey(KeyCode.RightArrow))
        {
            currentState = State.RUNRIGHT;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            currentState = State.RUNLEFT;
        }
        else if (Input.GetKey(KeyCode.Space))
        {
            currentState = State.ATTACK;
        }
        else
        {
            currentState = State.IDLE;
        }


        if (currentState == State.RUNRIGHT)
        {
            run(1);
        }
        else if (currentState == State.RUNLEFT)
        {
            run(-1);
        }
        else if (currentState == State.ATTACK)
        {
            attack();
        }
        else if (currentState == State.IDLE)
        {
            idle();
        }

        sync(currentState);
        prevState = currentState;
    }

    void attack()
    {
        audioData.Play(0);
        anim.SetBool("IsRunning", false);
        anim.SetBool("IsAttacking", true);
        anim.SetBool("IsIdle", false);
    }

    void run(int dir)
    {
        anim.SetBool("IsRunning", true);
        anim.SetBool("IsAttacking", false);
        anim.SetBool("IsIdle", false);
        targetPos = new Vector2(transform.position.x + stepSize * dir, transform.position.y);
        transform.localScale = new Vector3(-dir, 1, 1);
        transform.position = targetPos;
    }

    void idle()
    {
        anim.SetBool("IsRunning", false);
        anim.SetBool("IsAttacking", false);
        anim.SetBool("IsIdle", true);
    }

    void sync(State s)
    {
        if (currentState != prevState)
        {
            _pusherManager.ClientEvent("client-position", ConvertPosToString(targetPos));
        }
    }

    static string ConvertPosToString(Vector2 array)
    {
        // Concatenate all the elements into a StringBuilder.
        StringBuilder builder = new StringBuilder();

        builder.Append(array.x);
        builder.Append('|');
        builder.Append(array.y);

        return builder.ToString();
    }

    static Vector2 ConvertStringToPos(string p)
    {
        string[] posArr = p.Split();
        Vector2 vp = new Vector2(
            float.Parse(posArr[0]),
            float.Parse(posArr[1])
        );
        return vp;
    }

}
