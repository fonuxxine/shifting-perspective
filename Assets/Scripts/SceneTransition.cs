using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string sceneName; // name of scene to transition to
    public float interactionRange = 2f; // range for interaction

    private GameObject _player; // reference to player object
    private GameObject _enterKeyImage; // ENTER KEY image GameObject
    private bool _hasEnterPrompt = false; // whether caller has dialogue prepared
    private bool _isInRange = false; // whether player is in range

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");

        // hide ENTER KEY image to start (if it exists)
        Transform enterKeyImageTransform = transform.Find("EnterKeyCanvas");
        _hasEnterPrompt = enterKeyImageTransform != null;
        if (_hasEnterPrompt)
        {
            _enterKeyImage = enterKeyImageTransform.gameObject;
            _enterKeyImage.SetActive(false);
        }
    }

    private void Update()
    {
        if (!_player) return;

        // check if player is within interaction range
        _isInRange = Vector3.Distance(transform.position, _player.transform.position) <= interactionRange;

        // show ENTER KEY image if player is in range
        SetEnterKeyImageActive(_isInRange);

        // make the Enter Key image face the camera
        if (_hasEnterPrompt && _isInRange)
        {
            _enterKeyImage.transform.LookAt(_enterKeyImage.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
        }

        // check if player is in range and presses Enter key, if so load scene
        if (_isInRange && Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    private void SetEnterKeyImageActive(bool isActive)
    {
        // show or hide ENTER KEY image based on input
        if (_hasEnterPrompt)
        {
            _enterKeyImage.SetActive(isActive);
        }
    }
}
