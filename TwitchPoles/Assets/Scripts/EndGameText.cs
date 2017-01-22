using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    public class EndGameText : UnityObject
    {
        TextMesh _text;
        Team _winningTeam;

        public EndGameText(Team team)
            : base (Assets.Spawn<GameObject>("EndGameText"))
        {
            _winningTeam = team;
            _text = GameObject.GetComponent<TextMesh>();
            TinyCoro.SpawnNext(EndTheGame);
        }

        private IEnumerator EndTheGame()
        {
            string winMsg;
            if (_winningTeam == Team.lower)
                winMsg = "lower case\nwarriors\nwin!";
            else
                winMsg = "ALL CAPS\nCHAMPIONS\nWIN!";

            _text.text = "";
            foreach(var c in winMsg)
            {
                _text.text += c;
                yield return TinyCoro.WaitSeconds(0.1f);
            }

            yield return TinyCoro.WaitSeconds(10f);

            foreach (var coro in TinyCoro.AllCoroutines.ToList())
                coro.Kill();

            SceneManager.LoadScene("intro");
        }
    }
}
