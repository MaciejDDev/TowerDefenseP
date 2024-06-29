using Enemies;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    [Serializable]
    public abstract class StateBase //: MonoBehaviour
    {
        protected Enemy _agent;

        public StateBase(Enemy agent) => _agent = agent;

        public abstract void Enter();
        public abstract void Execute();
        public abstract void Exit();
    }
}