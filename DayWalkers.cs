using UnityEngine;
using MelonLoader;
using UnityEngine.Rendering.PostProcessing;
using Il2Cpp;

namespace DayWalkers
{
    public class DayWalkersMod : MelonMod
    {
        private readonly float Dawn = 0.85f;
        private readonly float Day = 1.15f;
        private readonly float Night = 0.5f;

        public float Time = 1.15f; // Default to Day      

        public override void OnInitializeMelon()
        {
            LoggerInstance.Msg("Made with love, by Coloured!");
            LoggerInstance.Msg("Special thanks to iLollek for the help!");
            LoggerInstance.Msg("Using any Car unlocker is considered piracy!!");

            // Set initial exposure to the Day value
            if (RenderSettings.skybox != null)
            {
                Time = Day;
                SetExposure(Time);
                LoggerInstance.Msg("Initial exposure set to Day!");
            }
            else
            {
                LoggerInstance.Msg("Skybox is not set in RenderSettings!");
            }
        }

        public override void OnUpdate()
        {
            var Jem = UnityEngine.Object.FindObjectOfType<GodConstant>();
            var camera = UnityEngine.Object.FindObjectOfType<Camera>();
            var postProcessVolume = UnityEngine.Object.FindObjectOfType<PostProcessVolume>();

            if (camera == null || postProcessVolume == null)
            {
                // LoggerInstance.Msg("Camera or PostProcessVolume not found!");
                return;
            }

            // Handle PageDown key to set time to Night
            if (Input.GetKeyDown(KeyCode.PageDown))
            {
                Time = Night;
                LoggerInstance.Msg("Set time to Night!");
            }

            // Handle PageUp key to set time to Dawn
            if (Input.GetKeyDown(KeyCode.PageUp))
            {
                Time = Dawn;
                DisableBloomGlobally();
                LoggerInstance.Msg("Set time to Dawn!");
            }

            // Handle Delete key to set time to Day
            if (Input.GetKeyDown(KeyCode.Delete))
            {
                Time = Day;
                DisableBloomGlobally();
                LoggerInstance.Msg("Set time to Day!");
            }

            // Continuously update the skybox exposure based on the Time variable
            if (RenderSettings.skybox != null)
            {
                Jem.reset_timeOfDay();
                DisableBloomGlobally();
                SetExposure(Time);
            }
        }

        private void DisableBloomGlobally()
        {
            var volumes = UnityEngine.Object.FindObjectsOfType<PostProcessVolume>();

            foreach (var volume in volumes)
            {
                var bloom = volume.profile.GetSetting<Bloom>();
                if (bloom != null)
                {
                    bloom.enabled.value = false;
                }
            }
        }
        private void SetExposure(float exposure)
        {
            if (RenderSettings.skybox != null)
            {
                RenderSettings.skybox.SetFloat("_Exposure", exposure);
                DynamicGI.UpdateEnvironment();
            }
            else
            {
                LoggerInstance.Msg("Skybox is not set in RenderSettings!");
            }
        }
    }
}
