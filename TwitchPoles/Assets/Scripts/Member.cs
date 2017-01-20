using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class Member
    {
        public Team Team;
        public Vector3 Position;
        public Vector3 Velocity;
        public float Seed;

        public void Update()
        {
            Position += Velocity * Time.deltaTime;

            if (Position.y < 0)
                Position.y = 0;

            //if (Velocity.y != 0f)
            //{
            //    // bouncing

            //    Velocity.y -= 10f * Time.deltaTime;

            //    if (Mathf.Abs(Position.y) + Mathf.Abs(Velocity.y) < 10f)
            //        Velocity.y = 0;
            //    else if (Position.y < 0)
            //        Velocity.y = Mathf.Abs(Velocity.y);
            //}
        }

        public ParticleSystem.Particle ToParticle()
        {
            return new ParticleSystem.Particle()
            {
                color = Color.white,
                position = World.ArenaToWorld(Position, Seed),
                size = 100f,
                lifetime = 1000,
                startLifetime = 500,
            };
        }
    }

}
