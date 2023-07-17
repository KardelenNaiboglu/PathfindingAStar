using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class InputManager : MonoBehaviour
{
    private Camera _camera;
    private int _layerMaskTile;
    private const string _layerNameTile = "Tile";

    private GraphicRaycaster _raycaster;
    private PointerEventData _pointerEventData;
    private EventSystem _eventSystem;
    private RaycastHit _hit;
    //==========================================================================

    private void Start()
    {
        _camera = Camera.main;
        _layerMaskTile = 1 << LayerMask.NameToLayer(_layerNameTile);
        _raycaster = FindObjectOfType<GraphicRaycaster>();
        _eventSystem = FindObjectOfType<EventSystem>();
    }

    //==========================================================================

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            _pointerEventData = new PointerEventData(_eventSystem);
            _pointerEventData.position = Input.mousePosition;

            List<RaycastResult> results = new();

            _raycaster.Raycast(_pointerEventData, results);

            if(results.Count > 0)
            {
                return;
            }

            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out _hit, 20f, _layerMaskTile))
            {
                Tile tile = _hit.collider.GetComponent<Tile>();

                if(tile != null)
                {
                    EventsManager.Instance.ClickedTile(tile);
                }
            }
        }
    }

    //==========================================================================

}
