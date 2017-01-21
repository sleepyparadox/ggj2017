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

        public static Arena S;

        public List<Rioter> Rioters;
        ParticleSystem _particleSystem;
        Rioter[,] _grid;

        public Arena()
            : base(Assets.Spawn<GameObject>("Mob"))
        {
            S = this;

            Rioters = new List<Rioter>();
            _particleSystem = GameObject.GetComponent<ParticleSystem>();
            _grid = new Rioter[800, 600];

            Spawn();

            u.Update += Update;
        }

        public Rioter this[Vec3 pos]
        {
            get
            {
                if (pos.x < 0 || pos.x >= Width
                    || pos.z < 0 || pos.z >= Depth)
                    return null;

                return _grid[pos.x, pos.z];
            }
            set
            {
                if (pos.x < 0 || pos.x >= Width
                    || pos.z < 0 || pos.z >= Depth)
                    return;

                _grid[pos.x, pos.z] = value;
            }
        }

        void Update(UnityObject uObj)
        {
            //CheckForCollides();

            foreach (var member in Rioters)
                member.Update();

            var particles = Rioters.Select(p => p.ToParticle()).ToArray();

            _particleSystem.SetParticles(particles, particles.Length);
            _particleSystem.time = 0f;
        }

        //void CheckForCollides()
        //{
        //    foreach(var leftMember in _members.Where(m => m.Team == Team.lower))
        //    {
        //        var rightMember = _members.FirstOrDefault(m => m.Team == Team.UPPER 
        //                                                 && m.Velocity.y == 0
        //                                                 && m.LerpyPosition.y == 0
        //                                                 && (m.LerpyPosition - leftMember.LerpyPosition).sqrMagnitude < HitRadiusSqr);

        //        if(rightMember != null)
        //        {
        //            leftMember.Velocity += Vector3.up * 20f;
        //            rightMember.Velocity += Vector3.up * 20f;
        //        }
        //    }

        //}

        void Spawn()
        {
            for (var iTeam = 0; iTeam < 2; ++iTeam)
            {
                var team = (Team)iTeam;
                for (int i = 0; i < Arena.Depth; i++)
                {
                    var rioter = new Rioter()
                    {
                        Team = team,
                        Seed = UnityEngine.Random.Range(0, 100),
                    };
                    rioter.TryMoveTo(new Vec3(team.GetArenaXStart(), 0, i), true);
                    Rioters.Add(rioter);
                }
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
