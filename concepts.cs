/*
MAKE SURE TO DELETE THIS FILE BEFORE BUILDING

public override void OnUpdate()
{
    // Load the new skybox material
    newSkyboxMaterial = Resources.Load<Material>("DayWalkersResources/skybox.mat");
    if (newSkyboxMaterial == null)
    {
        LoggerInstance.Msg("Failed to load new skybox material!");
    }
    else
    {
        // Apply the new skybox material
        RenderSettings.skybox = newSkyboxMaterial;
    }

    // Handle PageDown key
    if (Input.GetKeyDown(KeyCode.PageDown))
    {
        LoggerInstance.Msg("PageDown key pressed");
        SetSkyboxExposure(Night, "Night");
    }

    // Handle PageUp key
    if (Input.GetKeyDown(KeyCode.PageUp))
    {
        LoggerInstance.Msg("PageUp key pressed");
        SetSkyboxExposure(Dawn, "Dawn");
    }

    // Handle Delete key
    if (Input.GetKeyDown(KeyCode.Delete))
    {
        LoggerInstance.Msg("Delete key pressed");
        SetSkyboxExposure(Day, "Day");
    }
}

private void SetSkyboxExposure(float exposure, string timeOfDay)
{
    if (RenderSettings.skybox != null)
    {
        // Log the current exposure value
        float currentExposure = RenderSettings.skybox.GetFloat("_Exposure");
        LoggerInstance.Msg($"Current exposure: {currentExposure}");

        RenderSettings.skybox.SetFloat("_Exposure", exposure);
        DynamicGI.UpdateEnvironment();

        // Log the applied exposure value
        float newExposure = RenderSettings.skybox.GetFloat("_Exposure");
        LoggerInstance.Msg($"Time set to {timeOfDay} with exposure: {newExposure}");
    }
    else
    {
        LoggerInstance.Msg("Skybox is not set in RenderSettings!");
    }
}
*/

// concept
/* public override void OnFixedUpdate()
 {
     string material = "none";

     RenderSettings.skybox = (Material)material;
     DynamicGI.UpdateEnvironment();

 }
*/

// for memes :)

// only used for debugging reasons
// LoggerInstance.Msg(GameObject.FindGameObjectsWithTag("Untagged"));  //returns GameObject[])   

// this method is obsolete and just doesn't work
/*
// Make a game object
GameObject lightGameObject = new GameObject("The Light");

// Add the light component
Light lightComp = lightGameObject.AddComponent<Light>();

lightComp.enabled = true;

// Set color and position
lightComp.color = Color.white;

// Set the position (or any transform property)
lightGameObject.transform.position = new Vector3(0, 1000, 0);
lightComp.intensity = (float)V;
lightComp.range = 10000;
*/
