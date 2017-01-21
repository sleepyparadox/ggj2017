using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class Arena : UnityObject
    {
        public bool HasWinner;
        public const float HitRadius = 10f;
        public const float HitRadiusSqr = 10f;

        public const int Depth = 75;
        public const int Width = 100;
        public const int Floor = 0;

        public static Arena S;

        public List<Rioter> Rioters;
        public EndGameText EndGame;
        ParticleSystem _particleSystem;
        Rioter[,] _grid;
        ScoringPole _pole;

        public Arena()
            : base(Assets.Spawn<GameObject>("Mob"))
        {
            S = this;

            Rioters = new List<Rioter>();
            _particleSystem = GameObject.GetComponent<ParticleSystem>();
            _grid = new Rioter[800, 600];
            _pole = new ScoringPole();
            _pole.WorldPosition = ArenaTransformer.ArenaToWorld(new Vector3(Width / 2f, 0, Depth / 2f), 0);

            u.Update += Update;
        }

        public void Score(Team team)
        {
            if (HasWinner)
                return;
            _pole.Score(team);
        }

        public void Win(Team team)
        {
            if (HasWinner)
                return;
            HasWinner = true;

            EndGame = new EndGameText(team);
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

                if (value != null 
                    && _grid[pos.x, pos.z] != null /*
                    && _grid[pos.x, pos.z] != value*/)
                    throw new Exception("sorry " + value + "; " + pos + " already used by " + _grid[pos.x, pos.z]);

                _grid[pos.x, pos.z] = value;
            }
        }

        void Update(UnityObject uObj)
        {
            Spawn();

            foreach (var member in Rioters.ToList())
                member.Update();

            var particles = Rioters.Select(p => p.ToParticle()).ToArray();

            _particleSystem.SetParticles(particles, particles.Length);
            _particleSystem.time = 0f;

            for (int x = 0; x < Width; x++)
            {
                for (int z = 0; z < Depth; z++)
                {
                    if (_grid[x, z] == null)
                        continue;
                }
            }
        }

        void Spawn()
        {
            for (var iTeam = 0; iTeam < 2; ++iTeam)
            {
                var team = (Team)iTeam;
                for (int i = 0; i < Arena.Depth; i++)
                {
                    var alreadyExists = false;
                    for (int x = 0; x < Width; x++)
                    {
                        if(_grid[x, i] != null && _grid[x, i].Team == team)
                        {
                            alreadyExists = true;
                            break;
                        }
                    }

                    var startX = team.GetArenaXStart();
                    if (alreadyExists || _grid[startX, i] != null)
                        continue;

                    var rioter = new Rioter()
                    {
                        Team = team,
                        Seed = UnityEngine.Random.Range(0, 100),
                    };
                    rioter.TryMoveTo(new Vec3(startX, 0, i), true);
                    Rioters.Add(rioter);
                }
            }
        }
    }

}
