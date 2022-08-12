using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class BallController : MonoBehaviour
{
    [SerializeField] private GameObject _ballPrefab;
    [SerializeField] private TMP_Text _ballCountText = null;
    [SerializeField] private List<GameObject> _balls = new List<GameObject>();
    [SerializeField] private float _horizontalSpeed;
    [SerializeField] private float _horizontalLimit;
    [SerializeField] private float _moveSpeed;
    private float _horizontal;
    private int _gateNumber;
    private int _targetCount;
    private Scene _scene;
    void Awake()
    {
        _scene = SceneManager.GetActiveScene(); //caching
    }

    void Update()
    {
        HorizontalBallMove();
        ForwardBallMove();
        UpdateBallCountText();

    }

    private void HorizontalBallMove()
    {
        float _newX = 0;

        if (Input.GetMouseButton(0))
        {
            _horizontal = Input.GetAxisRaw("Mouse X");
        }
        else
        {
            _horizontal = 0;
        }

        _newX = transform.position.x + _horizontal * _horizontalSpeed * Time.deltaTime;
        _newX = Mathf.Clamp(_newX, -_horizontalLimit, _horizontalLimit);

        transform.position = new Vector3(
        _newX,
        transform.position.y,
        transform.position.z
        );
    }

    private void ForwardBallMove()
    {
        transform.Translate(Vector3.forward * _moveSpeed * Time.deltaTime);
    }

    private void UpdateBallCountText()
    {
        _ballCountText.text = _balls.Count.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("BallStack"))
        {
            other.gameObject.transform.SetParent(transform);
            other.gameObject.GetComponent<SphereCollider>().enabled = false;
            other.gameObject.transform.localPosition = new Vector3(0f, 0f, _balls[_balls.Count - 1].transform.localPosition.z - 1f);
            _balls.Add(other.gameObject);
        }

        if (other.gameObject.CompareTag("Gate"))
        {
            _gateNumber = other.gameObject.GetComponent<GateController>().GetGateNumber();
            _targetCount = _balls.Count + _gateNumber;

            if (_gateNumber > 0)
            {
                IncreaseBallCount();

            }else if(_gateNumber < 0) {

                DecreaseBallCount();
            }
        }

        if (other.gameObject.CompareTag("NextLevel"))
        {
            SceneManager.LoadScene(_scene.buildIndex + 1);
        }
    }
    public void StartLevel()
    {
        SceneManager.LoadScene(_scene.buildIndex + 1);
    }

    private void IncreaseBallCount()
    {
        for(int i =0; i < _gateNumber; i++)
        {
            GameObject _newBall = Instantiate(_ballPrefab);
            _newBall.transform.SetParent(transform);
            _newBall.GetComponent<SphereCollider>().enabled = false;
            _newBall.transform.localPosition = new Vector3(0f, 0f, _balls[_balls.Count - 1].transform.localPosition.z - 1f);
            _balls.Add(_newBall);
        }
    }

    private void DecreaseBallCount()
    {
        for (int i = _balls.Count - 1; i >= _targetCount; i--)
        {
            _balls[i].SetActive(false);
            _balls.RemoveAt(i);

            if (_balls.Count <= 0)
            {
                SceneManager.LoadScene("DeadScene");
            }

        }

        
    }
}
