using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class cameraFollowObject : MonoBehaviour
{
    private Transform _playerTransform;
    [SerializeField] private float _flipYRotationTime = 0.5f;

    private Coroutine _turnCoroutine;

    private playerInputs _playerInputs;

    private bool _isFacingRight;

    private void Awake()
    {
        NewObjectToFollow(transform);
    }

    public void NewObjectToFollow(Transform ToFollow)
    {
        _playerTransform = ToFollow;
        _playerInputs = _playerTransform.gameObject.GetComponent<playerInputs>();
        if(_playerInputs != null)
            _isFacingRight = _playerInputs.IsFacingRight;
        else
            _isFacingRight = false;
    }



    void Update()
    {
        transform.position = _playerTransform.position;
    }

    public void CallTurn()
    {
        //_turnCoroutine = StartCoroutine(FlipYLerp());

        LeanTween.rotateY(gameObject, DetermineEndRotation(), _flipYRotationTime).setEaseInOutSine();

    }

    private IEnumerator FlipYLerp()
    {
        float startRotation = transform.localEulerAngles.y;
        float endRotationAmount = DetermineEndRotation();
        float yRotation = 0f;

        float elapsedTime = 0f;
        while (elapsedTime < _flipYRotationTime)
        {
            elapsedTime += Time.deltaTime;

            yRotation = Mathf.Lerp(startRotation, endRotationAmount, (elapsedTime/_flipYRotationTime));
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

            yield return null;
        }
    }

    private float DetermineEndRotation()
    {
        _isFacingRight = !_isFacingRight;

        if (!_isFacingRight)
            return 180f;
        else return 0f; 
    }

}
