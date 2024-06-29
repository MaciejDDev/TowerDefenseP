using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;


namespace TerrainGeneration
{
    public abstract class Tile : PoolableObject 
    {


        /*public void RecycleObject()
        {
            Recycle();
        }
        */

        internal override void Config(ObjectPool objectPool)
        {
            _objectPool = objectPool;
        }

        internal override void Init()
        {

        }
        internal override void Release()
        {

        }
    }
}
