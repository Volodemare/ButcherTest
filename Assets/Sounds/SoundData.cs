using UnityEngine;

[CreateAssetMenu(fileName = "NewSoundCollection", menuName = "Audio/Sound Data")]
public class SoundData : ScriptableObject
{
    [Header("Список звуков")]
    public AudioClip[] firstSoundList; 
}

public static class SoundExtensions
{
    public static void PlaySound(this SoundData soundData, int index)
    {
        if (soundData.firstSoundList == null || index < 0 || index >= soundData.firstSoundList.Length)
        {
            Debug.LogError("Некорректный индекс или список звуков пуст!");
            return;
        }
        
        GameObject temp = new GameObject("TempAudio");
        AudioSource source = temp.AddComponent<AudioSource>();
        source.PlayOneShot(soundData.firstSoundList[index]);
        
        Object.Destroy(temp, soundData.firstSoundList[index].length + 0.1f);
    }
}