using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class Rioter
    {
        protected const float Speed = 100f;//0.5f;
        protected const float ActAfter = 0.5f;
        protected const int ChanceOfPush = 20;
        protected float Size = 8f;
        protected float Rotation = 0f;
        public Team Team;
        public Vec3 Cell;
        public Vector3 LerpyPosition;
        public float Seed;

        float _willAct = 0f;

        public Rioter()
        {
            _willAct -= UnityEngine.Random.Range(0, ActAfter);
            Seed = UnityEngine.Random.Range(-1f, 1f);
        }

        public virtual void Update()
        {
            _willAct += Time.deltaTime;
            if (_willAct >= 0)
            {
                _willAct -= UnityEngine.Random.Range(0, ActAfter);

                var moved = TryMoveTo(Cell + Team.GetDirection());
                
                if(!moved 
                    /*&& UnityEngine.Random.Range(0, 100) < ChanceOfPush*/)
                {
                    var lineAt = Arena.S.GetScoreNormalized(Team);
                    lineAt += Seed * 0.1f;

                    lineAt = (int)(lineAt * Arena.Width);

                    if ((Team == Team.lower && Cell.x  < lineAt)
                        || (Team == Team.UPPER && Cell.x < lineAt))
                    {
                        //Debug.Log(Team + " pushing to " + Cell + Team.GetDirection() + " cell " + Cell.x + " lineAt " + lineAt);
                        Push(this, Cell, Cell + Team.GetDirection(), Team.GetDirection().x);
                    }
                }
            }

            LerpyPosition = Vector3.MoveTowards(LerpyPosition, Cell.ToVector3(), Speed * Time.deltaTime);

            if (Cell.x >= Arena.Width || Cell.x < 0)
                Die();
        }

        public bool TryMoveTo(Vec3 pos, bool updateMesh = false)
        {
            if (Arena.S[pos] != null && Arena.S[pos] != this)
                return false; //occupied

            // clear old spot
            if (Arena.S[Cell] == this)
                Arena.S[Cell] = null;

            // move
            Cell = pos;
            Arena.S[Cell] = this;

            if (updateMesh)
                LerpyPosition = Cell.ToVector3();

            return true;
        }

        public ParticleSystem.Particle ToParticle()
        {
            return new ParticleSystem.Particle()
            {
                color = Team == Team.lower ? Color.red : Color.blue,
                position = ArenaTransformer.ArenaToWorld(LerpyPosition, Seed),
                size = Size,
                lifetime = 1000,
                startLifetime = 500,
                rotation = Rotation,
            };
        }

        public virtual void Die()
        {
            Arena.S.Rioters.Remove(this);
            if (Arena.S[Cell] == this)
                Arena.S[Cell] = null;
        }

        protected static void Push(Rioter rioter, Vec3 from, Vec3 to, int direction)
        {
            if (Arena.S[to] != null)
            {
                var neighbourTo = to;
                neighbourTo.x += direction;
                Push(Arena.S[to], to, neighbourTo, direction);

                if (Arena.S[to] != null)
                    throw new Exception("Failed to clear " + to);
            }

            if (Arena.S[from] == rioter)
                Arena.S[from] = null;

            Arena.S[to] = rioter;

            if (!(rioter is SiegeDriver))
                rioter.Cell = to;
        }

        public override string ToString()
        {
            return "rioter at " + Cell;
        }
    }

}
