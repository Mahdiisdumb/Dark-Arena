using UnityEngine;
using UnityEngine.UI; // Or use TMPro if using TextMeshPro

[RequireComponent(typeof(AudioSource))]
public class MusicPlaylistUI : MonoBehaviour
{
    [Header("Playlist Settings")]
    public bool loop = true;
    public float volume = 1f;

    [Header("UI")]
    public Text musicNameText; // Assign a UI Text to show the track name

    private AudioClip[] playlist;
    private AudioSource audioSource;
    private int currentIndex = 0;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = volume;
        audioSource.loop = false;

        // Load all audio clips from Resources/music/
        playlist = Resources.LoadAll<AudioClip>("music");

        if (playlist.Length == 0)
        {
            Debug.LogWarning("No audio clips found in Resources/music/");
            if (musicNameText != null)
                musicNameText.text = "No music found";
            return;
        }

        PlayCurrentClip();
    }

    void Update()
    {
        if (playlist.Length == 0) return;

        // If current clip finished, go to next
        if (!audioSource.isPlaying)
        {
            currentIndex++;

            if (currentIndex >= playlist.Length)
            {
                if (loop)
                    currentIndex = 0;
                else
                    return;
            }

            PlayCurrentClip();
        }
    }

    void PlayCurrentClip()
    {
        audioSource.clip = playlist[currentIndex];
        audioSource.Play();

        if (musicNameText != null)
            musicNameText.text = $"Now Playing: {playlist[currentIndex].name}";

        Debug.Log($"Playing: {playlist[currentIndex].name}");
    }

    // Optional: skip to next track manually
    public void NextTrack()
    {
        if (playlist.Length == 0) return;

        currentIndex++;
        if (currentIndex >= playlist.Length)
            currentIndex = 0;

        PlayCurrentClip();
    }
}