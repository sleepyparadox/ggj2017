using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class WordPicker
    {
        const string Upper = "ABCDEFJHIJKLMNOPQRSTUVWXYZ";
        const string Lower = "abcdefghijklmnopqrstuvwxyz";
        Dictionary<string, SeigeWord> _words = new Dictionary<string, SeigeWord>();
        List<string> _recentLines = new List<string>();

        public WordPicker()
        {
            TinyCoro.SpawnNext(DoPick);
        }

        private IEnumerator DoPick()
        {
            while(true)
            {
                if (_recentLines.Any())
                {
                    var recentIndex = UnityEngine.Random.Range(0, _recentLines.Count);
                    var recentLine = _recentLines[recentIndex];
                    _recentLines.RemoveAt(recentIndex);

                    var spawnedOne = false;
                    foreach (var word in recentLine.Split(' '))
                    {
                        var key = word.ToLower();
                        Team wordTeam;
                        if (GetTeam(word, out wordTeam))
                        {
                            if (_words.ContainsKey(key))
                            {
                                _words[key].Use(wordTeam);
                                continue;
                            }

                            spawnedOne = true;
                            var seigeWord = new SeigeWord(key, wordTeam);

                            _words.Add(key, seigeWord);
                            seigeWord.OnDispose += d => _words.Remove((d as SeigeWord).Key);
                        }
                    }

                    if (!spawnedOne)
                        continue;

                    yield return TinyCoro.WaitSeconds(1f);
                }
                else
                    yield return null;
            }
        }

        public void MessageRecieved(string msg)
        {
            var used = false;
            foreach (var word in msg.Split(' '))
            {
                Team wordTeam;
                if(GetTeam(word, out wordTeam))
                {
                    used = used ||  WordArrived(wordTeam, word.ToTeam(wordTeam));
                }
            }

            if (!used)
            {
                _recentLines.Add(msg);
                if (_recentLines.Count > 50)
                    _recentLines.RemoveAt(0);
            }

        }

        bool GetTeam(string word, out Team team)
        {
            var upper = word.Count(c => Upper.Contains(c));
            var lower = word.Count(c => Lower.Contains(c));

            if (upper == 0 && lower == 0)
            {
                team = (Team)(-1);
                return false;
            }

            if (upper == lower)
            {
                // randonly weight
                upper += UnityEngine.Random.Range(-1, 2);
            }

            if (upper > lower)
                team = Team.UPPER;
            else
                team = Team.lower;

            return true;

        }

        bool WordArrived(Team team, string word)
        {
            if (_words.ContainsKey(word))
            {
                _words[word].Use(team);
                return true;
            }
            return false;
        }

    }
}
