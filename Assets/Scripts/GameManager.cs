using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerController player;

    [SerializeField] private GravityManager gravityManager;
    [SerializeField] private MapGenerator mapGenerator;

    private void Start()
    {
        Initialize();
    }
    private void Initialize()
    {
        mapGenerator.SetupCircle();
        gravityManager.SubscribeToGravity(player.Rigidbody2D);
        gravityManager.StartGravity();
        player.Initilize(mapGenerator.GetWalls(), gravityManager);
    }
}