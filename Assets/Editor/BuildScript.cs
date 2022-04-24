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
        public string WindowsBuildResult = "None";
        public string LinuxBuildResult = "None";
        public string WebGLBuildResult = "None";
    }

    public static void MesonBuild()
    {
        var targetPlatform = new TargetPlatform
        {
            targetPlatform = BuildTarget.WebGL.ToString()
        };

        var paths = GetBuildScenePaths();
        var buildResult = new BuildResult();

        var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        Debug.Log($"Documents Folder Path > {documentsPath}");
        var buildConfigPath = Path.Combine(documentsPath, "buildConfig.json");
        var buildResultPath = Path.Combine(documentsPath, "buildResult.json");
        Debug.Log($"buildConfigPath > {buildConfigPath}");
        var configJson = File.ReadAllText(buildConfigPath);
        Debug.Log($"configJson > {configJson}");
        var config = JsonUtility.FromJson<BuildConfig>(configJson);
        Debug.Log($"Output Directory > {config.outputDir}");
        Debug.Log($"LocationPathName > {config.outputDir}");

        var buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.options = BuildOptions.Development;
        buildPlayerOptions.scenes = paths.ToArray();

        var locationPathName = config.outputDir;
        Debug.Log($"locationPathName > {locationPathName}");
        
        buildPlayerOptions.locationPathName = Path.Combine(locationPathName, $@"Linux\{PlayerSettings.productName}.x68_x64");
        buildPlayerOptions.target = BuildTarget.StandaloneLinux64;
        var buildReport = BuildPipeline.BuildPlayer(buildPlayerOptions);
        if (buildReport.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
        {
            Debug.Log("Linux Success");
            buildResult.LinuxBuildResult = "Success";
        }
        else
        {
            Debug.LogError("Linux Build Fail");
            buildResult.LinuxBuildResult = "Fail";
        }

        buildPlayerOptions.locationPathName = Path.Combine(locationPathName, $@"Windows\{PlayerSettings.productName}.exe");
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildReport = BuildPipeline.BuildPlayer(buildPlayerOptions);
        if (buildReport.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
        {
            Debug.Log("Windows Build Success");
            buildResult.WindowsBuildResult = "Success";
        }
        else
        {
            Debug.LogError("Windows Build Fail");
            buildResult.WindowsBuildResult = "Fail";
        }

        buildPlayerOptions.locationPathName = Path.Combine(locationPathName, $@"WebGL\{PlayerSettings.productName}");
        buildPlayerOptions.target = BuildTarget.WebGL;
        buildReport = BuildPipeline.BuildPlayer(buildPlayerOptions);
        if (buildReport.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
        {
            Debug.Log("WebGL Build Success");
            buildResult.WebGLBuildResult = "Success";
        }
        else
        {
            Debug.LogError("Windows Build Fail");
            buildResult.WebGLBuildResult = "Fail";
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