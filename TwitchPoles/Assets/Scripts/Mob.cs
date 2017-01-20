using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class Mob : UnityObject
    {
        ParticleSystem _particleSystem;
        List<Member> _members;

        public Mob()
            : base(Assets.Spawn<GameObject>("Mob"))
        {
            _members = new List<Member>();

            _particleSystem = GameObject.GetComponent<ParticleSystem>();

            u.Update += Update;
        }

        void Update(UnityObject uObj)
        {
            if(_members.Count < 100)
                Spawn();

            foreach (var member in _members)
                member.Update();

            var particles = _members.Select(p => p.ToParticle()).ToArray();

            _particleSystem.SetParticles(particles, particles.Length);
            _particleSystem.time = 0f;
        }


        void Spawn()
        {
            for(var i = 0; i < 2; ++i)
            {
                var team = (Team)i;
                _members.Add(new Member()
                {
                    Team = team,
                    Seed = UnityEngine.Random.Range(0, 100f),
                    Position = new Vector3(team.GetArenaXStart(), 0f, UnityEngine.Random.Range(0, World.Size.z)),
                    Velocity = team.GetDirection() * UnityEngine.Random.Range(0.5f, 1f) * 100f,
                });
            }
        }
    }

}
