using UnityEngine;
using MelonLoader;
using UnityEngine.Rendering.PostProcessing;
using Il2Cpp;

namespace DayWalkers
{
    public class DayWalkersMod : MelonMod
    {
        private readonly float Dawn = 0.85f;
        private readonly float Day = 1.1f;
        private readonly float Night = 0.5f;

        public static float Time = 0.5f; // Default to Day      

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

        public static bool showMenu = false;
        private static object selectedPlayer;
        private static int selectedTab, yOffset = 0;
        public static void ToggleMenu() => showMenu = !showMenu;
        private static GUIStyle MainBox, Header, MainTabs = null;

        public static float timeSlider = 0.5f;
        private static float height;
        private static float width;

        public static bool timeReset = true;

        private static Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height]; for (int i = 0; i < pix.Length; ++i) pix[i] = col;

            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }

        public static void InitMenu()
        {
            var style = new GUIStyle();
            style.normal.textColor = Color.white;

            if (!showMenu) return;
            // Menu Background Setup
            GUI.BeginGroup(new Rect(10, 2, 340, 25), style);
            Header = new GUIStyle(GUI.skin.box);
            Header.normal.background = MakeTex(2, 2, new Color(1, 1, 1, 1));
            Header.normal.textColor = Color.black;
            GUI.Box(new Rect(10, 0, 328, 25), $"▼ Day-Walkers | V3.1.0 | Left Shift to open the overlay.", Header);
            GUI.EndGroup();

            GUI.BeginGroup(new Rect(10, 15, 339, 500));
            MainBox = new GUIStyle(GUI.skin.box);
            MainBox.normal.background = MakeTex(2, 2, new Color(0.25f, 0.25f, 0.25f, 1.0f));
            GUI.Box(new Rect(10, 12, 330, 500), $"https://patreon.com/nightrunnersgame", MainBox);
            GUI.contentColor = Color.yellow;
            GUI.Label(new Rect(50, 16.5f, 400, 42), $"___________________________________");
            GUI.contentColor = Color.white;
            MainTabs = new GUIStyle(GUI.skin.button);
            MainTabs.normal.background = MakeTex(2, 2, new Color(0f, 0f, 1f, 0.75f));
            MainTabs.normal.textColor = Color.black;
            
            Time = GUI.HorizontalSlider(new Rect(100, 40, 100, 30), Time, 0.5f, 1.1f);
            GUI.Box(new Rect(100, 60, 100, 30), $"Exposure: {Time}");
            GUI.Label(new Rect(25, 130, 300, 300), "Note: whenever you update the lightning, it does need a second to load!");
            GUI.Label(new Rect(50, 180, 275, 200), "Day-walkers was devloped by: Coloured");

            timeReset = GUI.Toggle(new Rect(100, 220, 100, 30), timeReset, "Lock time");

            /* if (GUI.Button(new Rect(20, 40, 60, 27), (selectedTab == 0) ? "► Time" : "Main", MainTabs)) { selectedTab = 0; }

            if (GUI.Button(new Rect(80, 40, 60, 27), (selectedTab == 1) ? "► Fun" : "Fun", MainTabs)) { selectedTab = 1; }
            if (GUI.Button(new Rect(140, 40, 60, 27), (selectedTab == 2) ? "► World" : "World", MainTabs)) { selectedTab = 2; }
            if (GUI.Button(new Rect(200, 40, 60, 27), (selectedTab == 3) ? "► Misc" : "Misc", MainTabs)) { selectedTab = 3; }
            if (GUI.Button(new Rect(260, 40, 60, 27), (selectedTab == 4) ? "► Users" : "Users", MainTabs)) { selectedTab = 4; } */

            // Choose the tab to render when selected.
            /* switch (selectedTab)
            {
                case 0: break;
                case 1: break;
                case 2: break;
                case 3: break;
                case 4: break;
            } */
            GUI.EndGroup();
        }

        public override void OnGUI()
        {
            InitMenu();
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

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                if (Cursor.visible == false)
                {
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    showMenu = true;
                } else
                {
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    showMenu = false;
                }
            }

            // Continuously update the skybox exposure based on the Time variable
            if (RenderSettings.skybox != null)
            {
                DisableBloomGlobally();
                SetExposure(Time);
            }

            if (timeReset == true)
            {
                Jem.reset_timeOfDay();
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

