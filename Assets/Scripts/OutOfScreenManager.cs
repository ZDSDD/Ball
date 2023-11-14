using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OutOfScreenManager : MonoBehaviour
{
    public PlayerController playerController;
    public Renderer map;
    private Bounds _mapBounds;
    private bool _variablesSet = false;

    void Start()
    {
        /* If Manager wasn't properly set by the developer, try to find player and map in the scene */
        if (playerController is null)
        {
            playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        }
        //double-check because raw null is "NOT" enough :)
        if (map is null || map.IsUnityNull())
        {
            map =GameObject.FindWithTag("Map").GetComponent<SpriteRenderer>();
        }
        if (playerController && map)
        {
            _variablesSet = true;
            _mapBounds = map.bounds;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!_variablesSet) return;
        var playerPos = playerController.transform.position;
        if (!_mapBounds.Contains(playerPos))
        {
            playerController.Reset();
        }
    }
}