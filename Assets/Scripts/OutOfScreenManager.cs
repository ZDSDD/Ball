using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OutOfScreenManager : MonoBehaviour
{
    public PlayerController playerController;
    public Renderer map;
    private Bounds mapBounds;
    private bool _variablesSet;

    void Start()
    {   
        if (playerController is null || map is null)
        {
            _variablesSet = false;
            Debug.Log("playerController is null || map is null");
            return;
        }

        _variablesSet = true;
        mapBounds = map.bounds;
        Debug.Log("Map bounds:\t" + mapBounds);
    }

    // Update is called once per frame
    void Update()
    {
        if (!_variablesSet) return;
        var playerPos = playerController.transform.position;
        if (!mapBounds.Contains(playerPos))
        {
            playerController.Reset();
        }
        
    }
}