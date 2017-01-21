using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using Assets.Scripts;

public class Game : MonoBehaviour
{
    public TextMesh Text;
    TwitchIRC _irc;
    MessageDebugger _messageDebugger;
    Arena _mob;

    void Start ()
    {
        _irc = this.GetComponent<TwitchIRC>();

        _mob = new Arena();

        _messageDebugger = new MessageDebugger();

        if (_irc.enabled)
            _irc.messageRecievedEvent.AddListener(MessageRecieved);
        else
            StartCoroutine(DoFakeUpdate());

    }

    void Update()
    {
        TinyCoro.StepAllCoros();
    }

    IEnumerator DoFakeUpdate()
    {
        var lines = new List<string>();
        foreach(var file in Directory.GetFiles(Directory.GetCurrentDirectory()+"/Assets/Resources/TwitchLogs"))
            lines.AddRange(File.ReadAllLines(file));

        var i = 0;
        while (true)
        {
            var line = lines[i];
            i = (i + 1) % lines.Count;

            MessageRecieved(line);
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.1f, 1f));
        }
    }

    private void MessageRecieved(string msg)
    {
        var logPath = string.Concat(Directory.GetCurrentDirectory(), "/Assets/Resources/TwitchLogs/", _irc.channelName, ".txt");
        using (var writer = new StreamWriter(logPath, true))
            writer.WriteLine(msg);

        var pipeAt = msg.IndexOf(':', 2);
        if (pipeAt >= 0)
        {
            pipeAt++;
            msg = msg.Substring(pipeAt, msg.Length - pipeAt);
        }

        _messageDebugger.MessageRecieved(msg);
    }
}
