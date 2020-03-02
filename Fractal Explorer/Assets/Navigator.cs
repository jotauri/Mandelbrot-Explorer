using UnityEngine;

public class Navigator : MonoBehaviour
{

    public Material material;

    MenuControl menuControl;
    PresetData currentSettings;
    int monochrom = 0;
    string[] monochromType;
    int shading = 0;
    string[] shadingType;
    int floodShape = 0;
    string[] shapeType;
    bool userInput = true;
    Vector2 pos;
    Vector2 smoothPos;
    float scale = 4;
    float smoothScale;


    void Start()
    {
        // Application.targetFrameRate = 60;
        // QualitySettings.vSyncCount = 0;
        menuControl = FindObjectOfType<MenuControl>();
        shadingType = new string[4];
        shadingType[0] = "None";
        shadingType[1] = "Mixed";
        shadingType[2] = "Light";
        shadingType[3] = "Dark";
        shapeType = new string[6];
        shapeType[0] = "Waves";
        shapeType[1] = "Flowers";
        shapeType[2] = "Scales";
        shapeType[3] = "Feathers";
        shapeType[4] = "Fungi";
        shapeType[5] = "Coral";
        monochromType = new string[2];
        monochromType[0] = "Off";
        monochromType[1] = "On";
        currentSettings = new PresetData();
        LoadPreset(0);
    }

    void Update()
    {
        HandleUserInput();
        UpdateShader();
    }

    private void UpdateShader()
    {
        smoothPos = Vector2.Lerp(smoothPos, pos, .1f);
        smoothScale = Mathf.Lerp(smoothScale, scale, .1f);
        float aspect = (float)Screen.width / (float)Screen.height;

        float scaleX = smoothScale;
        float scaleY = smoothScale;

        if (aspect > 1f)
            scaleY /= aspect;
        else
            scaleX *= aspect;
        material.SetVector("_Area", new Vector4(smoothPos.x, smoothPos.y, scaleX, scaleY));
    }

    private void HandleUserInput()
    {
        // the inputfield is supposed to give back the controll but it does not work sometimes..
        if (!userInput)
            if (Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2) || Input.GetKeyDown(KeyCode.Escape))
                userInput = true;
            else
                return;

        // zoom with keypad
        if (Input.GetKey(KeyCode.KeypadPlus) || Input.GetKey(KeyCode.Joystick1Button2))
            scale *= .98f;
        if (Input.GetKey(KeyCode.KeypadMinus) || Input.GetKey(KeyCode.Joystick1Button0))
            scale *= 1.02f;
        // zoom with mousewheel
        scale -= Input.GetAxis("Mouse ScrollWheel") * scale * 2;
        if (scale < .0005f)
            scale = .0005f;
        
