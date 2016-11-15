using UnityEditor;

namespace Filibusters
{
    class BuildScripts
    {
        private static readonly string ScenesRoot = "Assets/"; 
        private static readonly string SceneSuffix = ".unity";

        public static string[] levels = new string[]
        {
            ScenesRoot + Scenes.START_MENU + SceneSuffix,
            ScenesRoot + Scenes.HOST_GAME + SceneSuffix,
            ScenesRoot + Scenes.JOIN_GAME + SceneSuffix,
            ScenesRoot + Scenes.HOW_TO_PLAY + SceneSuffix,
            ScenesRoot + Scenes.READY_SCREEN + SceneSuffix,
            ScenesRoot + Scenes.MAIN + SceneSuffix
        };

        [MenuItem("Builds/Build Both")]
        public static void BuildMacAndWin64()
        {
            BuildMac();
            BuildWin64();
        }

        [MenuItem("Builds/Build and Run Two Win64 Clients")]
        public static void BuildAndRunTwoWindowsClients()
        {
            string buildPathAndName = System.IO.Path.Combine("Builds", "FilibustersWin64.exe");
            const string successMsg = "Win64 build successful";
            const string failMsg = "win64 build failed";
            BuildAndRunXClients(buildPathAndName, BuildTarget.StandaloneWindows64,
                BuildOptions.None, successMsg, failMsg, 2);
        }

        [MenuItem("Builds/Build Win64")]
        public static void BuildWin64()
        {
            string buildPathAndName = System.IO.Path.Combine("Builds", "FilibustersWin64.exe");
            const string successMsg = "Win64 build successful";
            const string failMsg = "win64 build failed";
            Build(buildPathAndName, BuildTarget.StandaloneWindows64,
                BuildOptions.None, successMsg, failMsg);
        }

        [MenuItem("Builds/Build Mac")]
        public static void BuildMac()
        {
            string buildPathAndName = System.IO.Path.Combine("Builds", "FilibustersMac.app");
            const string successMsg = "Mac build successful";
            const string failMsg = "Mac build failed";
            Build(buildPathAndName, BuildTarget.StandaloneOSXUniversal,
                BuildOptions.None, successMsg, failMsg);
        }

        public static void Build(string path, BuildTarget target,
            BuildOptions options, string successMsg = "Build succeeded: ",
            string failMsg = "Build failed: ")
        {
            var errMessage = BuildPipeline.BuildPlayer(levels, path, target, options);
            if (string.IsNullOrEmpty(errMessage))
            {
                UnityEngine.Debug.Log(successMsg);
            }
            else
            {
                UnityEngine.Debug.Log(failMsg + errMessage);
            }
        }

        public static void BuildAndRunXClients(string path, BuildTarget target,
            BuildOptions options, string successMsg = "Build succeeded: ",
            string failMsg = "Build failed: ", int numClients = 1)
        {
            Build(path, target, options, successMsg, failMsg);
            for (int i = 0; i < numClients; ++i)
            {
                var process = new System.Diagnostics.Process();
                process.StartInfo.FileName = path;
                process.Start();
            }
        }

    }
}
