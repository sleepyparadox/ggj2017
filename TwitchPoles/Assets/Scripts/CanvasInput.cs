using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts
{
    public class CanvasInput : UnityObject
    {
        public Action<string> OnMessageRecieved;
        InputField _inputField;

        public CanvasInput()
            : base (Assets.Spawn<GameObject>("CanvasInput"))
        {
            _inputField = GameObject.GetComponentInChildren<UnityEngine.UI.InputField>();
            u.Update += Update;
        }

        private void Update(UnityObject uObj)
        {
            Focus();
            if (!string.IsNullOrEmpty(_inputField.text)
                && Input.GetKeyDown(KeyCode.Return))
            {
                if (OnMessageRecieved != null)
                    OnMessageRecieved(_inputField.text);
                //_inputField.text = string.Empty; //spamming is useful

                Focus();
            }
        }

        void Focus()
        {
            if (EventSystem.current != null)
            {
                EventSystem.current.SetSelectedGameObject(_inputField.gameObject, null);
                _inputField.OnPointerClick(new PointerEventData(EventSystem.current));
            }
        }
    }
}
