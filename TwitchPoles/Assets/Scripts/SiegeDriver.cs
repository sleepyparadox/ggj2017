using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts
{
    public class SiegeDriver : Rioter
    {
        const int CellsPerPush = 4;
        int _length = 12;
        SeigeWord _word;
        Vector3 _wordOffset;
        Vector3 _quadOffset;
        GameObject _quad;

        public SiegeDriver(SeigeWord word)
        {
            _word = word;

            Size = 16f;
            Rotation = 45f;
            Team = word.Team;

            var bounds = word.GameObject.GetComponent<Renderer>().bounds;

            var xBound = Team == Team.lower ? bounds.min.x : bounds.max.x;
            var spawnPos = new Vec3((int)(xBound / 8f), 0,(int)(bounds.min.y / 8));
            spawnPos.x += Team.GetDirection().x * -4;

            TryMoveTo(spawnPos, true);

            var rioterWorldPos = ArenaTransformer.ArenaToWorld(LerpyPosition, 0);

            _wordOffset = _word.WorldPosition - rioterWorldPos;

            _quad = GameObject.CreatePrimitive(PrimitiveType.Quad);
            _quad.transform.localScale = new Vector3(_length * 8f, 8f, 1f);

            _quadOffset = new Vector3((_length * 4f) + 4, 0, 10);
            _quadOffset.x *= Team.GetDirection().x;
            _quad.transform.position = rioterWorldPos + _quadOffset;

            foreach (var cell in CellsInWord())
            {
                if (Arena.S[cell] != null)
                {
                    if (Arena.S[cell] == this)
                        continue;
                    else
                        Arena.S[cell].Die();
                }
                Arena.S[cell] = this;
            }
        }

        public override void Update()
        {
            if (Cell.x >= Arena.Width || Cell.x < 0)
            {
                Die();
                return;
            }

            if (Team == Team.lower)
                _word.SetCaptialization(Cell.x < (Arena.Width * 0.3) ? Team.UPPER : Team.lower);

            if (Team == Team.UPPER)
                _word.SetCaptialization(Cell.x > (Arena.Width * 0.7) ? Team.lower : Team.UPPER);

            var rioterWorldPos = ArenaTransformer.ArenaToWorld(LerpyPosition, 0);
            _word.WorldPosition = rioterWorldPos + _wordOffset;
            _quad.transform.position = rioterWorldPos + _quadOffset;

            {
                // exit hack
                LerpyPosition = Vector3.MoveTowards(LerpyPosition, Cell.ToVector3(), Speed * Time.deltaTime);
                return;
            }

            var nextCell = GetNextCell();
            if (Arena.S[nextCell] != null )
            {
                Debug.Log(this + " stuck " + nextCell + " is occupied by " + Arena.S[nextCell]);
                LerpyPosition = Vector3.MoveTowards(LerpyPosition, Cell.ToVector3(), Speed * Time.deltaTime);
                return;
            }
            else
            {
                Debug.Log(this + " classic update");
                foreach(var cell in CellsInWord())
                {
                    Arena.S[cell] = null;
                }

                base.Update();

                foreach (var cell in CellsInWord())
                {
                    Arena.S[cell] = this;
                }
            }
        }

        public void Use(Team team)
        {
            Debug.Log("use " + this);
            Push(team, 4);
        }

        void Push(Team team, int amount)
        {
            var dir = team.GetDirection();
            for (int i = 0; i < amount; i++)
            {
                var cells = team == Team ? CellsInWord().Reverse() : CellsInWord();
                foreach (var cell in cells)
                {
                    //Debug.Log("pushing cells; " + cell);
                    Push(this, cell, cell + dir, dir.x);
                }

                Cell += dir;
            }

        }

        static void Push(Rioter rioter, Vec3 from, Vec3 to, int direction, string depthPrefix = "")
        {
            if (Arena.S[to] != null)
            {
                var neighbourTo = to;
                neighbourTo.x += direction;
                Push(Arena.S[to], to, neighbourTo, direction, "  " + depthPrefix);

                if (Arena.S[to] != null)
                    throw new Exception("Failed to clear " + to);
            }

            if (Arena.S[from] == rioter)
                Arena.S[from] = null;

            Arena.S[to] = rioter;

            if(!(rioter is SiegeDriver))
                rioter.Cell = to;
        }

        Vec3 GetNextCell()
        {
            var cell = Cell;
            var direction = Team == Team.lower ? 1 : -1;
            cell.x += direction * _length;
            cell.x += direction;
            return cell;
        }

        IEnumerable<Vec3> CellsInWord()
        {
            var cell = Cell;
            var direction = Team == Team.lower ? 1 : -1;
            for (int i = 0; i < _length + 1; i++)
            {
                yield return cell;
                cell.x += direction;
            }
        }

        public override void Die()
        {
            _word.Dispose();
            GameObject.Destroy(_quad);

            foreach (var cell in CellsInWord())
            {
                if (Arena.S[cell] == this)
                    Arena.S[cell] = null;
            }
           
            base.Die();
        }

        public override string ToString()
        {
            return string.Concat(_word.Key, " at ", Cell);
        }
    }
}
