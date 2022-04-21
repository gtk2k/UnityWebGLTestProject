using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;

public static class BuildScript
{
    private class BuildConfig
    {
        public string outputDir;
    }

    public static void WebGLBuild()
    {
        var paths = GetBuildScenePaths();
        var buildOptions = BuildOptions.Development;


        var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
        Debug.Log($"Documents Folder Path > {documentsPath}");
        var buildConfigPath = Path.Combine(documentsPath, "buildConfig.json");
        Debug.Log($"buildConfigPath > {buildConfigPath}");
        var configJson = File.ReadAllText(buildConfigPath);
        Debug.Log($"configJson > {configJson}");
        var config = JsonUtility.FromJson<BuildConfig>(configJson);
        Debug.Log($"Output Directory > {config.outputDir}");
        Debug.Log($"LocationPathName > {config.outputDir}");

        var buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = paths.ToArray();
        buildPlayerOptions.locationPathName = config.outputDir;
        buildPlayerOptions.target = BuildTarget.WebGL;
        buildPlayerOptions.options = BuildOptions.Development;

        var buildReport = BuildPipeline.BuildPlayer(buildPlayerOptions);

        var summary = buildReport.summary;
       
        if (summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
        {
            Debug.Log("Success");
        }
        else
        {
            Debug.LogError("Error");
        }
    }

    private static IEnumerable<string> GetBuildScenePaths()
    {
        var scenes = new List<EditorBuildSettingsScene>(EditorBuildSettings.scenes);
        return scenes
            .Where((arg) => arg.enabled)
            .Select((arg) => arg.path);
    }
}