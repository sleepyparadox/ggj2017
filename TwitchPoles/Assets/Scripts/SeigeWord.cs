using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class SeigeWord : UnityObject
    {
        const float MinScale = 1f;
        const float MaxScale = 4f;
        const float ShrinkSpeed = 1f;
        public readonly string Key;
        int _usesNeed = 5;
        float _health = 0;
        TextMesh _text;

        public SeigeWord(string word, Team team)
            : base(Assets.Spawn<GameObject>("Word"))
        {
            Key = word;
            _text = GameObject.GetComponent<TextMesh>();
            _text.text = word.ToTeam(team);
            
            WorldPosition = new Vector3(UnityEngine.Random.Range(0, 800), UnityEngine.Random.Range(0, 600), 0f);

            Use(team);

            u.Update += uo =>
            {
                _health -= ShrinkSpeed * (1f / _usesNeed) * Time.deltaTime;
                if (_health <= 0)
                    Dispose();
                else
                    UpdateScale();
            };
        }

        public void Use(Team team)
        {
            _health += (1f / _usesNeed);
            UpdateScale();
        }

        void UpdateScale()
        {
            LocalScale = Vector3.one * Mathf.Lerp(MinScale, MaxScale, _health);
        }
    }
}
