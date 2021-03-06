﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitGamePanel : TGBasePanel
{
	public Button confirmBtn;
	public Button cancelBtn;
	public System.Action onFinishClosePanel;

	public void OnFinishExit()
	{
		if (onFinishClosePanel != null)
			onFinishClosePanel();
	}
}
