using Enemies;
using System;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
namespace AI
{
    public class AttackState : StateBase
    {
        public AttackState(Enemy agent) : base(agent) { }


        float _timer;

        public override void Enter()
        {
        }

        public override void Execute()
        {
            if (_timer > 0)
            {
                _timer -= Time.deltaTime;
            }
            if (_timer <= 0 )
            {
                _agent.SetTowerTarget();
                if (_agent.Target != null)
                    AttackBase();
                else
                    _agent.ChangeState(new RunState(_agent));

            }


        }

        private void AttackBase()
        {

            //Debug.Log("[AttackState.cs] : Attacking tower.");
            _agent.BulletShooter.Shoot(_agent.Target as ITakeDamage, _agent.EnemyStats.AttackDamage);
            _timer += _agent.EnemyStats.AttackCooldown;
        }

        public override void Exit()
        {
            //throw new System.NotImplementedException();
        }
    }
}