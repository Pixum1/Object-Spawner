using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Object))]
public class CreateObjects : Editor
{
    [SerializeField]
    private bool lockSpawnpoints;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Object obj = (Object)target;

        #region Spawnpoints
        GUILayout.BeginHorizontal();

        if(!lockSpawnpoints)
        {
            if (GUILayout.Button("Add Spawnpoint"))
                obj.AddSpawnpoint();

            if (GUILayout.Button("Clear SpawnPoints"))
                obj.ClearSpawnPoints();
        }
        

        GUILayout.EndHorizontal();
        #endregion

        #region Objects
        GUILayout.BeginHorizontal();

        if (GUILayout.Button("Generate Objects"))
            obj.GenerateAll();

        if (GUILayout.Button("Clear Objects"))
            obj.ClearObjects();

        GUILayout.EndHorizontal ();
        #endregion

        if(!lockSpawnpoints)
            if (GUILayout.Button("Delete most recent Spawnpoint"))
                obj.DeleteLastSpawnPoint();
        

        if(!lockSpawnpoints)
            if (GUILayout.Button("Lock all Spawnpoints"))
                lockSpawnpoints = true;
        
        if (lockSpawnpoints)
            if (GUILayout.Button("Unlock Spawnpoints"))
                lockSpawnpoints = false;
    }
}
