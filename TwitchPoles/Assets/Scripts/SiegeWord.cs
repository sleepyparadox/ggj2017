using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class SiegeWord : UnityObject
    {
        public const int CellsHigh = 16;
        const float MinScale = 1f;
        const float MaxScale = 4f;
        const float ShrinkSpeed = 0.5f;
        public float LastUsedAt = 0f;
        public readonly string Key;
        public readonly Team Team;
        public  SiegeDriver SiegeDriver;
        int _usesNeed = 2;
        float _health = 0;
        TextMesh _text;

        public SiegeWord(string key, Team team, int z)
            : base(Assets.Spawn<GameObject>("Word"))
        {
            Key = key;
            GameObject.name = key + " " + team;

            _text = GameObject.GetComponent<TextMesh>();
            SetCaptialization(team);
            Team = team;

            WorldPosition = new Vector3(team.GetWordXStart() * 8, z * 8, 0f);

            Use(team);

            u.Update += Grow;
        }

        private void Grow(UnityObject uObj)
        {
            _health -= ShrinkSpeed * (1f / _usesNeed) * Time.deltaTime;
            if (_health <= 0)
                Dispose();
            else
                UpdateScale();
        }

        public void Use(Team team)
        {
            if (IsDisposed)
                return;

            LastUsedAt = Time.time;

            if (SiegeDriver != null)
                SiegeDriver.Use(team);
            else
            {
                _health += (1f / _usesNeed);

                if (_health >= 1f)
                {
                    Spawn();
                }

                UpdateScale();
            }
        }

        public void Spawn()
        {
            Debug.Log("Spawned " + Key);
            u.Update -= Grow;

            SiegeDriver = new SiegeDriver(this);
            Arena.S.Rioters.Add(SiegeDriver);
            UpdateScale();

            Arena.S.Score(Team);
        }

        void UpdateScale()
        {
            if(SiegeDriver != null)
                LocalScale = Vector3.one * MaxScale;
            else
                LocalScale = Vector3.one * Mathf.Lerp(MinScale, MaxScale, _health);
        }

        public void SetCaptialization(Team team, bool scoreOnChange = false)
        {
            var newText = Key.ToTeam(team);

            if(_text.text != newText && scoreOnChange)
                Arena.S.Score(team);

            _text.text = Key.ToTeam(team);
            _text.color = team.ToColor(true);
        }
    }
}
