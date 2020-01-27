using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using PusherClient;
using UnityEngine;
using System.Linq;
using System.Collections;

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
    private ConcurrentDictionary<string, dynamic> mbrs = new ConcurrentDictionary<string, dynamic>();

    private Pusher _pusher;
    private static PresenceChannel _channel;
    private const string APP_KEY = "b928ab800c5c554a47ad";
    private const string APP_CLUSTER = "mt1";


    async Task Start()
    {
        await InitialisePusher();

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private async Task InitialisePusher()
    {
        //Environment.SetEnvironmentVariable("PREFER_DNS_IN_ADVANCE", "true");

        if (_pusher == null && (APP_KEY != "APP_KEY") && (APP_CLUSTER != "APP_CLUSTER"))
        {
            _pusher = new Pusher(APP_KEY, new PusherOptions()
            {
                Cluster = APP_CLUSTER,
                Authorizer = new HttpAuthorizer("https://node-auth.damdo.now.sh/api/auth"),
                Encrypted = true
            });

            _pusher.Error += OnPusherOnError;
            _pusher.ConnectionStateChanged += PusherOnConnectionStateChanged;
            _pusher.Connected += PusherOnConnected;

            _channel = (PresenceChannel)_pusher.SubscribeAsync("presence-channel").Result;
            _channel.Subscribed += OnChannelOnSubscribed;
            _channel.MemberAdded += OnPresenceMemberAdded;
            _channel.MemberRemoved += OnPresenceMemberRemoved;

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

        _channel.Bind("client-position", (dynamic data) =>
        {
            Debug.Log(data);
        });

    }

    private void PusherOnConnectionStateChanged(object sender, ConnectionState state)
    {
        Debug.Log("Connection state changed:");
        Debug.Log(state);
    }

    private void OnPusherOnError(object s, PusherException e)
    {
        Debug.Log("Errored");
        Debug.Log(e);
    }

    private void OnChannelOnSubscribed(object s)
    {
        Debug.Log("Subscribed");
        foreach (KeyValuePair<string, dynamic> v in _channel.Members)
        {
            ((IDictionary)mbrs).Add(v.Key, v.Value);
        }
    }

    private void OnPresenceMemberAdded(object sender, KeyValuePair<string, dynamic> member)
    {
        Debug.Log(member.Key + " has joined");
        ((IDictionary)mbrs).Add(member.Key, member.Value);
    }

    private void OnPresenceMemberRemoved(object sender)
    {
        foreach (KeyValuePair<string, dynamic> entry in mbrs)
        {
            if (!_channel.Members.ContainsKey(entry.Key))
            {
                Debug.Log(entry.Key + " has left");
                ((IDictionary)mbrs).Remove(entry.Key);
            }
        }
    }

    private void ListMembers()
    {
        var names = new List<string>();

        foreach (var mem in mbrs)
        {
            names.Add(mem.Key);
        }

        Debug.Log("[MEMBERS] " + names.Aggregate((i, j) => i + ", " + j));
    }

    public void ClientEvent(string client_event, string message)
    {
        _channel?.Trigger(client_event, message);
    }

    async Task OnApplicationQuit()
    {
        if (_pusher != null)
        {
            await _pusher.DisconnectAsync();
        }
    }
}
