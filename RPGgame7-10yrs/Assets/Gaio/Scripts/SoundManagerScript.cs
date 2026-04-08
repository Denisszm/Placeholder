using UnityEngine;
using System.Collections;
public class SoundManagerScript : MonoBehaviour
{
    public static SoundManagerScript instance;

    [SerializeField] private GameObject soundEffectObject;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySoundEffect(AudioClip audioClip, float volume)
    {
        GameObject obj = Instantiate(soundEffectObject, transform);
        AudioSource audioSource = obj.GetComponent<AudioSource>();

        audioSource.clip = audioClip;
        audioSource.volume = volume;
        audioSource.ignoreListenerPause = true;

        audioSource.Play();

        StartCoroutine(DestroyWhenFinished(audioSource));
}
    private IEnumerator DestroyWhenFinished(AudioSource source)
    {
        // Wait until the sound finishes playing
        while (source.isPlaying)
        {
            yield return null;
        }

        Destroy(source.gameObject);
    }
}
