using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class Rioter
    {
        const float Speed = 100f;//0.5f;
        const float ActAfter = 0.5f;
        public Team Team;
        public Vec3 ArenaCell;
        public Vector3 LerpyPosition;
        public float Seed;

        float _willAct = 0f;

        public Rioter()
        {
            _willAct -= UnityEngine.Random.Range(0, ActAfter);
        }

        public void Update()
        {
            _willAct += Time.deltaTime;
            if (_willAct >= 0)
            {
                _willAct -= UnityEngine.Random.Range(0, ActAfter);

                TryMoveTo(ArenaCell + Team.GetDirection());
            }

            LerpyPosition = Vector3.MoveTowards(LerpyPosition, ArenaCell.ToVector3(), Speed * Time.deltaTime);
        }

        public bool TryMoveTo(Vec3 pos, bool updateMesh = false)
        {
            if (Arena.S.Grid[pos.x, pos.z] != null)
                return false; //occupied

            // clear old spot
            if (Arena.S.Grid[ArenaCell.x, ArenaCell.z] == this)
                Arena.S.Grid[ArenaCell.x, ArenaCell.z] = null;

            // move
            ArenaCell = pos;
            Arena.S.Grid[ArenaCell.x, ArenaCell.z] = this;

            if (updateMesh)
                LerpyPosition = ArenaCell.ToVector3();

            return true;
        }

        public ParticleSystem.Particle ToParticle()
        {
            return new ParticleSystem.Particle()
            {
                color = Team == Team.lower ? Color.red : Color.blue,
                position = ArenaTransformer.ArenaToWorld(LerpyPosition, Seed),
                size = 8f,
                lifetime = 1000,
                startLifetime = 500,
            };
        }
    }

}
