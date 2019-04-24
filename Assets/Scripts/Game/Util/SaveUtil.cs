using UnityEngine;
using System.Collections;

public class SaveUtil {

	public static char DATA_SPLITTER = ':';

	public static void SaveData(int score) {
		PlayerPrefs.SetString(GameSettings.GetFullSaveName(), score+"");
	}

	public static int LoadData() {
		string savedData = PlayerPrefs.GetString(GameSettings.GetFullSaveName());
		int score = 0;

		if(savedData.Length > 0) {
			score = System.Convert.ToInt32(savedData);
		}

		return score;
	}
}
