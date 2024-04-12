using System;
using UnityEngine;
using UnityEditor;

class HierarchyPathViewWindow : EditorWindow
{
    private SerializedObject obj;
    private const string TOPOBJECT_NAME = "ESTool";
    private const string HIPATH_NAME = "HierarchyPath View";
    private const string COPY_HAN = "복사"; // 선택한 게임 오브젝트 
    //public static Object selectObj = null; // 윈도우 설정 초기화 
    [MenuItem("Window/ESTool/HierarchyPath View")]
    internal static void Init()
    {
        var window = (HierarchyPathViewWindow)GetWindow(typeof(HierarchyPathViewWindow), false, "HiPath View"); window.position = new Rect(window.position.xMin + 100f, window.position.yMin + 100f, 200f, 400f);
    }

    void OnGUI()
    {
        EditorGUILayout.LabelField("선택한 Object", EditorStyles.boldLabel); EditorGUILayout.Space();
        if (Selection.activeObject != null)
        {
            // 이름 
            CreateSelectLabel("Name ", Selection.activeObject.name);
            // type 
            var type = Selection.activeObject.GetType().ToString();
            CreateSelectLabel("Type", type);
            // 프리팹인지 아닌지 체크 
            if (PrefabUtility.GetPrefabParent(Selection.activeObject))
            {
                // Prefab이면 Project 창에서의 경로 표시 
                CreateSelectLabel("Prefab Path", AssetDatabase.GetAssetPath(PrefabUtility.GetPrefabParent(Selection.activeObject)), true, COPY_HAN, new System.Action<string>((string str) =>
                {
                    CopyToClipboard(str);
                }));
                CreateSelectLabel("Hierarchy Path", GetHiFullPath(Selection.activeGameObject.transform), true, COPY_HAN, new System.Action<string>((string str) =>
                {
                    CopyToClipboard(str);
                }));
            }
            // Hierarchy 창에 존재하는 object 인지 체크 
            else if (Selection.activeTransform)
            {
                CreateSelectLabel("Hierarchy Path", GetHiFullPath(Selection.activeGameObject.transform), true, COPY_HAN, new System.Action<string>((string str) =>
                {
                    CopyToClipboard(str);
                }));
            }
            else
            {
                CreateSelectLabel("Project Path", AssetDatabase.GetAssetPath(Selection.activeObject), true, COPY_HAN, new System.Action<string>((string str) =>
                {
                    CopyToClipboard(str);
                }));
            }
        }
        else
        {
            return;
        }
    }
    // Hoerarchy Object 클릭 
    public void OnSelectionChange()
    {
        if (!Selection.activeObject)
        {
            return;
        }
        Repaint();
    }

    /// <summary> 
    /// Selectlabel 텍스트 제작 1개 
    /// </summary> 
    private void CreateSelectLabel(string title, string content, bool btnCreate = false, string btnName = "", System.Action<string> a = null)
    {
        //EditorGUILayout.BeginHorizontal(); 
        EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
        // 높이 재수정 
        Rect position = EditorGUILayout.GetControlRect(false, 15f); EditorGUI.SelectableLabel(position, string.Format("{0}", content)); if (btnCreate)
        {
            // 클립보드에 복사 
            if (GUILayout.Button(btnName))
            {
                a(content);
            }

        }
        EditorGUILayout.Space();
        //EditorGUILayout.EndHorizontal(); 
    }
    // 하이라키창에서 오브젝트 전체 경로 얻어오기
    public static string GetHiFullPath(Transform trans)
    {
        string path = "/" + trans.name; while (trans.transform.parent != null)
        {
            trans = trans.parent; path = "/" + trans.name + path;
        }
        path = path.Substring(1); return path;
    }
    // 클립보드에다가 복사하기 
    public static void CopyToClipboard(string str)
    {
        var textEditor = new TextEditor();
        textEditor.text = str; textEditor.SelectAll();
        textEditor.Copy();
        Debug.Log("Clipboard Copy : " + str);
    }
}