using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class Rioter
    {
        public Team Team;
        public Vec3 TargetCell;
        public Vector3 Position;
        public Vector3 Velocity;
        public float Seed;

        public void Update()
        {
            Position += Velocity * Time.deltaTime;

            if (Position.y < 0)
                Position.y = 0;

            if (Position.x >= Arena.Width)
                Position.x = 0;

            if (Position.x < 0)
                Position.x = Arena.Width - 1;

            if (Velocity.y != 0f)
            {
                // bouncing
                Velocity.y -= 50f * Time.deltaTime;

                if (Mathf.Abs(Position.y) + Mathf.Abs(Velocity.y) < 10f)
                {
                    // stop
                    Velocity.y = 0;
                    Position.y = 0;
                }
                else if (Position.y < 0 && Velocity.y < 0)
                {
                    // bounce
                    Velocity.y *= -0.9f;
                    Position.y = 0f;
                }
            }
        }

        public ParticleSystem.Particle ToParticle()
        {
            return new ParticleSystem.Particle()
            {
                color = Team == Team.lower ? Color.red : Color.blue,
                position = ArenaTransformer.ArenaToWorld(Position, Seed),
                size = 8f,
                lifetime = 1000,
                startLifetime = 500,
            };
        }
    }

}
