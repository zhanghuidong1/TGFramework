﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TGUIRoot : MonoBehaviour
{
	public TutorialPanel tutorialPanel;
	public AutoGameOverPanel gameOverPanel;
	public ExitGamePanel exitGamePanel;
	public Transform gameplayPanel;
	public TMPro.TextMeshProUGUI scoreTxt;
	public TimeBar timeBar;
	public GetPointTextUI getScorePrefab;
	public GetPointTextUI lossScorePrefab;
	public Button exitBtn;
	public Button recalibrationBtn;
	public Button questionBtn;

	public void CreateScorePrefab(int _score, Vector3 _pos)
	{
		GetPointTextUI prefab = (_score > 0) ? getScorePrefab : lossScorePrefab;

		var score = Instantiate(prefab);
		score.transform.SetParent(gameplayPanel, false);
		score.transform.position = _pos;

		string sign = (_score > 0) ? "+" : string.Empty;
		score.SetText(sign + _score);
	}
}
