﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
	#region Singleton

	public static Player instance;

	void Awake()
	{
		instance = this;
	}

	#endregion
	//Небольшие данные об игроке
	void Start()
	{
		playerStats.OnHealthReachedZero += Die;
	}

	public CharacterCombat playerCombatManager;
	public PlayerStats playerStats;


	void Die()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);//смерть игрока
	}
}
