using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class MessageDebugger
    {
        const int MaxMessages = 50;
        List<string> _msgs;
        TextMesh _text;
        public MessageDebugger(TextMesh text)
        {
            _text = text;
            _msgs = new List<string>();
        }

        public void MessageRecieved(string msg)
        {
            _msgs.Add(msg);
            if(_msgs.Count > MaxMessages)
                _msgs.RemoveAt(0);

            _text.text = string.Join("\n", _msgs.ToArray());
        }
    }
}
