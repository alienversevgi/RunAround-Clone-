using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerController player;
    [SerializeField] private GravityManager gravityManager;
    [SerializeField] private MapGenerator mapGenerator;
    [SerializeField] private UIManager uiManager;

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
        player.ProgressIncreased += Player_ProgressIncreased;
    }

    private void Player_ProgressIncreased()
    {
        uiManager.SetPercentageText(GetLevelPercentage());
    }

    private sbyte GetLevelPercentage()
    {
        int activatedCount = mapGenerator.GetWalls().Count(it => it.IsActivated);
        return GetPercentage(activatedCount, mapGenerator.GetSegmentCount());
    }

    private sbyte GetPercentage(float current, float max)
    {
        return (sbyte)((current / max) * 100); ;
    }
}
