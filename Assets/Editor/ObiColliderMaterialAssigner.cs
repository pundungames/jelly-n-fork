using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Obi;

public class ObiColliderMaterialAssigner : EditorWindow
{
    private List<ObiCollider> obiColliders = new List<ObiCollider>();
    private ObiCollisionMaterial obiMaterial;
    private SerializedObject serializedWindow;
    private Vector2 scrollPos;

    [MenuItem("Tools/Obi Collider Material Assigner")]
    public static void ShowWindow()
    {
        var window = GetWindow<ObiColliderMaterialAssigner>("Obi Material Assigner");
        window.minSize = new Vector2(400, 300);
    }

    private void OnGUI()
    {
        EditorGUILayout.Space(10);
        GUILayout.Label("Obi Collider Material Assigner", EditorStyles.boldLabel);
        EditorGUILayout.Space(5);

        // --- Obi Material Field ---
        EditorGUI.BeginChangeCheck();
        obiMaterial = (ObiCollisionMaterial)EditorGUILayout.ObjectField(
        new GUIContent("Obi Material", "Tüm collider'lara atanacak ObiColliderMaterial"),
        obiMaterial,
        typeof(ObiCollisionMaterial),
        false
        );
        EditorGUILayout.Space(8);

        // --- ObiCollider List Header ---
        EditorGUILayout.BeginHorizontal();
        GUILayout.Label("ObiCollider Listesi", EditorStyles.boldLabel);
        if (GUILayout.Button("+", GUILayout.Width(30)))
            obiColliders.Add(null);
        if (GUILayout.Button("Temizle", GUILayout.Width(65)))
        {
            if (EditorUtility.DisplayDialog("Listeyi Temizle", "Tüm collider'lar listeden kaldırılsın mı?", "Evet", "Hayır"))
                obiColliders.Clear();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(4);

        // --- Scrollable Collider List ---
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.MaxHeight(220));
        for (int i = 0; i < obiColliders.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            obiColliders[i] = (ObiCollider)EditorGUILayout.ObjectField(
            $"[{i}]",
            obiColliders[i],
            typeof(ObiCollider),
            true
            );
            if (GUILayout.Button("✕", GUILayout.Width(28)))
            {
                obiColliders.RemoveAt(i);
                break;
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space(4);

        // --- Drag & Drop Area ---
        Rect dropArea = GUILayoutUtility.GetRect(0f, 36f, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, "↓  Buraya ObiCollider sürükle & bırak", EditorStyles.helpBox);
        HandleDragAndDrop(dropArea);

        EditorGUILayout.Space(10);

        // --- Assign Button ---
        bool canAssign = obiMaterial != null && obiColliders.Count > 0;
        GUI.enabled = canAssign;
        if (GUILayout.Button("Tüm Collider'lara Ata", GUILayout.Height(36)))
            AssignMaterialToAll();
        GUI.enabled = true;

        if (!canAssign)
        {
            string hint = obiMaterial == null && obiColliders.Count == 0
            ? "Obi Material seçin ve en az bir ObiCollider ekleyin."
            : obiMaterial == null
            ? "Bir ObiColliderMaterial seçmelisiniz."
            : "Listeye en az bir ObiCollider eklemelisiniz.";
            EditorGUILayout.HelpBox(hint, MessageType.Warning);
        }
    }

    private void HandleDragAndDrop(Rect dropArea)
    {
        Event evt = Event.current;
        if (!dropArea.Contains(evt.mousePosition)) return;

        if (evt.type == EventType.DragUpdated)
        {
            bool anyValid = false;
            foreach (var obj in DragAndDrop.objectReferences)
            {
                if (obj is GameObject go && go.GetComponent<ObiCollider>() != null)
                    anyValid = true;
                else if (obj is ObiCollider)
                    anyValid = true;
            }
            DragAndDrop.visualMode = anyValid ? DragAndDropVisualMode.Copy : DragAndDropVisualMode.Rejected;
            evt.Use();
        }
        else if (evt.type == EventType.DragPerform)
        {
            DragAndDrop.AcceptDrag();
            foreach (var obj in DragAndDrop.objectReferences)
            {
                ObiCollider col = null;
                if (obj is ObiCollider directCol)
                    col = directCol;
                else if (obj is GameObject go)
                    col = go.GetComponent<ObiCollider>();

                if (col != null && !obiColliders.Contains(col))
                    obiColliders.Add(col);
            }
            evt.Use();
        }
    }

    private void AssignMaterialToAll()
    {
        int successCount = 0;
        int skipCount = 0;

        foreach (var obiCollider in obiColliders)
        {
            if (obiCollider == null)
            {
                skipCount++;
                continue;
            }

            Undo.RecordObject(obiCollider, "Assign Obi Collider Material");

            // Obi Material ata
            obiCollider.CollisionMaterial = obiMaterial;

            // Source Collider: objedeki ilk uygun Collider bileşenini bul
            Collider sourceCol = obiCollider.GetComponent<Collider>();
            if (sourceCol != null)
            {
                SerializedObject so = new SerializedObject(obiCollider);
                SerializedProperty sourceProp = so.FindProperty("m_SourceCollider");
                if (sourceProp != null)
                {
                    sourceProp.objectReferenceValue = sourceCol;
                    so.ApplyModifiedProperties();
                }
            }

            EditorUtility.SetDirty(obiCollider);
            successCount++;
        }

        string msg = $"{successCount} ObiCollider güncellendi.";
        if (skipCount > 0) msg += $" {skipCount} boş slot atlandı.";
        Debug.Log($"[ObiColliderMaterialAssigner] {msg}");
        EditorUtility.DisplayDialog("Atama Tamamlandı", msg, "Tamam");
    }
}