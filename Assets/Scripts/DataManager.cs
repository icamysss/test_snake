using Steamworks;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DataManager : MonoBehaviour
{
	[Serializable]
	public class Data
	{
		public string skinName;

		public List<string> levelsEntered = new List<string>();

		public int stat_count_deaths;

		public int stat_count_bulletShots;

		public int stat_count_friendlyDeaths;

		public int stat_count_enemyKills;

		public int stat_count_friendlyKills;

		public int stat_count_frindlyKills_UnSaved;

		public int stat_count_friendlyRescued;

		public int stat_levels_finished_01;

		public int stat_levels_finished_02;

		public int stat_levels_finished_03;

		public int stat_levels_finished_04;

		public int stat_levels_finished_05;

		public bool mainGameFinished;

		public bool badAssFinished;

		public bool firstPersonFinished;

		public bool theMythFinished;

		public bool suitManFinished;

		public bool suitManUnlocked;

		public bool theMythDefeated;

		[SerializeField]
		public Dictionary<string, float> customFloatValues;

		[SerializeField]
		public Dictionary<string, int> customIntValues;

		[SerializeField]
		private float _mouseSensitivity;

		public bool holdToAim;

		public float mouseSensitivity
		{
			get
			{
				if (_mouseSensitivity < 0.0001f)
				{
					_mouseSensitivity = 0.0001f;
				}
				return _mouseSensitivity;
			}
			set
			{
				if (value < 0.0001f)
				{
					value = 0.0001f;
				}
				_mouseSensitivity = value;
			}
		}

		public float GetFloat(string key)
		{
			if (customFloatValues.TryGetValue(key, out float value))
			{
				return value;
			}
			return 0f;
		}

		public void SetFloat(string key, float value)
		{
			if (customFloatValues.ContainsKey(key))
			{
				customFloatValues[key] = value;
			}
			else
			{
				customFloatValues.Add(key, value);
			}
		}

		public int GetInt(string key)
		{
			if (customIntValues.TryGetValue(key, out int value))
			{
				return value;
			}
			return 0;
		}

		public void SetInt(string key, int value)
		{
			if (customIntValues.ContainsKey(key))
			{
				customIntValues[key] = value;
			}
			else
			{
				customIntValues.Add(key, value);
			}
		}

		public Data()
		{
			levelsEntered = new List<string>();
			mouseSensitivity = 5f;
			customFloatValues = new Dictionary<string, float>();
			customIntValues = new Dictionary<string, int>();
		}
	}

	public struct LevelSuccessContext
	{
		public int levelIndex;

		public int teamCount;
	}

	public struct SaveBroContext
	{
		public int teamCount;
	}

	public struct BombDamageContext
	{
		public int hurtCount;

		public int killCount;

		public bool killedPlayer;
	}

	public const int LevelCount = 7;

	private static int index;

	private static Data _data;

	private static ES3Settings _saveSettings;

	public static bool AchievementAndStatisticsEnabled
	{
		get
		{
			if (FriendlyDamageSwitch.FriendlyDamageOn)
			{
				return !CheatModeSwitch.CheatModeOn;
			}
			return false;
		}
	}

	public static Data data
	{
		get
		{
			if (_data == null)
			{
				_data = new Data();
			}
			return _data;
		}
		set
		{
			_data = value;
		}
	}

	private static ES3Settings saveSettings
	{
		get
		{
			if (_saveSettings == null)
			{
				_saveSettings = new ES3Settings(ES3.EncryptionType.AES, "Bilibili~GanBei!");
			}
			return _saveSettings;
		}
	}

	public static string GetSaveKeyName()
	{
		return "Save_" + index.ToString();
	}

	public static void Save()
	{
		ES3.Save(GetSaveKeyName(), data, saveSettings);
		ES3.CreateBackup(saveSettings);
		if (SteamManager.Initialized)
		{
			SteamUserStats.StoreStats();
		}
		PlayerPrefs.Save();
	}

	public static void Load()
	{
		try
		{
			data = ES3.Load(GetSaveKeyName(), new Data(), saveSettings);
		}
		catch
		{
			if (ES3.RestoreBackup(saveSettings))
			{
				data = ES3.Load(GetSaveKeyName(), new Data(), saveSettings);
				UnityEngine.Debug.Log("Backup Restored!");
			}
			else
			{
				UnityEngine.Debug.Log("Backup could not be restored as no backup exists.");
				data = new Data();
			}
		}
		if (SteamManager.Initialized)
		{
			SteamUserStats.RequestCurrentStats();
		}
	}

	public static void SetSavesIndex(int index)
	{
		DataManager.index = index;
	}

	public static bool EverEnteredLevel(string levelName)
	{
		return data.levelsEntered.Contains(levelName);
	}

	public static void SetEnteredLevel(string levelName)
	{
		if (AchievementAndStatisticsEnabled && !data.levelsEntered.Contains(levelName))
		{
			data.levelsEntered.Add(levelName);
		}
	}

	public static void SetAchievement(string achievementName, bool save = true)
	{
		if (AchievementAndStatisticsEnabled)
		{
			bool pbAchieved;
			if (SteamManager.Initialized && SteamUserStats.GetAchievement(achievementName, out pbAchieved) && !pbAchieved)
			{
				SteamUserStats.SetAchievement(achievementName);
				SteamUserStats.StoreStats();
			}
			if (save)
			{
				Save();
			}
		}
	}

	public static void UpdateSteamStats()
	{
		if (SteamManager.Initialized)
		{
			SteamUserStats.SetStat("stat_count_deaths", data.stat_count_deaths);
			SteamUserStats.SetStat("stat_count_bulletShots", data.stat_count_bulletShots);
			SteamUserStats.SetStat("stat_count_friendlyDeaths", data.stat_count_friendlyDeaths);
			SteamUserStats.SetStat("stat_count_enemyKills", data.stat_count_enemyKills);
			SteamUserStats.SetStat("stat_count_friendlyKills", data.stat_count_friendlyKills);
			SteamUserStats.SetStat("stat_count_frindlyKills_UnSaved", data.stat_count_frindlyKills_UnSaved);
			SteamUserStats.SetStat("stat_count_friendlyRescued", data.stat_count_friendlyRescued);
			SteamUserStats.SetStat("stat_levels_finished_01", data.stat_levels_finished_01);
			SteamUserStats.SetStat("stat_levels_finished_02", data.stat_levels_finished_02);
			SteamUserStats.SetStat("stat_levels_finished_03", data.stat_levels_finished_03);
			SteamUserStats.SetStat("stat_levels_finished_04", data.stat_levels_finished_04);
			SteamUserStats.SetStat("stat_levels_finished_05", data.stat_levels_finished_05);
			SteamUserStats.StoreStats();
		}
	}

	public static void CountPlayerDead()
	{
		if (AchievementAndStatisticsEnabled)
		{
			data.stat_count_deaths++;
			if (data.stat_count_deaths >= 1)
			{
				SetAchievement("count_death_1");
			}
		}
	}

	public static void CountBulletShot()
	{
		if (AchievementAndStatisticsEnabled)
		{
			data.stat_count_bulletShots++;
		}
	}

	public static void CountFriendlyDead()
	{
		if (AchievementAndStatisticsEnabled)
		{
			data.stat_count_friendlyDeaths++;
		}
	}

	public static void CountEnemyKilled()
	{
		if (AchievementAndStatisticsEnabled)
		{
			data.stat_count_enemyKills++;
			if (data.stat_count_enemyKills >= 1)
			{
				SetAchievement("enemyKilled_1");
			}
			if (data.stat_count_enemyKills >= 50)
			{
				SetAchievement("enemyKilled_50");
			}
			if (data.stat_count_enemyKills >= 100)
			{
				SetAchievement("enemyKilled_100");
				data.suitManUnlocked = true;
			}
		}
	}

	public static void CountFriendlyRescued()
	{
		if (AchievementAndStatisticsEnabled)
		{
			data.stat_count_friendlyRescued++;
			if (data.stat_count_friendlyRescued > 50)
			{
				SetAchievement("friendlyRescued_50");
			}
		}
	}

	public static void CountFriendlyKilled()
	{
		if (AchievementAndStatisticsEnabled)
		{
			data.stat_count_friendlyKills++;
			if (data.stat_count_friendlyKills >= 20)
			{
				SetAchievement("friendlyKilled_20");
			}
		}
	}

	public static void CountFriendlyKilledUnsaved()
	{
		if (AchievementAndStatisticsEnabled)
		{
			data.stat_count_frindlyKills_UnSaved++;
			if (data.stat_count_frindlyKills_UnSaved >= 10)
			{
				SetAchievement("friendlyKilledUnsaved_10");
			}
		}
	}

	public static void CountLevel1Finished()
	{
		data.stat_levels_finished_01++;
	}

	public static void CountLevel2Finished()
	{
		data.stat_levels_finished_02++;
	}

	public static void CountLevel3Finished()
	{
		data.stat_levels_finished_03++;
	}

	public static void CountLevel4Finished()
	{
		data.stat_levels_finished_04++;
	}

	public static void CountLevel5Finished()
	{
		data.stat_levels_finished_05++;
	}

	public static void CountLevelSuccess(LevelSuccessContext context)
	{
		if (!AchievementAndStatisticsEnabled)
		{
			return;
		}
		if (context.teamCount == 3)
		{
			SetAchievement("finishLevelWithFriends_4", save: false);
		}
		if (context.teamCount == 7)
		{
			SetAchievement("finishLevelWithFriends_8", save: false);
		}
		if (context.levelIndex == 7)
		{
			if (data != null)
			{
				data.mainGameFinished = true;
			}
			SetAchievement("finishedTheWholeGame", save: false);
			if (GameManager.Instance.LevelManager.gameMode == LevelManager.gameModes.single)
			{
				SetAchievement("badass", save: false);
				data.badAssFinished = true;
				string skinName = GetSkinName();
				if (skinName == "KR")
				{
					SetAchievement("finishSpecialModeWithSuitMan", save: false);
					data.suitManFinished = true;
				}
				else if (skinName == "?")
				{
					SetAchievement("finishSpecialModeWithTheMyth", save: false);
					data.theMythFinished = true;
				}
			}
			if (GameManager.Instance.LevelManager.game3CType == LevelManager.game3Ctypes.fps)
			{
				SetAchievement("firstPersonFinished", save: false);
				data.firstPersonFinished = true;
				string skinName2 = GetSkinName();
				if (skinName2 == "KR")
				{
					SetAchievement("finishSpecialModeWithSuitMan", save: false);
					data.suitManFinished = true;
				}
				else if (skinName2 == "?")
				{
					SetAchievement("finishSpecialModeWithTheMyth", save: false);
					data.theMythFinished = true;
				}
			}
		}
		if (context.levelIndex != 0 && context.levelIndex != 7)
		{
			SetAchievement("levelFinished_" + context.levelIndex.ToString(), save: false);
		}
		Save();
	}

	public static void SavedABro(SaveBroContext context)
	{
		if (context.teamCount >= 9)
		{
			SetAchievement("snakeLength_10");
		}
	}

	public static void HandleBombDamage(BombDamageContext context)
	{
		if (context.killCount == 4)
		{
			SetAchievement("kill4withABomb");
		}
		if (context.killedPlayer)
		{
			SetAchievement("killByABomb");
		}
	}

	public static string GetSkinName()
	{
		if (data != null)
		{
			return data.skinName;
		}
		return "Default";
	}

	public static void SetSkinName(string name)
	{
		if (data != null)
		{
			data.skinName = name;
			Save();
		}
	}

	public static bool IsMainGameFinished()
	{
		if (data != null)
		{
			return data.mainGameFinished;
		}
		return false;
	}

	public static void DeleteSaveData()
	{
		data = new Data();
		Save();
	}

	public static void ClearAchievement(string achievementName)
	{
		if (SteamManager.Initialized)
		{
			SteamUserStats.ClearAchievement(achievementName);
		}
	}

	public static void ClearAllAchievements()
	{
		if (SteamManager.Initialized)
		{
			string[] array = new string[25]
			{
				"count_death_1",
				"finishLevelWithFriends_4",
				"snakeLength_10",
				"friendlyKilledUnsaved_10",
				"killAllFriendliesWhenFinished",
				"finishedTheWholeGame",
				"finishLevelWithFriends_8",
				"kill4withABomb",
				"badass",
				"friendlyKilled_20",
				"enemyKilled_1",
				"enemyKilled_50",
				"enemyKilled_100",
				"killByABomb",
				"levelFinished_1",
				"levelFinished_2",
				"levelFinished_3",
				"levelFinished_4",
				"levelFinished_5",
				"friendlyRescued_50",
				"peaceElite",
				"firstPersonFinished",
				"finishSpecialModeWithSuitMan",
				"defeatTheMyth",
				"finishSpecialModeWithTheMyth"
			};
			for (int i = 0; i < array.Length; i++)
			{
				SteamUserStats.ClearAchievement(array[i]);
			}
		}
	}
}