        // move with directional keys
        pos.x += Input.GetAxisRaw("Horizontal") * scale * Time.deltaTime * .5f;
        pos.y += Input.GetAxisRaw("Vertical") * scale * Time.deltaTime * .5f;
        // move with mouse
        if (Input.GetMouseButton(2))
        {
            pos.x -= Input.GetAxis("Mouse X") * scale * Time.deltaTime * 2;
            pos.y -= Input.GetAxis("Mouse Y") * scale * Time.deltaTime * 2;
        }
    }

    // used when inputfield is set
    public void UserInputSwitch(bool s) => userInput = s;

    // important, when sliders are changed by code they call their functions (can't avoid?!)
    // so when we want to change the shader we only have to change the slider
    public void FloodSlider(float f)
    {
        material.SetFloat("_Flood", f);
        currentSettings.flood = f;
        menuControl.SetInputField("Flood", Mathf.RoundToInt(f * 100));
    }

    public void FloodInputField(string f)
    {
        if (int.TryParse(f, out int f_Int))
        {
            f_Int = Mathf.Min(Mathf.Max(f_Int, 0), 100);
            currentSettings.flood = f_Int * .01f;
            menuControl.SetSlider("Flood", f_Int * .01f);
        }
    }

    public void RepeatsSlider(float rep)
    {
        material.SetFloat("_Repeats", rep * 100 + 4);
        currentSettings.repeats = rep;
        menuControl.SetInputField("Repeats", Mathf.RoundToInt(rep * 100));
    }

    public void RepeatsInputField(string rep)
    {
        if (int.TryParse(rep, out int rep_Int))
        {
            rep_Int = Mathf.Min(Mathf.Max(rep_Int, 0), 100);
            menuControl.SetSlider("Repeats", rep_Int * .01f);
        }
    }

    public void DensitySlider(float den)
    {
        material.SetFloat("_Density", Mathf.Clamp(den * den, .01f, 1));
        currentSettings.density = den;
        menuControl.SetInputField("Density", Mathf.RoundToInt(den * 100));
    }

    public void DensityInputField(string den)
    {
        if (int.TryParse(den, out int den_Int))
        {
            den_Int = Mathf.Min(Mathf.Max(den_Int, 1), 100);
            menuControl.SetSlider("Density", den_Int * .01f);
        }
    }

    public void RedSlider(float r)
    {
        material.SetFloat("_Red", r);
        currentSettings.red = r;
        menuControl.SetInputField("Red", Mathf.RoundToInt(r * 100));
    }

    public void RedInputField(string r)
    {
        if (int.TryParse(r, out int r_Int))
        {
            r_Int = Mathf.Min(Mathf.Max(r_Int, 0), 100);
            menuControl.SetSlider("Red", r_Int * .01f);
        }
    }

    public void GreenSlider(float g)
    {
        material.SetFloat("_Green", g);
        currentSettings.green = g;
        menuControl.SetInputField("Green", Mathf.RoundToInt(g * 100));
    }

    public void GreenInputField(string g)
    {
        if (int.TryParse(g, out int g_Int))
        {
            g_Int = Mathf.Min(Mathf.Max(g_Int, 0), 100);
            menuControl.SetSlider("Green", g_Int * .01f);
        }
    }

    public void BlueSlider(float b)
    {
        material.SetFloat("_Blue", b);
        currentSettings.blue = b;
        menuControl.SetInputField("Blue", Mathf.RoundToInt(b * 100));
    }

    public void BlueInputField(string b)
    {
        if (int.TryParse(b, out int b_Int))
        {
            b_Int = Mathf.Min(Mathf.Max(b_Int, 0), 100);
            menuControl.SetSlider("Blue", b_Int * .01f);
        }
    }

    public void DissolveSlider(float d)
    {
        material.SetFloat("_Dissolve", d * d * d * d + 1);
        currentSettings.dissolve = d;
        menuControl.SetInputField("Dissolve", Mathf.RoundToInt(d * 100));
    }

    public void DissolveInputField(string d)
    {
        if (int.TryParse(d, out int d_Int))
        {
            d_Int = Mathf.Min(Mathf.Max(d_Int, 0), 100);
            menuControl.SetSlider("Dissolve", d_Int * .01f);
        }
    }

    public void MovementSlider(float m)
    {
        material.SetFloat("_Movement", m * .3f);
        currentSettings.movement = m;
        menuControl.SetInputField("Movement", Mathf.RoundToInt(m * 100));
    }

    public void MovementInputField(string m)
    {
        if (int.TryParse(m, out int m_Int))
        {
            m_Int = Mathf.Min(Mathf.Max(m_Int, 0), 100);
            menuControl.SetSlider("Movement", m_Int * .01f);
        }
    }

    public void BreathingSlider(float br)
    {
        material.SetFloat("_Breathing", br * 2);
        currentSettings.breathing = br;
        menuControl.SetInputField("Breathing", Mathf.RoundToInt(br * 100));
    }

    public void BreathingInputField(string br)
    {
        if (int.TryParse(br, out int br_Int))
        {
            br_Int = Mathf.Min(Mathf.Max(br_Int, 0), 100);
            menuControl.SetSlider("Breathing", br_Int * .01f);
        }
    }

    public void SaturationSlider(float sat)
    {
        material.SetFloat("_Saturation", sat);
        currentSettings.saturation = sat;
        menuControl.SetInputField("Saturation", Mathf.RoundToInt(sat * 100));
    }

    public void SaturationInputField(string sat)
    {
        if (int.TryParse(sat, out int sat_Int))
        {
            sat_Int = Mathf.Min(Mathf.Max(sat_Int, 0), 100);
            menuControl.SetSlider("Saturation", sat_Int * .01f);
        }
    }

    public void ToggleMonochrom()
    {
        monochrom++;
        if (monochrom > 1)
            monochrom = 0;
        material.SetFloat("_Monochrom", monochrom);
        currentSettings.monochrom = monochrom;
        menuControl.SetText("Monochrom", "Monochrom: " + monochromType[monochrom]);
    }

    public void ToggleShading()
    {
        shading++;
        if (shading > 3)
            shading = 0;
        material.SetFloat("_Shading", shading);
        currentSettings.shading = shading;
        menuControl.SetText("Shading", "Shading: " + shadingType[shading]);
    }

    public void ToggleFloodShape()
    {
        floodShape++;
        if (floodShape > 5)
            floodShape = 0;
        material.SetFloat("_FloodShape", floodShape);
        currentSettings.floodShape = floodShape;
        menuControl.SetText("FloodShape", "Shape: " + shapeType[floodShape]);
    }


    public void RandomizeColor()
    {
        menuControl.SetSlider("Red", Random.Range(0f, 1f));
        menuControl.SetSlider("Green", Random.Range(0f, 1f));

        // "no blue" gives good colors sometimes
        if (Random.Range(0, 4) != 0)
            menuControl.SetSlider("Blue", Random.Range(0f, 1f));
        else
            menuControl.SetSlider("Blue", 0);

        if (Random.Range(0, 2) == 0)
            menuControl.SetSlider("Saturation", Random.Range(0f, 1f));
        else
            menuControl.SetSlider("Saturation", 1);
    }

    public void RandomizeAll()
    {
        RandomizeColor();
        // menuControl.SetSlider("Flood", Random.Range(0f, 1f));
        menuControl.SetSlider("Repeats", Random.Range(0f, 1f));
        menuControl.SetSlider("Density", Random.Range(0f, 1f));

        int r = Random.Range(0, 5);
        if (r < 3)
            menuControl.SetSlider("Movement", Random.Range(0f, 1f));
        else
            menuControl.SetSlider("Movement", 0);
        if (r > 1)
            menuControl.SetSlider("Dissolve", Random.Range(0f, 1f));
        else
            menuControl.SetSlider("Dissolve", 0);

        r = Random.Range(0, 3);
        if (r == 0)
            menuControl.SetSlider("Breathing", Mathf.Pow(Random.Range(0f, 1f), 2));
        else
            menuControl.SetSlider("Breathing", 0);

        r = Random.Range(0, 4);
        material.SetFloat("_Shading", r);
        menuControl.SetText("Shading", "Shading: " + shadingType[r]);
        currentSettings.shading = r;
        shading = r;

        r = Random.Range(0, 6);
        material.SetFloat("_FloodShape", r);
        menuControl.SetText("FloodShape", "Shape: " + shapeType[r]);
        currentSettings.floodShape = r;
        floodShape = r;
    }

    public void SavePreset(string fileName)
    {
        if (!Input.GetKeyDown(KeyCode.Return) && !Input.GetKeyDown(KeyCode.KeypadEnter))
            return;
        if (SaveSystem.SavePreset(currentSettings, fileName) == true)
            menuControl.DisplaySavedText();
        menuControl.ResetSaveInputfield();
        menuControl.RefreshPresetDropdown();
        userInput = true;
    }

    public void LoadPreset(int index)
    {
        currentSettings = SaveSystem.LoadPreset(SaveSystem.GetFileList()[index]);
        menuControl.RefreshPresetDropdown();

        menuControl.SetSlider("Flood", currentSettings.flood);
        menuControl.SetSlider("Repeats", currentSettings.repeats);
        menuControl.SetSlider("Density", currentSettings.density);
        menuControl.SetSlider("Red", currentSettings.red);
        menuControl.SetSlider("Green", currentSettings.green);
        menuControl.SetSlider("Blue", currentSettings.blue);
        menuControl.SetSlider("Saturation", currentSettings.saturation);
        menuControl.SetSlider("Movement", currentSettings.movement);
        menuControl.SetSlider("Breathing", currentSettings.breathing);
        menuControl.SetSlider("Dissolve", currentSettings.dissolve);

        material.SetFloat("_Monochrom", currentSettings.monochrom);
        menuControl.SetText("Monochrom", "Monochrom: " + monochromType[(int)currentSettings.monochrom]);
        monochrom = (int)currentSettings.monochrom;

        material.SetFloat("_Shading", currentSettings.shading);
        menuControl.SetText("Shading", "Shading: " + shadingType[(int)currentSettings.shading]);
        shading = (int)currentSettings.shading;

        material.SetFloat("_FloodShape", currentSettings.floodShape);
        menuControl.SetText("FloodShape", "Shape: " + shapeType[(int)currentSettings.floodShape]);
        floodShape = (int)currentSettings.floodShape;
    }

    public void Quit() => Application.Quit();
}
