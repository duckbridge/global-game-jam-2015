using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SoundUtils {
	
	public static void SetSoundVolumeToSavedValue(SoundType soundType) {
		if(soundType == SoundType.FX) {
			float fxVolume = PlayerPrefs.GetFloat(GameSettings.FX_SAVE_NAME, GameSettings.DEFAULT_FX_VOLUME);
			SetSoundVolume(SoundType.FX, fxVolume);
		} else {
			float bgVolume = PlayerPrefs.GetFloat(GameSettings.BG_SAVE_NAME, GameSettings.DEFAULT_MUSIC_VOLUME);
			SetSoundVolume(SoundType.BG, bgVolume);
		}
	}

	public static void SetSoundVolume(SoundType soundType, float newVolume) {

		if(newVolume > 1 || newVolume < 0) {
			Logger.Log ("you cannot have the volume higher than 1 or lower than 0", LogType.Error);
			return;
		}

		List<SoundObject> sounds = SceneUtils.FindObjects<SoundObject>();
		foreach(SoundObject sound in sounds) {
			if(sound.soundType == soundType) {
				sound.SetVolume(newVolume);
			}
		}
	}

	public static void SetTimeScale(float newTimeScale) {
		List<SoundObject> sounds = SceneUtils.FindObjects<SoundObject>();
		foreach(SoundObject sound in sounds) {
			sound.SetTimeScale(newTimeScale);
		}
	}

	public static void ResetTimeScale() {
		List<SoundObject> sounds = SceneUtils.FindObjects<SoundObject>();
		foreach(SoundObject sound in sounds) {
			sound.ResetTimeScale();
		}
	}

	public static void FadeOutBackgroundMusic(float amount = 0) {
		List<SoundObject> sounds = SceneUtils.FindObjects<SoundObject>();
		foreach(FadingAudio fadingAudio in sounds) {
			if(fadingAudio.soundType == SoundType.BG) {
				if(amount > 0) {
					fadingAudio.FadeOut(amount);
				}
			}
		}
	}
	public static void MuteAll() {
		List<SoundObject> sounds = SceneUtils.FindObjects<SoundObject>();
		foreach(SoundObject sound in sounds) {
			sound.Mute();
		}
	}

	public static void UnMuteAll() {
		List<SoundObject> sounds = SceneUtils.FindObjects<SoundObject>();
		foreach(SoundObject sound in sounds) {
			sound.UnMute();
		}
	}

	public static void MuteAll(SoundType soundType) {
		List<SoundObject> sounds = SceneUtils.FindObjects<SoundObject>();
		foreach(SoundObject sound in sounds) {
			if(sound.soundType == soundType) {
				sound.Mute();
			}
		}
	}

	public static void UnMuteAll(SoundType soundType) {
		List<SoundObject> sounds = SceneUtils.FindObjects<SoundObject>();
		foreach(SoundObject sound in sounds) {
			if(sound.soundType == soundType) {
				sound.UnMute();
			}
		}
	}

	public static void SwapBackgroundMusic(List<SoundObject> oldMusic, List<SoundObject> newMusic, bool playForced = true) {

		foreach(SoundObject oldMusicSound in oldMusic) {
			oldMusicSound.Stop ();
			oldMusicSound.active = false;
		}
		
		newMusic.ForEach(music => music.active = true);

		SetSoundVolumeToSavedValue(SoundType.BG);
		
		foreach(SoundObject newSong in newMusic) {
			newSong.Play(playForced);
		}
	}
}
