﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class ScoringPole : UnityObject
    {
        public float ScoreNormalized = 0.5f;
        const float RotationSpeed = 60;
        const int PointsToWin = 10;
        int _score;
        float _anglePerPoint = 10f;

        public ScoringPole()
            : base (Assets.Spawn<GameObject>("ScoringPole"))
        {
            _anglePerPoint = 90f / PointsToWin;
            u.Update += Update;
        }

        public void Score(Team team)
        {
            if (team == Team.lower)
            {
                _score--;
                if (_score <= -PointsToWin)
                    Arena.S.Win(Team.lower);
            }

            if (team == Team.UPPER)
            {
                _score++;

                if (_score >= PointsToWin)
                    Arena.S.Win(Team.UPPER);
            }
        }

        void Update(UnityObject uObj)
        {
            var angle = _score * _anglePerPoint;
            angle = Mathf.Clamp(angle, -90f, 90f);

            var targetAngle =  Quaternion.Euler(0, 0, angle);

            LocalRotation = Quaternion.RotateTowards(LocalRotation, targetAngle, RotationSpeed * Time.deltaTime);
        }
    }
}
