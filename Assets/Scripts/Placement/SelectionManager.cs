using System.Collections;
using System.Collections.Generic;
using TerrainGeneration;
using UnityEngine;
using Utilities;

public class SelectionManager : MonoBehaviour
{
    [Header("Selection Tag")]
    [SerializeField] private string _selectableTag;

    [Header("Materials")]
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Material buildableMaterial;
    [SerializeField] private Material notBuildableMaterial;

    [Header("Placeable Factory Config")]
    [SerializeField] FactoryConfig _placeableFactoryConfig;

    [Header("Ray Properties")]
    [SerializeField] LayerMask _buildableLayer;
    [SerializeField] float _rayDistance;

    [SerializeField] GameObject _placementMenu;
    
    
    
    Camera _camera;
    Transform _selection;
    BuildableFloorTile _buildableFloor;

    Factory _placeablesFactory;
    GoldManager _goldManager;
    List<int> _placeablesCost = new List<int>();
    private bool _menuIsActive;


    public void Init()
    {
        _placeablesFactory = new Factory(_placeableFactoryConfig);
        _goldManager = GetComponent<GoldManager>();
        foreach (var placeable in _placeableFactoryConfig.Prefabs)
        {
            var turret = placeable as Placeable;
            _placeablesCost.Add(turret.Stats.Cost);
        }
        _camera = Camera.main;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_menuIsActive)
            return;
        //Deselect
        if (_selection != null)
        {
            var selection = _selection;
            var selectionRenderer = selection.GetComponentInChildren<Renderer>();
            if (selectionRenderer != null)
            {
                selectionRenderer.material = defaultMaterial;
            }
        }

        //Check Selection
        var ray = _camera.ScreenPointToRay(Input.mousePosition);

        _selection = null;

        if (Physics.Raycast(ray,out var hit, _rayDistance, _buildableLayer))
        {
            var selection = hit.transform;
            if (selection.CompareTag(_selectableTag))
            {
                _selection = selection;
            }
        }
        
        //Select
        if (_selection != null)
        {
            _buildableFloor = _selection.GetComponent<BuildableFloorTile>();
            var selectionRenderer = _selection.GetComponentInChildren<Renderer>();
            if (selectionRenderer != null && _buildableFloor != null)
            {
                //Material si se puede construir
                if (!_buildableFloor.HasBuilding)
                {
                    selectionRenderer.material = buildableMaterial;
                    //Si se hace click construye una torre
                    if (Input.GetMouseButtonDown(0))
                    {
                        if (CanBuildAnyTurret())
                        {
                            //Show Placement Menu
                            TogglePlacementMenu(true);
                        }

                        //Debug.Log("[SelectionManager.cs] : TurretSpawned.");
                        //PlaceBuilding(buildableFloor);
                    }
                }
                //Material si NO se puede construir
                else
                {
                    selectionRenderer.material = notBuildableMaterial;

                }


            }

        }
    }


    public void RecycleAllPlaceables()
    {
        _placeablesFactory.RecycleAllPools();
    }

    public void TogglePlacementMenu(bool visible)
    {
        _placementMenu.SetActive(visible);
        _menuIsActive = visible;
        
    }

    private bool CanBuildAnyTurret()
    {
        if (_placeablesCost.Count == 0)
            return false;

        foreach (var cost in _placeablesCost)
        {
            if (cost <= _goldManager.GetCurrentGold())
                return true;
        }

        return false;
    }

    public void HidePlacementMenu()
    {
        TogglePlacementMenu(false);
    }

    public void PlaceSmallTurret()
    {
        if (_goldManager.GetCurrentGold() < _placeablesCost[0])
        {
            //Show message
        }
        else
        {
            var smallTurret = _placeablesFactory.Create("SmallTurret", _selection.position, Quaternion.identity.eulerAngles) as Placeable;
            _buildableFloor.HasBuilding = true;
            smallTurret.SetTile(_buildableFloor);
            _goldManager.SpendGold(_placeablesCost[0]);
        }
        TogglePlacementMenu(false);
    }
    public void PlaceBigTurret()
    {
        if (_goldManager.GetCurrentGold() < _placeablesCost[1])
        {
            //Show message
        }
        else
        {
            var bigTurret = _placeablesFactory.Create("BigTurret", _selection.position, Quaternion.identity.eulerAngles) as Placeable;
            _buildableFloor.HasBuilding = true;
            bigTurret.SetTile(_buildableFloor);
            _goldManager.SpendGold(_placeablesCost[1]);
        }
        TogglePlacementMenu(false);
    }


}
