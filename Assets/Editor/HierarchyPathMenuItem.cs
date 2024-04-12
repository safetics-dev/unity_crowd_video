using UnityEditor;
using UnityEngine;
public class HierarchyPathMenuItem
{ 
    // 경로 
    private const string HIERARCHY_MENU_NAME = "GameObject/CopyToPath"; 
    private const string PROJECT_MENU_NAME = "Assets/CopyToPath"; 
    // Hierarchy 창에서의 특정 gameObject의 경로를 클립보드에 저장한다. 
    [MenuItem(HIERARCHY_MENU_NAME, false, -10)] 
    static void MainCopyToPath() 
    { 
        if (Selection.activeGameObject != null) HierarchyPathViewWindow.CopyToClipboard(HierarchyPathViewWindow.GetHiFullPath(Selection.activeGameObject.transform));
    }
    //[MenuItem(HIERARCHY_MENU_NAME, false, 10)] 
    //static bool CopyToPath() //{ // return Selection.activeGameObject != null; //} // Asset Project 창에서 특정 파일의 경로를 저장한다. 
    [MenuItem(PROJECT_MENU_NAME)] 
    static void MainCopyToProjectPath() 
    {
        HierarchyPathViewWindow.CopyToClipboard( AssetDatabase.GetAssetPath(Selection.activeObject)); 
    } 
    [MenuItem(PROJECT_MENU_NAME, true, -10)] 
    static bool CopyToProjectPath() 
    {
        return Selection.activeObject != null; 
    }
}