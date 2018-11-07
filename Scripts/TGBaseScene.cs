﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct ScreenshotCropInfo
{
    public int xPos, yPos;
    public int width, height;

    public ScreenshotCropInfo(int xPos, int yPos, int width, int height)
    {
        this.xPos = xPos;
        this.yPos = yPos;
        this.width = width;
        this.height = height;
    }
}

public class TGBaseScene : MonoBehaviour
{
    protected float m_startTime;
    protected float m_timePassed;
    public Dictionary<string, string> additionDataToSave = new Dictionary<string, string>();

    public TGController controller { get; private set; }
    public ScreenshotCropInfo screenshotCropInfo = new ScreenshotCropInfo(0, 0, 1920, 1080);

    public System.Action<string> onCaptureScreen;

    public float TimePassed
    {
        get { return m_timePassed; }
        set
        {
            OnTimePassed(value);
        }
    }

    public bool isActive = false;

    public virtual void Init() 
    {
        controller = TGController.Instance;
    }

    public virtual void OnStart()
    {
        isActive = true;

        m_startTime = Time.time;

        TimePassed = 0;

        Recalibration();
    }

    public virtual void OnUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        { 
            ExitScene();
            return;
        }

        TimePassed += Time.deltaTime;
    }

    public virtual void ExitScene()
    {
        if (!additionDataToSave.ContainsKey(ssKey))
        {
            additionDataToSave.Add(ssKey, string.Empty);
        }
        isActive = false;
    }

    public void DelayCall(System.Action _func, float _delay)
    {
        StartCoroutine(DelayCallRoutine(_func, _delay));
    }

    private bool m_captureScreen;

    public void CaptureScreen(System.Action<string> callback = null)
    {
        m_captureScreen = true;
        onCaptureScreen = callback;
    }

    public virtual void Recalibration()
    {
        controller.inputSetting.Recalibration();
    }

    private void LateUpdate()
    {
        if (m_captureScreen)
        {
            var fileName = System.DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
            StartCoroutine(RecordFrame(fileName));
            m_captureScreen = false;
        }
    }
    IEnumerator RecordFrame(string _dateString)
    {
        yield return new WaitForEndOfFrame();
        float ratio = (float)screenshotCropInfo.width / screenshotCropInfo.height;
        int width = 700;
        int height = Mathf.RoundToInt((float)width / ratio);

        var raw = ScreenCapture.CaptureScreenshotAsTexture();
        Color[] c = raw.GetPixels(screenshotCropInfo.xPos,
                                  screenshotCropInfo.yPos,
                                  screenshotCropInfo.width,
                                  screenshotCropInfo.height);
        var tex = new Texture2D(screenshotCropInfo.width, screenshotCropInfo.height);
        tex.SetPixels(c);
        tex.Apply(false);

        tex = TextureScaler.ResizeTexture(tex, TextureScaler.ImageFilterMode.Bilinear, width, height);

        byte[] bytes = tex.EncodeToPNG();
        GameObject.Destroy(tex);

        string fileName = _dateString + ".png";

        controller.fileWriter.Write(fileName, bytes);

        SaveScreenshotKey(fileName);

        if (onCaptureScreen != null)
        {
            onCaptureScreen(fileName);
            onCaptureScreen = null;
        }
    }

    private const string ssKey = "图片";

    private void SaveScreenshotKey(string fileName)
    {

        if (additionDataToSave.ContainsKey(ssKey))
        {
            additionDataToSave[ssKey] += "|" + fileName;
        }
        else
        {
            additionDataToSave.Add(ssKey, fileName);
        }
    }


    protected virtual void OnTimePassed(float _value)
    {
        m_timePassed = _value;
    }

    IEnumerator DelayCallRoutine(System.Action _func, float _delay)
    {
        yield return new WaitForSeconds(_delay);
        _func();
    }
}
