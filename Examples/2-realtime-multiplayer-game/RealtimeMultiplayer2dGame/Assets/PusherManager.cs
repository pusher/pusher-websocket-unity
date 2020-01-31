using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using PusherClient;
using UnityEngine;
using System.Linq;
using System.Collections;
using System.Text;

public class PusherManager : MonoBehaviour
{
    public class Pair<T1, T2>
    {
        public T1 First { get; set; }
        public T2 Second { get; set; }
    }

    // A mutation of https://unity3d.com/learn/tutorials/projects/2d-roguelike-tutorial/writing-game-manager
    public static PusherManager instance = null;
    public ConcurrentDictionary<string, dynamic> mbrs = new ConcurrentDictionary<string, dynamic>();
    public ConcurrentQueue<int> crossThreadQueue = new ConcurrentQueue<int>();

    private Pusher _pusher;
    private static PresenceChannel _channel;
    private const string APP_KEY = "APP_KEY";
    private const string APP_CLUSTER = "APP_CLUSTER";
    private const string APP_AUTH_ENDPOINT = "APP_AUTH_ENDPOINT";
    private const int MAX_OPPONENTS = 100;
    public GameObject opponent;
    public List<Pair<GameObject, OpponentMover>> opponents = new List<Pair<GameObject,OpponentMover>>();
    private int currOpponents = 0;


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


        //opponent = GameObject.Find("opponent");
        //opponentClone = Instantiate(opponent, new Vector3(-1, 1, 1), new Quaternion());
        //var op = opponentClone.GetComponent<OpponentMover>();

        opponent = GameObject.Find("opponent");
        opponent.SetActive(false);

        for (int o = 0; o < MAX_OPPONENTS; o++)
        {
            var obj = Instantiate(opponent, new Vector2(-1, -1), new Quaternion());
            var mover = obj.GetComponent<OpponentMover>();
            Pair<GameObject, OpponentMover> p = new Pair<GameObject, OpponentMover>();
            p.First = obj;
            p.Second = mover;
            opponents.Add(p);
        }

    }

    private void Update()
    {
        int res;
        crossThreadQueue.TryDequeue(out res);
        if (res != 0) {
            opponents[res].First.SetActive(true);
        }
    }

    //void Update()
    //{
    //    private string result;
    //}

    private async Task InitialisePusher()
    {
        //Environment.SetEnvironmentVariable("PREFER_DNS_IN_ADVANCE", "true");

        if (_pusher == null && (APP_KEY != "APP_KEY") && (APP_CLUSTER != "APP_CLUSTER") && (APP_AUTH_ENDPOINT != "APP_AUTH_ENDPOINT"))
        {
            _pusher = new Pusher(APP_KEY, new PusherOptions()
            {
                Cluster = APP_CLUSTER,
                Authorizer = new HttpAuthorizer(APP_AUTH_ENDPOINT),
                Encrypted = true
            });

            _pusher.Error += OnPusherOnError;
            _pusher.ConnectionStateChanged += PusherOnConnectionStateChanged;
            _pusher.Connected += PusherOnConnected;
            _pusher.Disconnected += PusherOnDisconnected;

            _channel = (PresenceChannel)_pusher.SubscribeAsync("presence-channel").Result;
            _channel.Subscribed += OnChannelOnSubscribed;
            _channel.MemberAdded += OnPresenceMemberAdded;
            _channel.MemberRemoved += OnPresenceMemberRemoved;

            await _pusher.ConnectAsync();
        }
        else
        {
            Debug.LogError("APP_KEY and APP_CLUSTER and APP_AUTH_ENDPOINT must be correctly set. Find how to set it at https://dashboard.pusher.com");
        }
    }

    private void PusherOnConnected(object sender)
    {
        Debug.Log("Connected");

        _channel.Bind("client-position", (PusherEvent ev) =>
        {
            //Debug.Log(ev.Data);
            Vector2 posData = ConvertStringToPos(ev.Data);
            Debug.LogFormat("{0} {1}", ev.UserId, ev.Data);
            ((IDictionary)mbrs)[ev.UserId] = posData;
        });
    }

    private void PusherOnDisconnected(object sender)
    {
        Debug.Log("Disconnected");
    }

    private void PusherOnConnectionStateChanged(object sender, ConnectionState state)
    {
        Debug.LogFormat("Connection state changed: {0}", state);
    }

    private void OnPusherOnError(object s, PusherException e)
    {
        Debug.LogFormat("Errored: {0}", e);
    }

    private void OnChannelOnSubscribed(object s)
    {
        Debug.Log("Subscribed");

        foreach (KeyValuePair<string, dynamic> v in _channel.Members)
        {
            ((IDictionary)mbrs).Add(v.Key, v.Value);
            //opponents[currOpponents].First.SetActive(true);
            opponents[currOpponents].Second.opponendId = v.Key;
            crossThreadQueue.Enqueue(currOpponents);
            currOpponents++;
            Debug.Log(v.Key);
        }
    }

    private void OnPresenceMemberAdded(object sender, KeyValuePair<string, dynamic> member)
    {
        Debug.Log(member.Key + " has joined");
        mbrs.TryAdd(member.Key, new Vector3(-1, -1, 0));

        opponents[currOpponents].Second.opponendId = member.Key;
        crossThreadQueue.Enqueue(currOpponents);
        currOpponents++;
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

        string[] posArr = p.Split(new Char[] { '|' });
        Vector2 vp = new Vector2(
            float.Parse(posArr[0]),
            float.Parse(posArr[1])
        );
        return vp;
    }
}
