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
        }

        public virtual void Update()
        {
            _willAct += Time.deltaTime;
            if (_willAct >= 0)
            {
                _willAct -= UnityEngine.Random.Range(0, ActAfter);

                TryMoveTo(Cell + Team.GetDirection());
            }

            LerpyPosition = Vector3.MoveTowards(LerpyPosition, Cell.ToVector3(), Speed * Time.deltaTime);
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

        public override string ToString()
        {
            return "rioter at " + Cell;
        }
    }

}
