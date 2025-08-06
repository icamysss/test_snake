using UnityEngine;

public class ListenerSetter : MonoBehaviour
{
	private Camera _camera;
	private GameManager _gameManager;
	private void Start()
	{
		_camera = Camera.main;
		_gameManager = GameManager.Instance;
	}

	private void Update()
	{
		if (_gameManager == null) _gameManager = GameManager.Instance;
		if (_camera == null) _camera = Camera.main;
		
		if (_gameManager !=null && _gameManager.LevelManager && _gameManager.LevelManager.Player)
		{
			transform.position = _gameManager.LevelManager.Player.transform.position;
		}
		transform.rotation = _camera.transform.rotation;
	}
}
