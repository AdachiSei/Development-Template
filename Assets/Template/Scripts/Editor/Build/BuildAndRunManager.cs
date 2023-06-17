using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TemplateEditor.Build
{
    public static class BuildAndRunManager
    {
        #region Constants

        private const string BUILD_DIRECTORY_NAME = "Builds";

        #endregion

        #region Private Methods

        [MenuItem("Build/Build And Run CurrentScene", false, 1)]
        private static void BuildAndRunPhotonTutorial()
        {
            var currentSceneName = SceneManager.GetActiveScene().name;
            BuildAndRun(currentSceneName, currentSceneName);
        }

        private static void BuildAndRun(string appName, string sceneName)
        {
            var targetScene = 
                EditorBuildSettings
                    .scenes
                    .FirstOrDefault
                    (scene => Path.GetFileNameWithoutExtension(scene.path) == sceneName);

            if (targetScene == null)
            {
                Debug.LogError("�V�[�����r���h�Z�b�e�B���O�X�ɒǉ����Ă�������");
                return;
            }

            var outputDirectory = Path.Combine(BUILD_DIRECTORY_NAME, appName);

            if (!Directory.Exists(outputDirectory))
                Directory.CreateDirectory(outputDirectory);

            var buildTarget = EditorUserBuildSettings.activeBuildTarget;
            var locationPath = Path.Combine(outputDirectory, MakeApplicationFileName(appName, buildTarget));
            var buildOptions = BuildOptions.SymlinkSources | BuildOptions.AutoRunPlayer;

            var originalName = PlayerSettings.productName;
            PlayerSettings.productName = appName;
            BuildPipeline.BuildPlayer(new EditorBuildSettingsScene[] { targetScene }, locationPath, buildTarget, buildOptions);
            PlayerSettings.productName = originalName;
            AssetDatabase.SaveAssets();
        }

        private static string MakeApplicationFileName(string fileName, BuildTarget buildTarget)
        {
            switch (buildTarget)
            {
                case BuildTarget.StandaloneWindows64:
                    return $"{fileName}.exe";
                case BuildTarget.StandaloneOSX:
                    return $"{fileName}.app";
            }
            return fileName;
        }

        #endregion
    }
}
