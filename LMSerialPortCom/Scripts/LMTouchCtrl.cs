using System;
using System.Collections.Generic;
using UnityEngine;

public class LMTouchCtrl : MonoBehaviour
{
    private Dictionary<string, float> m_lastPosDict;
    [SerializeField]
    private Vector3 m_lastInput;

    public Action<Vector3> onTouch3D;

    public enum TouchDimension
    {
        TwoD,
        ThreeD
    }

    public LayerMask rayMaskForThreeD;

    public TouchDimension touchDimension;

    private void OnEnable()
    {
        m_lastPosDict = new Dictionary<string, float>();
    }

    private void Update()
    {
        if (touchDimension == TouchDimension.TwoD)
            Get2DTouchValue();

        else
            Get3DTouchValue();
    }

    private void Get2DTouchValue()
    { }

    private void Get3DTouchValue()
    {
        Camera cam = Camera.main;

        if (CheckIsPressed())
        {
            Vector3 screenPos = GetSingleInputPos();
            Ray ray = cam.ScreenPointToRay(screenPos);

            RaycastHit hit;

            Debug.DrawRay(ray.origin, ray.direction * 1000f, Color.red);

            if (Physics.Raycast(ray, out hit, rayMaskForThreeD))
            {
                if (onTouch3D != null)
                    onTouch3D(hit.point);
            }
        }
    }

    private bool CheckIsPressed()
    {
        return Input.touchCount > 0 || Input.GetMouseButton(0);
    }

    private Vector3 GetSingleInputPos()
    {
        if (Input.touchCount > 0)
        {
            m_lastInput = Input.GetTouch(0).position;
        }
        else if (Input.GetMouseButton(0))
        {
            m_lastInput = Input.mousePosition;
        }

        return m_lastInput;
    }
}