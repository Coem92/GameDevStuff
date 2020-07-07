using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.PackageManager.UI;

public class TextureArrayTool : EditorWindow
{
    // variables
    List<Texture2D> textures = new List<Texture2D>();
    Vector2 scrollPosition;
    Rect scrollArea;
    Texture2D dragTexture;
    bool ErrorSize  = false;
    TextureFormat textureFormat;
    FilterMode filtermode;
    bool usefristFormat = false;
    //texture array variables

    string arrayName;
    int currentIndex = 0;

    [MenuItem("Window/Texture Array Tool")]
    public static void ShowWindow()
    {

        GetWindow<TextureArrayTool>("Texture Array Tool");
    }
    private void Awake()
    {
        textures.Add(null);
    }


    private void OnGUI()
    {
        if (Event.current.type == EventType.DragUpdated)
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;

            Event.current.Use();
        }
        else if (Event.current.type == EventType.DragPerform)
        {
            textures.RemoveAt(textures.Count - 1);
            for (int i = 0; i < DragAndDrop.objectReferences.Length; i++)
            {
                if (DragAndDrop.objectReferences[i].GetType() == typeof(Texture2D))
                {
                    textures.Add(DragAndDrop.objectReferences[i] as Texture2D);
                }
                if (DragAndDrop.objectReferences[i].GetType() == typeof(Texture2DArray))
                {
                    loadArray(DragAndDrop.objectReferences[i] as Texture2DArray);
                }
            }
            Event.current.Use();

        }

        GUILayout.BeginHorizontal();
        //scroll area
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(138));
        ErrorSize = false;
        for (int i = 0; i < textures.Count; i++)
        {
            EditorGUILayout.BeginHorizontal("box");
            textures[i] = drawTextureField(textures[i]);
            switchElements(textures, i);
            EditorGUILayout.EndHorizontal();

            if (i < textures.Count - 1)
            {
                if (textures[i] != null && textures[i + 1] != null)
                {
                    if (textures[i].width != textures[i + 1].width || textures[i].height != textures[i + 1].height)
                    {
                        ErrorSize = true;
                    }
                }
            }
        }
        if (textures[textures.Count - 1] != null)
        {
            textures.Add(null);
        }
        GUILayout.EndScrollView();
        //options area
        GUILayout.BeginVertical("box", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        arrayName = TextField(arrayName, "Type name...");
        var style = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleCenter };
        GUILayout.Label(textures[currentIndex], style);// GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
        GUILayout.BeginHorizontal();
        if(currentIndex > textures.Count - 1)
        {
            currentIndex = textures.Count - 1;
        }
        if(currentIndex == 0)
        {
            GUI.enabled = false;
        }
        if (GUILayout.Button("←", GUILayout.Width(24)))
        {
            
            currentIndex--;
        }
        GUI.enabled = true;
        if (textures[currentIndex] != null)
        {
            GUILayout.Label(textures[currentIndex].name);
        }
        if (currentIndex >= textures.Count-2)
        {
            GUI.enabled = false;
        }
        if (GUILayout.Button("→", GUILayout.Width(24)))
        {

            currentIndex++;
        }
        GUI.enabled = true;
        GUILayout.EndHorizontal();

        usefristFormat = EditorGUILayout.Toggle("Use frist texture format", usefristFormat);
        if (usefristFormat)
        {
            textureFormat = textures[0].format;
            GUI.enabled = false;
        }
        textureFormat = (TextureFormat)EditorGUILayout.EnumPopup("Texture format", textureFormat);
        GUI.enabled = true;
        filtermode = (FilterMode)EditorGUILayout.EnumPopup("Texture filtermode", filtermode);
        if (ErrorSize)
        {
            GUI.enabled = false;
        }
        if (GUILayout.Button("Create Texture Array"))
        {
            
            CreatArray();
        }
        GUI.enabled = true;
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();


    }

    Texture2D drawTextureField(Texture2D t)
    {

        EditorGUILayout.BeginVertical("box");
        t = EditorGUILayout.ObjectField(t, typeof(Texture2D), GUILayout.Width(64), GUILayout.Height(64), GUILayout.ExpandWidth(false)) as Texture2D;
        if (t != null)
        {
            GUILayout.Label(t.name, GUILayout.ExpandWidth(false));
        }
        else
        {
            GUILayout.Label("Add Texture", GUILayout.MaxWidth(64));
        }
        EditorGUILayout.EndVertical();

        return t;
    }

    void switchElements(List<Texture2D> list, int index)
    {
        EditorGUILayout.BeginVertical("box");
        if (index == 0 || index == list.Count - 1)
        {
            GUI.enabled = false;
        }
        if (GUILayout.Button("↑", GUILayout.Width(24)))
        {
            Texture2D tmp = list[index];
            list[index] = list[index - 1];
            list[index - 1] = tmp;
        }
        GUI.enabled = true;
        if (index == list.Count - 1 || index == list.Count - 2)
        {
            GUI.enabled = false;
        }
        if (GUILayout.Button("↓", GUILayout.Width(24)))
        {
            Texture2D tmp = list[index];
            list[index] = list[index + 1];
            list[index + 1] = tmp;
        }
        GUI.enabled = true;
        if (index == list.Count - 1)
        {
            GUI.enabled = false;
        }
        if (GUILayout.Button("x", GUILayout.Width(24)))
        {
            list.RemoveAt(index);
        }
        GUI.enabled = true;

        if (index != list.Count - 1)
        {
            GUILayout.Label(index + "");
        }
        EditorGUILayout.EndVertical();
    }

    string TextField(string text, string placeholder)
    {
        var newText = EditorGUILayout.TextField(text);
        if (string.IsNullOrEmpty(text))
        {
            var guiColor = GUI.color;
            GUI.color = Color.gray;
            EditorGUI.LabelField(GUILayoutUtility.GetLastRect(), placeholder);
            GUI.color = guiColor;
        }
        return newText;
    }

    void CreatArray()
    {
        
        Texture2DArray textureArray = new Texture2DArray(textures[0].width, textures[0].height, textures.Count-1, textureFormat, false);
        for (int i = 0; i <= textures.Count-1; i++)
        {
            if (textures[i] != null)
            {
                textureArray.SetPixels(textures[i].GetPixels(0), i, 0);
            }
        }
        textureArray.filterMode = filtermode;
        textureArray.Apply();
       // Debug.Log(textures[0].name);
        string path = "Assets/"+arrayName+".asset";
        AssetDatabase.CreateAsset(textureArray, path);
    }

    void loadArray(Texture2DArray textureArray)
    {
        for (int i = 0; i <= textureArray.depth - 1; i++)
        {
            Texture2D t = new Texture2D(textureArray.width, textureArray.height);
            t.SetPixels(textureArray.GetPixels(i));
            t.Apply();
            textures.Add(t);
        }
    }
}
