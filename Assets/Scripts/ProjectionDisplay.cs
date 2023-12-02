using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProjectionDisplay : MonoBehaviour
{
    private Scene _simulationScene;
    private PhysicsScene2D _physicScene;
    [SerializeField] private Transform _obstaclesCollection;
    // Start is called before the first frame update
    void Start()
    {
        CreatePhysicScene();
    }

    void CreatePhysicScene()
    {
        //Physic Scene preparation
        _simulationScene = SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics2D));
        _physicScene = _simulationScene.GetPhysicsScene2D();

        //Adding all obstacles to the physic scene
        foreach(Transform parent in _obstaclesCollection) 
        {
            if (parent.tag == "CheckPointList")
            {
                continue;
            }

            for(int i = 0; i < parent.childCount; i++)
            {
                var obj = parent.GetChild(i);
                var ghostObj = Instantiate(obj.gameObject, obj.position, obj.rotation);
                ghostObj.GetComponent<Renderer>().enabled = false;
                SceneManager.MoveGameObjectToScene(ghostObj, _simulationScene);
            }
        }

    }

    [SerializeField] private LineRenderer _linePrefab;
    [SerializeField] private int _maxFrameIterations = 100;


    public void SimulateTrajectory(PlayerController playerRef, Vector2 dragDistance)
    {
        var ghostObj = Instantiate(playerRef.gameObject, playerRef.transform.position, Quaternion.identity);
        ghostObj.GetComponent<Renderer>().enabled = false;
        SceneManager.MoveGameObjectToScene(ghostObj, _simulationScene);

        Vector2 launchDirection = dragDistance * playerRef.launchPower;
        Rigidbody2D _rb = ghostObj.GetComponent<Rigidbody2D>();

        _rb.AddForce(launchDirection, ForceMode2D.Impulse);

        _rb.gravityScale = 1f;

        _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, playerRef.maxSpeed);

        _linePrefab.positionCount = _maxFrameIterations;

        for(int i = 0; i < _maxFrameIterations; i++)
        {
            _physicScene.Simulate(Time.fixedDeltaTime);
            _linePrefab.SetPosition(i, ghostObj.transform.position);
        }

        Destroy(ghostObj.gameObject);
    }

    public void ResetDisplay()
    {
        _linePrefab.positionCount = 0;
    }
}