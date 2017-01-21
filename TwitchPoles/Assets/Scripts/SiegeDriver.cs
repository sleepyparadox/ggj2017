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
        int _length = 10;
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

            if (Arena.S[spawnPos] != null)
                (Arena.S[spawnPos] as Rioter).Die();

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
                    (Arena.S[cell] as Rioter).Die();
                Arena.S[cell] = this;
            }
        }

        public override void Update()
        {
            var rioterWorldPos = ArenaTransformer.ArenaToWorld(LerpyPosition, 0);
            _word.WorldPosition = rioterWorldPos + _wordOffset;
            _quad.transform.position = rioterWorldPos + _quadOffset;

            var nextCell = GetNextCell();
            if (Arena.S[nextCell] != null 
                && Arena.S[nextCell] != this
                && CellsInWord().All(c => Arena.S[c] == null || Arena.S[c] == this )
                )
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

        }

        IEnumerable<Vec3> CellsInWord()
        {
            var cell = Cell;
            var direction = Team == Team.lower ? 1 : -1;
            for (int i = 0; i < _length; i++)
            {
                cell.x += direction;
                yield return cell;
            }
        }

        Vec3 GetNextCell()
        {
            var cell = Cell;
            var direction = Team == Team.lower ? 1 : -1;
            cell.x += direction * _length;
            cell.x += direction;
            return cell;
        }

        public override void Die()
        {
            _word.Dispose();
            GameObject.Destroy(_quad);
            base.Die();
        }

        public override string ToString()
        {
            return string.Concat(_word.Key, " ", Cell);
        }
    }
}
