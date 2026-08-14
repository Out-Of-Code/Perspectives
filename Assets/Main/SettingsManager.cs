using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    // ===== Default game-wide values =====
    private const float DefaultMasterVolume = 100f;
    private const float DefaultMusicVolume = 50f;
    private const float DefaultSFXVolume = 50f;

    private const bool DefaultFullscreen = true;
    private const bool DefaultVSync = true;
    private const int DefaultFPSLimit = 60;

    private const bool DefaultReduceFlashing = false;
    private const bool DefaultHighContrast = false;
    private const bool DefaultColorblindMode = false;

    private const bool DefaultConfirmBeforeQuitting = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadSettings();
    }

    public void LoadSettings()
    {
        float masterVolume = PlayerPrefs.GetFloat(
            "MasterVolume",
            DefaultMasterVolume
        );

        float musicVolume = PlayerPrefs.GetFloat(
            "MusicVolume",
            DefaultMusicVolume
        );

        float sfxVolume = PlayerPrefs.GetFloat(
            "SFXVolume",
            DefaultSFXVolume
        );

        bool fullscreen = PlayerPrefs.GetInt(
            "Fullscreen",
            DefaultFullscreen ? 1 : 0
        ) == 1;

        bool vsync = PlayerPrefs.GetInt(
            "VSync",
            DefaultVSync ? 1 : 0
        ) == 1;

        int fpsLimit = PlayerPrefs.GetInt(
            "FPSLimit",
            DefaultFPSLimit
        );

        bool reduceFlashing = PlayerPrefs.GetInt(
            "ReduceFlashing",
            DefaultReduceFlashing ? 1 : 0
        ) == 1;

        bool highContrast = PlayerPrefs.GetInt(
            "HighContrast",
            DefaultHighContrast ? 1 : 0
        ) == 1;

        bool colorblindMode = PlayerPrefs.GetInt(
            "ColorblindMode",
            DefaultColorblindMode ? 1 : 0
        ) == 1;

        bool confirmBeforeQuitting = PlayerPrefs.GetInt(
            "ConfirmBeforeQuitting",
            DefaultConfirmBeforeQuitting ? 1 : 0
        ) == 1;

        // Apply loaded settings here.
        ApplyMasterVolume(masterVolume);
        ApplyMusicVolume(musicVolume);
        ApplySFXVolume(sfxVolume);

        ApplyFullscreen(fullscreen);
        ApplyVSync(vsync);
        ApplyFPSLimit(fpsLimit);

        ApplyReduceFlashing(reduceFlashing);
        ApplyHighContrast(highContrast);
        ApplyColorblindMode(colorblindMode);
    }

    public void SetMasterVolume(float value)
    {
        PlayerPrefs.SetFloat("MasterVolume", value);
        ApplyMasterVolume(value);
        PlayerPrefs.Save();
    }

    public void SetMusicVolume(float value)
    {
        PlayerPrefs.SetFloat("MusicVolume", value);
        ApplyMusicVolume(value);
        PlayerPrefs.Save();
    }

    public void SetSFXVolume(float value)
    {
        PlayerPrefs.SetFloat("SFXVolume", value);
        ApplySFXVolume(value);
        PlayerPrefs.Save();
    }

    public void SetFullscreen(bool value)
    {
        PlayerPrefs.SetInt("Fullscreen", value ? 1 : 0);
        ApplyFullscreen(value);
        PlayerPrefs.Save();
    }

    public void SetVSync(bool value)
    {
        PlayerPrefs.SetInt("VSync", value ? 1 : 0);
        ApplyVSync(value);
        PlayerPrefs.Save();
    }

    public void SetFPSLimit(int value)
    {
        PlayerPrefs.SetInt("FPSLimit", value);
        ApplyFPSLimit(value);
        PlayerPrefs.Save();
    }

    public void SetReduceFlashing(bool value)
    {
        PlayerPrefs.SetInt("ReduceFlashing", value ? 1 : 0);
        ApplyReduceFlashing(value);
        PlayerPrefs.Save();
    }

    public void SetHighContrast(bool value)
    {
        PlayerPrefs.SetInt("HighContrast", value ? 1 : 0);
        ApplyHighContrast(value);
        PlayerPrefs.Save();
    }

    public void SetColorblindMode(bool value)
    {
        PlayerPrefs.SetInt("ColorblindMode", value ? 1 : 0);
        ApplyColorblindMode(value);
        PlayerPrefs.Save();
    }

    public void SetConfirmBeforeQuitting(bool value)
    {
        PlayerPrefs.SetInt("ConfirmBeforeQuitting", value ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void ApplyMasterVolume(float value)
    {
        // Connect to your audio system.
    }

    private void ApplyMusicVolume(float value)
    {
        // Connect to your music audio system.
    }

    private void ApplySFXVolume(float value)
    {
        // Connect to your SFX audio system.
    }

    private void ApplyFullscreen(bool value)
    {
        Screen.fullScreen = value;
    }

    private void ApplyVSync(bool value)
    {
        QualitySettings.vSyncCount = value ? 1 : 0;
    }

    private void ApplyFPSLimit(int value)
    {
        Application.targetFrameRate = value;
    }

    private void ApplyReduceFlashing(bool value)
    {
        // Your Perspectives can check this setting.
    }

    private void ApplyHighContrast(bool value)
    {
        // Your UI can check this setting.
    }

    private void ApplyColorblindMode(bool value)
    {
        // Your game can check this setting.
    }

    public void ResetToDefaults()
    {
        PlayerPrefs.DeleteKey("MasterVolume");
        PlayerPrefs.DeleteKey("MusicVolume");
        PlayerPrefs.DeleteKey("SFXVolume");

        PlayerPrefs.DeleteKey("Fullscreen");
        PlayerPrefs.DeleteKey("VSync");
        PlayerPrefs.DeleteKey("FPSLimit");

        PlayerPrefs.DeleteKey("ReduceFlashing");
        PlayerPrefs.DeleteKey("HighContrast");
        PlayerPrefs.DeleteKey("ColorblindMode");

        PlayerPrefs.DeleteKey("ConfirmBeforeQuitting");

        PlayerPrefs.Save();

        LoadSettings();
    }
}