using System;
using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;

public class CameraController : MonoBehaviour
{

    // List of players, possibly allow more than 2?
    [SerializeField] private GameObject Player1;
    [SerializeField] private GameObject Player2;

    // Camera fields
    private Camera _mainCamera;
    [SerializeField] private float MinZoom;
    [SerializeField] private float MaxZoom;
    [SerializeField] private float ZoomCoefficient; // Used to scale down zoom, remove later

    // Player positions
    private Vector3 _pos1, _pos2;

    // Use this for initialization
    void Start ()
    {
        _mainCamera = Camera.main;

        _pos1 = Player1.transform.position;
        _pos2 = Player2.transform.position;
    }
	
	// Update is called once per frame
	void Update ()
    {
        // Move camera to focus on average distance between the two players
        _pos1 = Player1.transform.position;
        _pos2 = Player2.transform.position;
        _mainCamera.transform.position = new Vector3((_pos1.x + _pos2.x)/2.0f, (_pos1.y + _pos2.y)/2.0f, Camera.main.transform.position.z);

        // Change zoom based on player separation
	    float zoom = Vector3.Distance(_pos1, _pos2)*ZoomCoefficient;
        if (zoom >= MinZoom && zoom <= MaxZoom)
	        _mainCamera.orthographicSize = zoom;
        else if (zoom < MinZoom)
            _mainCamera.orthographicSize = MinZoom;
        else if (zoom > MaxZoom)
            _mainCamera.orthographicSize = MaxZoom;
	}

    // Inverse square root algorithm
    private static double Sqrt(double value)
    {
        if (Math.Abs(value) < 0.001)
            return 0;

        FloatIntUnion union;
        union.tmp = 0;
        float xhalf = 0.5f * (float)value;
        union.f = (float)value;
        union.tmp = 0x5f375a86 - (union.tmp >> 1);
        union.f = union.f * (1.5f - xhalf * union.f * union.f);
        return union.f * value;
    }
    // Goofiness
    [StructLayout(LayoutKind.Explicit)]
    private struct FloatIntUnion
    {
        [FieldOffset(0)]
        public float f;
        [FieldOffset(0)]
        public int tmp;
    }
}
