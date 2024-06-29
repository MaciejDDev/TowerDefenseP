namespace TerrainGeneration
{
    public class BuildableFloorTile : Tile
    {
        private bool _hasBuilding;
        public bool HasBuilding { get { return _hasBuilding; } set { _hasBuilding = value; } }
        internal override void Init()
        {
            _hasBuilding = false;
        }

    }


}
