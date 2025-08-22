using System;
using System.Threading.Tasks;
using PusherClient;
using UnityEngine;

public enum State
{
    RUNLEFT,
    RUNRIGHT,
    IDLE,
    ATTACK
}

public class PusherManager : MonoBehaviour
{
    // A mutation of https://unity3d.com/learn/tutorials/projects/2d-roguelike-tutorial/writing-game-manager
    public static PusherManager instance = null;
    private Pusher _pusher;
    private Channel _channel;
    private const string APP_KEY = "APP_KEY";
    private const string APP_CLUSTER = "APP_CLUSTER";
    private State _state = State.IDLE;

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
        _channel.Bind("run-left", (dynamic data) =>
        {
            _state = State.RUNLEFT;
        });

        _channel.Bind("run-right", (dynamic data) =>
        {
            _state = State.RUNRIGHT;
        });

        _channel.Bind("idle", (dynamic data) =>
        {
            _state = State.IDLE;
        });

        _channel.Bind("attack", (dynamic data) =>
        {
            _state = State.ATTACK;
        });
        Debug.Log("Connected");
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

    public void Message(string message)
    {
        _channel?.Trigger("time has occured", message);
    }

    public State CurrentState()
    {
        return _state;
    }

    async Task OnApplicationQuit()
    {
        if (_pusher != null)
        {
            await _pusher.DisconnectAsync();
        }
    }
}
