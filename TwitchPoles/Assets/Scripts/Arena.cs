using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class Arena : UnityObject
    {
        public const float HitRadius = 10f;
        public const float HitRadiusSqr = 10f;

        public const int Depth = 75;
        public const int Width = 100;
        public const int Floor = 0;


        public Rioter[,] Grid;
        

        ParticleSystem _particleSystem;
        List<Rioter> _members;

        public Arena()
            : base(Assets.Spawn<GameObject>("Mob"))
        {
            _members = new List<Rioter>();
            _particleSystem = GameObject.GetComponent<ParticleSystem>();
            Grid = new Rioter[800, 600];


            u.Update += Update;
        }

        void Update(UnityObject uObj)
        {
            if(_members.Count < 100)
                Spawn();

            CheckForCollides();

            foreach (var member in _members)
                member.Update();

            var particles = _members.Select(p => p.ToParticle()).ToArray();

            _particleSystem.SetParticles(particles, particles.Length);
            _particleSystem.time = 0f;
        }

        void CheckForCollides()
        {
            foreach(var leftMember in _members.Where(m => m.Team == Team.Left))
            {
                var rightMember = _members.FirstOrDefault(m => m.Team == Team.Right 
                                                         && m.Velocity.y == 0
                                                         && (m.Position - leftMember.Position).sqrMagnitude < HitRadiusSqr);

                if(rightMember != null)
                {
                    leftMember.Velocity += (Vector3.left + Vector3.up) * 100f;
                    rightMember.Velocity += (Vector3.right + Vector3.up) * 100f;
                }
            }

        }


        void Spawn()
        {
            while(_members.Count < 100)
            {
                var team = Team.Left;
                _members.Add(new Rioter()
                {
                    Team = team,
                    Seed = UnityEngine.Random.Range(0, 100),
                    Position = new Vector3(team.GetArenaXStart(), 0f, _members.Count),
                    Velocity = team.GetDirection() * UnityEngine.Random.Range(0.5f, 1f) * 4,
                });
            }


            //for(var i = 0; i < 2; ++i)
            //{
            //    var team = (Team)i;
            //    _members.Add(new Rioter()
            //    {
            //        Team = team,
            //        Seed = UnityEngine.Random.Range(0, 100),
            //        Position = new Vector3(team.GetArenaXStart(), 0f, (int)UnityEngine.Random.Range(0, ArenaTransformer.Size.z)),
            //        Velocity = team.GetDirection() * UnityEngine.Random.Range(0.5f, 1f) * 100f,
            //    });
            //}
        }
    }

}
