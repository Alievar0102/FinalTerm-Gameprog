using UnityEngine;
using Unity.Cinemachine;

public class CameraController : MonoBehaviour
{
    CinemachineOrbitalFollow orbitalFollow;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        orbitalFollow = GetComponent<CinemachineOrbitalFollow>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CameraChangeRotation(float rotation) 
    {
        orbitalFollow.HorizontalAxis.Value = rotation;
    }
}
