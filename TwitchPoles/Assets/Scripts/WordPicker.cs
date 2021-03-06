﻿using System;
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
        Dictionary<string, SiegeWord> _words = new Dictionary<string, SiegeWord>();
        List<string> _recentLines = new List<string>();

        public WordPicker()
        {
            TinyCoro.SpawnNext(DoPick);
            TinyCoro.SpawnNext(PushbackUnusedWords);

        }

        private IEnumerator DoPick()
        {
            while(true)
            {
                if (_recentLines.Any() )
                { 
                    var recentIndex = UnityEngine.Random.Range(0, _recentLines.Count);
                    var recentLine = _recentLines[recentIndex];
                    _recentLines.RemoveAt(recentIndex);

                    var spawnedOne = false;
                    var handledKeys = new List<string>();
                    foreach (var word in recentLine.Split(' '))
                    {
                        var key = word.ToLower();
                        if (handledKeys.Contains(key))
                            continue;
                        handledKeys.Add(key);

                        Team wordTeam;
                        if (GetTeam(word, out wordTeam))
                        {
                            if (_words.ContainsKey(key))
                            {
                                _words[key].Use(wordTeam);
                                continue;
                            }

                            var lane = GetUnusedLane();
                            if (!lane.HasValue)
                                continue;

                            spawnedOne = true;
                            var seigeWord = new SiegeWord(key, wordTeam, lane.Value);

                            _words.Add(key, seigeWord);
                            seigeWord.OnDispose += d => _words.Remove((d as SiegeWord).Key);
                        }
                    }

                    if (!spawnedOne)
                        continue;

                    yield return TinyCoro.WaitSeconds(0.25f);
                }
                else
                    yield return null;
            }
        }

        int? GetUnusedLane()
        {
            var unusedLanes = new List<int>();
            for (int z = 4; z < Arena.Depth; z += SiegeWord.CellsHigh / 2)
            {
                if (_words.Values.Any(w => (w.SiegeDriver != null && w.SiegeDriver.Cell.z >= z && w.SiegeDriver.Cell.z <= z + SiegeWord.CellsHigh)
                                            || ((int)(w.WorldPosition.y / 8) >= z && (int)(w.WorldPosition.y / 8) <= z + SiegeWord.CellsHigh)))
                    continue;
                unusedLanes.Add(z);
            }

            if (unusedLanes.Any())
                return unusedLanes[UnityEngine.Random.Range(0, unusedLanes.Count)];
            else
                return null;
        }

        public void MessageRecieved(string msg)
        {
            var used = false;
            var handledKeys = new List<string>();
            foreach (var word in msg.Split(' '))
            {
                Team wordTeam;
                if(GetTeam(word, out wordTeam))
                {
                    var key = word.ToLower();
                    if (handledKeys.Contains(key))
                        continue;
                    handledKeys.Add(key);
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

        private IEnumerator PushbackUnusedWords()
        {
            while(true)
            {
                foreach(var word in _words.Values.Where(w => (Time.time - w.LastUsedAt) > 10f).ToList())
                {
                    word.Use(word.Team.GetOpponent());

                    yield return TinyCoro.WaitSeconds(0.5f);
                }

                yield return TinyCoro.WaitSeconds(3f);
            }
        }

    }
}
