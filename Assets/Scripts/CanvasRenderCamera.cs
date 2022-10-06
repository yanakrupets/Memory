using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasRenderCamera : MonoBehaviour
{
    private Canvas _canvas;
    private Camera _camera;

    void Start()
    {
        _canvas = GetComponent<Canvas>();
        _camera = GameObject.Find("Main Camera").transform.Find("UICamera").GetComponent<Camera>();

        _canvas.worldCamera = _camera;
    }
}
