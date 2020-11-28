using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode.Custom;
using UnityEngine;

namespace yoyohan
{
    /// <summary>
    /// 描述：
    /// 功能：
    /// 作者：yoyohan
    /// 创建时间：2020-03-26 11:36:39
    /// </summary>
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

            PBXProject project = new PBXProject();
            project.ReadFromFile(projectPath);
            string targetGuid = project.TargetGuidByName("Unity-iPhone");


            #region 修改BITCODE
            //project.SetBuildProperty(targetGuid, "ENABLE_BITCODE", "NO");
            #endregion


            #region 添加lib
            //AddLibToProject(project, targetGuid, "libz.tbd");
            //AddLibToProject(project, targetGuid, "libresolv.tbd");
            //AddLibToProject(project, targetGuid, "libsqlite3.tbd");
            #endregion


            #region 添加系统的Framework
            //project.AddFrameworkToProject(targetGuid, "CoreTelephony.framework", false);
            //project.AddFrameworkToProject(targetGuid, "SystemConfiguration.framework", false);
            //project.AddFrameworkToProject(targetGuid, "WebKit.framework", false);
            #endregion


            #region 增加Other Linker Flags
            //project.AddBuildProperty(targetGuid, "OTHER_LDFLAGS", "-ObjC");
            #endregion


            File.WriteAllText(projectPath, project.WriteToString());


            #region 添加UniversalLink
            //var capManager = new ProjectCapabilityManager(projectPath, "test.entitlements", PBXProject.GetUnityTargetName());
            //capManager.AddAssociatedDomains(new string[] { "applinks:ulwvx.share2dlink.com" });
            //capManager.AddPushNotifications(true);//包含向plist中加入aps-environment值 true代表development或false代表production
            //capManager.WriteToFile();
            #endregion


            #region 删掉plist的UIApplicationExitsOnSuspend
            //删掉info.plist的UIApplicationExitsOnSuspend 因为该属性苹果不再使用 在上传appstore报错
            string plistPath = pathToBuiltProject + "/Info.plist";
            PlistDocument plist = new PlistDocument();
            plist.ReadFromString(File.ReadAllText(plistPath));
            PlistElementDict rootDict = plist.root;
            rootDict.values.Remove("UIApplicationExitsOnSuspend");
            #endregion


            plist.WriteToFile(plistPath);


            #region 修改OC代码


            #region 代码动态设置状态栏显示隐藏
            //修改UnityViewControllerBase+iOS.mm代码：statusBar
            //XClass UnityViewControllerBase_iOS = new XClass(pathToBuiltProject + "/Classes/UI/UnityViewControllerBase+iOS.mm");
            //string statusCode = "static bool _PrefersStatusBarHidden = true;\nextern \"C\" void showStatusBar(bool isHide)\n{\n_PrefersStatusBarHidden = isHide;\n[GetAppController().rootViewController setNeedsStatusBarAppearanceUpdate];\n}\n- (BOOL) prefersStatusBarHidden\n{\n";
            //UnityViewControllerBase_iOS.Replace("- (BOOL)prefersStatusBarHidden\n{\n    static bool _PrefersStatusBarHidden = true;", statusCode);
            #endregion


            #endregion


            #region 替换OC代码文件


            #region 替换UnityAppController.mm文件
            //StreamReader streamReader = new StreamReader(Application.dataPath+ "/Scripts/Author2/Editor/UnityAppController.mm");
            //string text_all = streamReader.ReadToEnd();
            //streamReader.Close();

            //StreamWriter streamWriter = new StreamWriter(pathToBuiltProject + "/Classes/UnityAppController.mm");
            //streamWriter.Write(text_all);
            //streamWriter.Close();
            #endregion


            #endregion


        }


        static void AddLibToProject(PBXProject inst, string targetGuid, string lib)
        {
            string fileGuid = inst.AddFile("usr/lib/" + lib, "Frameworks/" + lib, PBXSourceTree.Sdk);
            inst.AddFileToBuild(targetGuid, fileGuid);
        }


    }
}
