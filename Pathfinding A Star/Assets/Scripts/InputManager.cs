using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Camera _camera;
    private int _layerMaskTile;
    private const string _layerNameTile = "Tile";

    //==========================================================================

    private void Start()
    {
        _camera = Camera.main;
        _layerMaskTile = 1 << LayerMask.NameToLayer(_layerNameTile);
    }

    //==========================================================================

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 20f, _layerMaskTile))
            {
                Tile tile = hit.collider.GetComponent<Tile>();

                if(tile != null)
                {
                    EventsManager.Instance.ClickedTile(tile);
                }
            }
        }
    }

    //==========================================================================

}
