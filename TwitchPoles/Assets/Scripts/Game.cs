using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Collections.Generic;
using Assets.Scripts;

public class Game : MonoBehaviour
{
    public bool UseManualInput;
    public bool DebugAllMessages;
    TwitchIRC _irc;
    MessageDebugger _messageDebugger;
    Arena _mob;
    WordPicker _wordPicker;
    private Coroutine _fakeUpdate;

    void Start ()
    {
        _irc = this.GetComponent<TwitchIRC>();

        _mob = new Arena();

        _wordPicker = new WordPicker();
        if(DebugAllMessages)
            _messageDebugger = new MessageDebugger();

        if (_irc.enabled)
            _irc.messageRecievedEvent.AddListener(MessageRecieved);
        else if(!UseManualInput)
            _fakeUpdate = StartCoroutine(DoFakeUpdate());

        if(UseManualInput)
            new CanvasInput().OnMessageRecieved += MessageRecieved;

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
            yield return new WaitForSeconds(UnityEngine.Random.Range(0.5f, 0.1f));
        }
    }

    private void MessageRecieved(string msg)
    {
        if(_fakeUpdate == null)
        {
            var logPath = string.Concat(Directory.GetCurrentDirectory(), "/Assets/Resources/TwitchLogs/", _irc.channelName, ".txt");
            using (var writer = new StreamWriter(logPath, true))
                writer.WriteLine(msg);
        }

        if (msg.Length > 1)
        {
            var pipeAt = msg.IndexOf(':', 1);
            if (pipeAt >= 0)
            {
                pipeAt++;
                msg = msg.Substring(pipeAt, msg.Length - pipeAt);
            }
        }

        _wordPicker.MessageRecieved(msg);
        if(_messageDebugger != null)
            _messageDebugger.MessageRecieved(msg);
    }
}
