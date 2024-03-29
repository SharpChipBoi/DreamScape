﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerState : MonoBehaviour
{
	public static PlayerState Instance;

	public Transform playerPosition;

	//TUTORIAL
	public PlayerStatistics localPlayerData = new PlayerStatistics();

	void Awake()
	{
		if (Instance == null)
			Instance = this;

		if (Instance != this)
			Destroy(gameObject);

		GlobalControl.Instance.Player = gameObject;
	}

	//В старте загружаем локальную дату игрока из GlobalControl.
	void Start()
	{
		localPlayerData = GlobalControl.Instance.savedPlayerData;
	}

	void Update()
	{

	}

}
