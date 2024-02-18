using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCharacter : MonoBehaviour
{
    private GameObject _player;
    private CharacterController _characterController;
    private Transform _playerTransform;

    private Vector3 _charSpawnLocation = new Vector3(0.7f, 2.23f, 0.22f);
    
    public const int NumOfLives = 3;
    
    public int PlayerLivesLeft;
    
    private const float RespawnHeight = -10f;
    
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Instantiate(Resources.Load("Player"), 
            _charSpawnLocation, Quaternion.identity) as GameObject;
        _playerTransform = _player.transform;
        _characterController = GetComponent<CharacterController>();
        PlayerLivesLeft = NumOfLives;
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerTransform.position.y < RespawnHeight)
        {
            if (PlayerLivesLeft > 0)
            {
                // _playerTransform.position = _charSpawnLocation;
                // _characterController.position = new Vector3(0, 0, 0);
                // PlayerLivesLeft -= 1;
                // UpdatePlayerInfo.UpdatePlayerLivesText(PlayerLivesLeft);
            }
            else
            {
                // EndGame();
                // UpdatePlayerInfo.DisplayGameOver();
            }
        }
    }
}
