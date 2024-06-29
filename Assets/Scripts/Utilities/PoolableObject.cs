using UnityEngine;

namespace Utilities
{
    public abstract class PoolableObject : MonoBehaviour
    {
        [SerializeField] string _id;

        public string Id => _id;
        protected ObjectPool _objectPool;

        //Lo llamamos al crear el objeto
        internal abstract void Config(ObjectPool objectPool);

        public void Open()
        {

        }
        public void Recycle()
        {
            _objectPool.RecycleGameObject(this);
        }

        //Lo llamamos cada vez que se activa el objeto
        internal abstract void Init();
        internal abstract void Release();

    }
}