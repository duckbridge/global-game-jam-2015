public class GameSettings {

	public const float CAMERA_FOLLOW_SPEED_X = 0.05f;
	public const float CAMERA_FOLLOW_SPEED_Y = 0.005f;

	public const float CAMERA_MINIMUM_FOLLOW_DISTANCE_X = 0f;
	public const float CAMERA_MINIMUM_FOLLOW_DISTANCE_Y = 10f;

	public const float DEFAULT_MUSIC_VOLUME = 0.8f;
	public const float DEFAULT_FX_VOLUME = 1f;

	public static float SAVEDBACKGROUNDMUSICTIME = 0f;

	public const string FX_SAVE_NAME = "LuckgameFXVolume";
	public const string BG_SAVE_NAME = "LuckgameBGVolume";

	public const int LUCK_MAX_VALUE = 8;
	public const int LUCK_MIN_VALUE = 0;

	public const int ENEMY_LAYER = 8;
	public const int LOOT_LAYER = 9;
	public const int PLAYER_LAYER = 10;
	public const int IGNORE_PLAYER_LAYER = 11;
	public const int IGNORE_ENEMY_LAYER = 12;
	public const int IGNORE_SELF_LAYER = 13;
	public const int RAGDOLL_LAYER = 14;
	public const int IGNORE_VILLAGER_LAYER = 15;

	public const string GAME_SAVE_NAME = "YesKingGameSave_";
	public const string ALLSAVES = "AllSaves_YesKing_";

	public static int CHOSEN_SAVE_SLOT = 1;

	public const bool ERASE_SAVED_DATA = false;

	public static bool IS_USING_LEVELSELECT = false;
	public const Scene LEVEL_SELECT_SCENE = Scene.NONE;

	public const string ANIMATION_STANDINGPREFIX = "Standing";
	public const string ANIMATION_RUNNINGPREFIX = "Running";
	public const string ANIMATION_JUMPINGPREFIX = "Jumping";
	public const string ANIMATION_DUCKINGPREFIX = "Ducking";
	public const string ANIMATION_FALLINGPREFIX = "Falling";
	public const string ANIMATION_WALKINGPREFIX = "Walking";

	public const float MINIMUM_VELOCITY_FOR_MOVE = .1f;
	public const float MINIMUM_VELOCITY_FOR_ROLL = .01f;
	public const float MINIMUM_RUNVELOCITY = 7f;

	public static string GetFullSaveName() {
		return GAME_SAVE_NAME + CHOSEN_SAVE_SLOT;
	}

	public static string GetFullSaveNameAndAllSaves() {
		return GAME_SAVE_NAME + CHOSEN_SAVE_SLOT + ALLSAVES;
	}

	public static string GetFullNameSavedObjects(int villageNumber, string objectName) {
		return GetFullSaveName() + "_" + villageNumber + "_" + objectName;
	}
}
