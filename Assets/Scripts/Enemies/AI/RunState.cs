using Enemies;
using System;
using UnityEngine;

namespace AI
{
    public class RunState : StateBase
    {

        
        public RunState(Enemy agent) : base(agent) { }


        public override void Enter()
        {
            LookAtNextIndex();
        }

        public override void Execute()
        {
            if (IsTargetInRange())
            {
                _agent.ChangeState(new AttackState(_agent));
                return;
            }


            if (Vector3.Distance(_agent.transform.position, _agent.Path[_agent.CurrentPathIndex]) < 1.5f)
            {
                //if (_agent.CurrentPathIndex == _agent.Path.Count - 1 )
                _agent.UpdatePathIndex();
                LookAtNextIndex();
            }
            
            var moveDir = _agent.Path[_agent.CurrentPathIndex] - _agent.transform.position;
            var moveDistance = moveDir.normalized * Time.deltaTime * _agent.EnemyStats.MovementSpeed;
            _agent.transform.position += moveDistance;




        }

        private void LookAtNextIndex()
        {
            //Vector3 desiredLookingDirection = _agent.Path[_agent.CurrentPathIndex] - _agent.transform.position;
            //float angle = Vector3.Angle(desiredLookingDirection.normalized, _agent.transform.forward);
            //_agent.transform.Rotate(Vector3.up * angle);
            _agent.transform.LookAt(_agent.Path[_agent.CurrentPathIndex]);
        }

        private bool IsTargetInRange()
        {
            bool inRange = false;
            _agent.SetTowerTarget();
            if (_agent.Target != null)
                inRange = true;
            return inRange;
        }

        public override void Exit()
        {
        }
    }
}