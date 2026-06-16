using UnityEditor;
using UnityEngine;

public static class AudioAssigner
{
    [MenuItem("Tools/Assign Audio Clips")]
    public static void AssignAudioClips()
    {
        AudioClip gunshot      = Load("gunshot");
        AudioClip revolver     = Load("revolver2");
        AudioClip wingflap     = Load("wingflap");
        AudioClip reloadstart  = Load("reloadstart");
        AudioClip playerdeath  = Load("playerdeath");
        AudioClip hurt         = Load("hurt");
        AudioClip death        = Load("death");
        AudioClip teleport     = Load("teleport");
        AudioClip playerhurt   = Load("playerhurt");
        AudioClip explosion    = Load("explosion2");
        AudioClip plink        = Load("plink");
        AudioClip music        = Load("orange_game_stuff");

        // --- Player (scene object) ---
        Player player = Object.FindFirstObjectByType<Player>();
        if (player != null)
        {
            player.GunshotSFX    = gunshot;
            player.FlapWingsSFX  = wingflap;
            player.ReloadSFX     = reloadstart;
            player.DeathSFX      = playerdeath;
            player.Music         = music;
            EditorUtility.SetDirty(player);
            Debug.Log("Player audio assigned.");
        }
        else Debug.LogWarning("Player not found in scene.");

        // --- HealthBar (scene object) ---
        HealthBar healthBar = Object.FindFirstObjectByType<HealthBar>();
        if (healthBar != null)
        {
            healthBar.HurtSFX = playerhurt;
            EditorUtility.SetDirty(healthBar);
            Debug.Log("HealthBar audio assigned.");
        }
        else Debug.LogWarning("HealthBar not found in scene.");

        // --- AmmoBar (scene object) ---
        AmmoBar ammoBar = Object.FindFirstObjectByType<AmmoBar>();
        if (ammoBar != null)
        {
            ammoBar.HitSFX = plink;
            EditorUtility.SetDirty(ammoBar);
            Debug.Log("AmmoBar audio assigned.");
        }
        else Debug.LogWarning("AmmoBar not found in scene.");

        // --- DestructableTilemap (scene object) ---
        DestructableTilemap tilemap = Object.FindFirstObjectByType<DestructableTilemap>();
        if (tilemap != null)
        {
            tilemap.DestructionSFX = explosion;
            EditorUtility.SetDirty(tilemap);
            Debug.Log("DestructableTilemap audio assigned.");
        }
        else Debug.LogWarning("DestructableTilemap not found in scene.");

        // --- Enemy prefab ---
        AssignEnemyPrefabs(teleport, hurt, death);

        AssetDatabase.SaveAssets();
        Debug.Log("Audio assignment complete.");
    }

    private static void AssignEnemyPrefabs(AudioClip spawnSFX, AudioClip hurtSFX, AudioClip deathSFX)
    {
        string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { "Assets" });
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null) continue;

            bool dirty = false;

            Enemy enemy = prefab.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.SpawnSFX = spawnSFX;
                dirty = true;
            }

            EnemyHealth enemyHealth = prefab.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.HurtSFX  = hurtSFX;
                enemyHealth.DeathSFX = deathSFX;
                dirty = true;
            }

            if (dirty)
            {
                EditorUtility.SetDirty(prefab);
                PrefabUtility.SavePrefabAsset(prefab);
                Debug.Log($"Enemy prefab audio assigned: {path}");
            }
        }
    }

    private static AudioClip Load(string name)
    {
        string[] guids = AssetDatabase.FindAssets($"{name} t:AudioClip", new[] { "Assets/Audio" });
        if (guids.Length == 0)
        {
            Debug.LogWarning($"Audio clip not found: {name}");
            return null;
        }
        return AssetDatabase.LoadAssetAtPath<AudioClip>(AssetDatabase.GUIDToAssetPath(guids[0]));
    }
}
