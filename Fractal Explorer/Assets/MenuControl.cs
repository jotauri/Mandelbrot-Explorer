using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MenuControl : MonoBehaviour
{

    public TMP_Dropdown resDropdown;
    public TMP_Dropdown loadDropdown;
    public TMP_InputField saveInputField;
    public TMP_Text savedText;
    public Toggle fullScreenToggle;
    public GameObject sliders;
    public GameObject options;
    Resolution[] resolutions;
    float fadeTimer;
    Color fadeColor;
    bool isFading = false;


    void Start()
    {
        resolutions = Screen.resolutions;
        resDropdown.ClearOptions();
        List<string> resOptions = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            resOptions.Add(option);

            if (resolutions[i].width == Screen.width &&
               resolutions[i].height == Screen.height)
                currentResolutionIndex = i;
        }
        resDropdown.AddOptions(resOptions);
        resDropdown.value = currentResolutionIndex;
        resDropdown.RefreshShownValue();

        fullScreenToggle.isOn = Screen.fullScreen;

        Application.targetFrameRate = 60;
        QualitySettings.vSyncCount = 0;
    }

    void Update()
    {
        // toggle sliders
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1))
        {
            ToggleMenu();
        }
    }

    public void ResetSaveInputfield() => saveInputField.text = null;

    public void ToggleMenu() => sliders.SetActive(!sliders.activeSelf);

    public void ToggleOptions() => options.SetActive(!options.activeSelf);

    public void ToggleFullScreen(bool fS) => Screen.fullScreen = fS;

    IEnumerator FadeText()
    {
        isFading = true;
        while (Time.time < fadeTimer)
        {
            fadeColor = savedText.color;
            fadeColor.a = Mathf.Lerp(0, 1, (fadeTimer - Time.time) / 2);
            savedText.color = fadeColor;
            yield return null;
        }
        isFading = false;
    }

    public void DisplaySavedText()
    {
        fadeTimer = Time.time + 2f;
        if (!isFading)
            StartCoroutine(FadeText());
    }

    public void RefreshPresetDropdown()
    {
        loadDropdown.ClearOptions();
        loadDropdown.AddOptions(SaveSystem.GetFileList());
    }

    // to set InputFields to the numbers set by sliders
    public void SetInputField(string name, float num)
    {
        foreach (Transform child in sliders.transform)
            if (child.gameObject.name == name)
                if (child.GetComponentInChildren<TMP_InputField>())
                    child.GetComponentInChildren<TMP_InputField>().text = num.ToString();
    }

    public void SetSlider(string name, float num)
    {        
        foreach (Transform child in sliders.transform)
            if (child.gameObject.name == name)
                if (child.GetComponentInChildren<Slider>())
                    child.GetComponentInChildren<Slider>().value = num;
    }

    public void SetText(string name, string text)
    {
        foreach (Transform child in sliders.transform)
            if (child.gameObject.name == name)
                if (child.GetComponentInChildren<TMP_Text>())
                    child.GetComponentInChildren<TMP_Text>().text = text;
    }

    public void SetResolution(int resIndex)
    {
        Resolution r = resolutions[resIndex];
        Screen.SetResolution(r.width, r.height, Screen.fullScreen);
    }
}
