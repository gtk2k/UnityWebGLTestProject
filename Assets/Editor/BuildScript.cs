using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;

public static class BuildScript
{
    private class TargetPlatform
    {
        public string targetPlatform;        
    }

    private class BuildConfig
    {
        public string outputDir;
    }

    private class BuildResult
    {
        public string buildResult;
    }

    public static void MesonBuild()
    {
        var targetPlatform = new TargetPlatform
        {
            targetPlatform = BuildTarget.WebGL.ToString()
        };

        var paths = GetBuildScenePaths();
        var buildOptions = BuildOptions.Development;

        var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        Debug.Log($"Documents Folder Path > {documentsPath}");
        var buildConfigPath = Path.Combine(documentsPath, "buildConfig.json");
        var buildResultPath = Path.Combine(documentsPath, "buildResult.json");
        Debug.Log($"buildConfigPath > {buildConfigPath}");
        //var configJson = File.ReadAllText(buildConfigPath);
        //Debug.Log($"configJson > {configJson}");
        //var config = JsonUtility.FromJson<BuildConfig>(configJson);
        //Debug.Log($"Output Directory > {config.outputDir}");
        //Debug.Log($"LocationPathName > {config.outputDir}");
        //var commitHash = config.outputDir.Substring(config.outputDir.LastIndexOf("\\") + 1);
        var buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = paths.ToArray();

        //var locationPathName = $"{config.outputDir}\\{PlayerSettings.productName}";
        //Debug.Log($"locationPathName > {locationPathName}");
        buildPlayerOptions.locationPathName = @"D:\MESON_Projects_2\UnityWebGLTestProject\LinuxBuild\hoge.x86_x64";//locationPathName;
        buildPlayerOptions.target = BuildTarget.StandaloneLinux64;
        buildPlayerOptions.options = BuildOptions.Development;

        var buildReport = BuildPipeline.BuildPlayer(buildPlayerOptions);

        buildPlayerOptions.locationPathName = @"D:\MESON_Projects_2\UnityWebGLTestProject\WindowsBuild\hoge.exe";//locationPathName;
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildReport = BuildPipeline.BuildPlayer(buildPlayerOptions);

        var summary = buildReport.summary;

        var buildResult = new BuildResult();
        if (summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
        {
            Debug.Log("Success");
            buildResult.buildResult = "Success";
        }
        else
        {
            Debug.LogError("Fail");
            buildResult.buildResult = "Fail";
        }
        File.WriteAllText(buildResultPath, JsonUtility.ToJson(buildResult, true));
    }

    private static IEnumerable<string> GetBuildScenePaths()
    {
        var scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
        return scenes
            .Where((arg) => arg.enabled)
            .Select((arg) => arg.path);
    }
}