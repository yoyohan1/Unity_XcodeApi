using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode.Custom;

public class XCodeApi
{

    [PostProcessBuildAttribute(1)]
    public static void OnPostProcessBuild(BuildTarget buildTarget, string pathToBuiltProject)
    {
        // 只处理IOS工程， pathToBuildProject会传入导出的ios工程的根目录
        if (buildTarget != BuildTarget.iOS)
            return;

        // 创建工程设置对象
        var projectPath = pathToBuiltProject + "/Unity-iPhone.xcodeproj/project.pbxproj";
        var capManager = new ProjectCapabilityManager(projectPath, "test.entitlements", PBXProject.GetUnityTargetName());
        capManager.AddAssociatedDomains(new string[] { "webcredentials:example.com"});
        capManager.WriteToFile();
    }
}
