using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParllaxBackground : MonoBehaviour
{
    [SerializeField] float parallaxEffectMultiplier;
    private Transform _cameraTransfrom;
    private Vector3 _lastCameraPosition;
    private float textureUnitSizeX;

     private void Start()
    {
        _cameraTransfrom = Camera.main.transform;
        _lastCameraPosition = _cameraTransfrom.position;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
    }

    private void LateUpdate()
    {
        Vector3 deltaMovement = _cameraTransfrom.position - _lastCameraPosition;
        transform.position += deltaMovement * parallaxEffectMultiplier;
        _lastCameraPosition = _cameraTransfrom.position ;

        if (Mathf.Abs(_cameraTransfrom.position.x - transform.position.x) >= textureUnitSizeX)
        {
            float offsetPositionX = (_cameraTransfrom.position.x - transform.position.x) % textureUnitSizeX;
            transform.position = new Vector3(_cameraTransfrom.position.x + offsetPositionX, transform.position.y);
        }
    }


}
