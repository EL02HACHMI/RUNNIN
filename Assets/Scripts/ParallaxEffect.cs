using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    public Camera cam;
    public Transform followTarget;

    private Vector2 startingPosition;
    private float startingZ;
    private float parallaxFactor;

    private Vector2 CamMoveSinceStart => (Vector2)cam.transform.position - startingPosition;
    private float ZDistanceFromTarget => transform.position.z - followTarget.position.z;

    private float ClippingPlane => cam.transform.position.z + (ZDistanceFromTarget > 0 ? cam.farClipPlane : cam.nearClipPlane);

    private void Start()
    {
        startingPosition = transform.position;
        startingZ = transform.position.z;
    }

    private void Update()
    {
        float parallaxFactor = Mathf.Abs(ZDistanceFromTarget) / ClippingPlane;
        Vector2 newPosition = startingPosition + CamMoveSinceStart * parallaxFactor;
        transform.position = new Vector3(newPosition.x, newPosition.y, startingZ);
    }
}
