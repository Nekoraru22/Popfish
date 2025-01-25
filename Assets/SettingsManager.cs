using UnityEngine;
using System.Collections;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] private GameObject settingsPrefab;
    [SerializeField] private Canvas targetCanvas;
    
    private GameObject settingsInstance;
    private bool isSettingsOpen = false;
    private RectTransform settingsRectTransform;

    private void Awake()
    {
        Debug.Log("$: Awake - Component references check:");
        Debug.Log($"$: SettingsPrefab assigned: {settingsPrefab != null}");
        Debug.Log($"$: TargetCanvas assigned: {targetCanvas != null}");
        CreateSettingsInstance();
    }

    private void Start()
    {
        Debug.Log($"$: Start - Settings instance exists: {settingsInstance != null}");
        Debug.Log($"$: Start - Settings instance active state: {settingsInstance?.activeInHierarchy}");
        Debug.Log($"$: Start - isSettingsOpen flag: {isSettingsOpen}");
    }

    private void CreateSettingsInstance()
    {
        Debug.Log("$: CreateSettingsInstance - Starting creation process");
        
        if (settingsInstance != null)
        {
            Debug.Log("$: Destroying existing settings instance");
            Destroy(settingsInstance);
        }

        if (settingsPrefab != null && targetCanvas != null)
        {
            Debug.Log("$: All references valid, creating new settings instance");
            settingsInstance = Instantiate(settingsPrefab, targetCanvas.transform);
            settingsRectTransform = settingsInstance.GetComponent<RectTransform>();
            Debug.Log($"$: RectTransform component found: {settingsRectTransform != null}");
            
            settingsInstance.SetActive(false);
            Debug.Log($"$: New settings instance created and hidden. Active state: {settingsInstance.activeInHierarchy}");
        }
        else
        {
            Debug.LogError("$: ERROR - Missing references:");
            Debug.LogError($"$: SettingsPrefab: {settingsPrefab != null}");
            Debug.LogError($"$: TargetCanvas: {targetCanvas != null}");
        }
    }

    private void OnEnable()
    {
        Debug.Log("$: OnEnable called");
        if (settingsInstance != null)
        {
            settingsInstance.SetActive(false);
            Debug.Log("$: OnEnable - Force hiding settings");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("$: ESC key detected");
            ToggleSettings();
        }
    }

    private void ToggleSettings()
    {
        Debug.Log("$: ToggleSettings called");
        Debug.Log($"$: Current settings instance exists: {settingsInstance != null}");
        Debug.Log($"$: Current isSettingsOpen state: {isSettingsOpen}");

        if (settingsInstance == null)
        {
            Debug.Log("$: Settings instance was null, creating new instance");
            CreateSettingsInstance();
            return;
        }

        isSettingsOpen = !isSettingsOpen;
        settingsInstance.SetActive(isSettingsOpen);
        Debug.Log($"$: Settings toggled - New isSettingsOpen state: {isSettingsOpen}");
        Debug.Log($"$: Settings instance active state: {settingsInstance.activeInHierarchy}");

        if (isSettingsOpen)
        {
            CenterSettingsOnScreen();
        }
    }

    private void CenterSettingsOnScreen()
    {
        Debug.Log("$: CenterSettingsOnScreen called");
        Debug.Log($"$: RectTransform exists: {settingsRectTransform != null}");
        
        if (settingsRectTransform != null)
        {
            Vector2 previousPos = settingsRectTransform.anchoredPosition;
            settingsRectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            settingsRectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            settingsRectTransform.pivot = new Vector2(0.5f, 0.5f);
            settingsRectTransform.anchoredPosition = Vector2.zero;
            Debug.Log($"$: Centered settings - Previous pos: {previousPos}, New pos: {settingsRectTransform.anchoredPosition}");
        }
    }

    private void OnDisable()
    {
        Debug.Log("$: OnDisable called");
    }

    private void OnDestroy()
    {
        Debug.Log("$: OnDestroy called");
    }
}