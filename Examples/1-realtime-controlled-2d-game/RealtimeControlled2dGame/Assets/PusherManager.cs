using System;
using System.Threading.Tasks;
using PusherClient;
using UnityEngine;

public enum State
{
    IDLE,
    RUNRIGHT,
    RUNLEFT,
    ATTACK
}

public class PusherManager : MonoBehaviour
{
    // A mutation of https://unity3d.com/learn/tutorials/projects/2d-roguelike-tutorial/writing-game-manager
    public static PusherManager instance = null;
    private Pusher _pusher;
    private Channel _channel;
    private State _currentState = State.IDLE;
    private const string APP_KEY = "APP_KEY";
    private const string APP_CLUSTER = "APP_CLUSTER";

    async Task Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        await InitialisePusher();
        Console.WriteLine("Starting");
    }

    private async Task InitialisePusher()
    {
        //Environment.SetEnvironmentVariable("PREFER_DNS_IN_ADVANCE", "true");

        if (_pusher == null && (APP_KEY != "APP_KEY") && (APP_CLUSTER != "APP_CLUSTER"))
        {
            _pusher = new Pusher(APP_KEY, new PusherOptions()
            {
                Cluster = APP_CLUSTER,
                Encrypted = true
            });

            _pusher.Error += OnPusherOnError;
            _pusher.ConnectionStateChanged += PusherOnConnectionStateChanged;
            _pusher.Connected += PusherOnConnected;
            _channel = await _pusher.SubscribeAsync("my-channel");
			_pusher.Subscribed += OnChannelOnSubscribed;
            await _pusher.ConnectAsync();
        }
        else
        {
            Debug.LogError("APP_KEY and APP_CLUSTER must be correctly set. Find how to set it at https://dashboard.pusher.com");
        }
    }

    private void PusherOnConnected(object sender)
    {
        Debug.Log("Connected");

        // Bind to game control events
        _channel.Bind("move_right", (dynamic data) =>
        {
            Debug.Log("move_right received");
            SetCurrentState(State.RUNRIGHT);
        });

        _channel.Bind("move_left", (dynamic data) =>
        {
            Debug.Log("move_left received");
            SetCurrentState(State.RUNLEFT);
        });

        _channel.Bind("attack", (dynamic data) =>
        {
            Debug.Log("attack received");
            SetCurrentState(State.ATTACK);
        });

        _channel.Bind("idle", (dynamic data) =>
        {
            Debug.Log("idle received");
            SetCurrentState(State.IDLE);
        });

        // Keep the original event binding for compatibility
        _channel.Bind("my-event", (dynamic data) =>
        {
            Debug.Log("my-event received");
        });
    }

    private void PusherOnConnectionStateChanged(object sender, ConnectionState state)
    {
        Debug.Log("Connection state changed");
    }

    private void OnPusherOnError(object s, PusherException e)
    {
        Debug.Log("Errored");
    }

    private void OnChannelOnSubscribed(object s, Channel channel)
    {
        Debug.Log("Subscribed");
    }

    async Task OnApplicationQuit()
    {
        if (_pusher != null)
        {
            await _pusher.DisconnectAsync();
        }
    }

    public State CurrentState()
    {
        return _currentState;
    }

    public void SetCurrentState(State newState)
    {
        _currentState = newState;
    }
}
