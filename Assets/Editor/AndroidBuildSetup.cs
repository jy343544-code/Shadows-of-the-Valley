using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class AndroidBuildSetup
{
    private const string ScenePath = "Assets/Scenes/BlackGate_Prototype.unity";

    [MenuItem("Shadows of the Valley/Generate Prototype Scene")]
    public static void GeneratePrototypeScene()
    {
        Directory.CreateDirectory("Assets/Scenes");
        Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(0.34f, 0.31f, 0.27f);
        RenderSettings.fog = true;
        RenderSettings.fogColor = new Color(0.48f, 0.39f, 0.29f);
        RenderSettings.fogDensity = 0.012f;

        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Black Gate - Sandstone Arena";
        ground.transform.localScale = new Vector3(5f, 1f, 5f);
        ground.GetComponent<Renderer>().sharedMaterial = MakeMaterial(new Color(0.48f, 0.37f, 0.25f));

        CreatePillar(new Vector3(-8f, 2f, 7f));
        CreatePillar(new Vector3(8f, 2f, 7f));
        CreatePillar(new Vector3(-8f, 2f, -7f));
        CreatePillar(new Vector3(8f, 2f, -7f));

        GameObject player = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        player.name = "Sekhem";
        player.transform.position = new Vector3(0f, 1f, -4f);
        player.GetComponent<Collider>().enabled = false;
        CharacterController controller = player.AddComponent<CharacterController>();
        controller.height = 1.8f;
        controller.radius = 0.35f;
        controller.center = new Vector3(0f, 0.9f, 0f);
        player.AddComponent<PlayerController>();
        player.AddComponent<PlayerHealth>();
        player.AddComponent<PlayerCombat>();
        player.AddComponent<AirComboCombat>();
        player.AddComponent<ShadowTether>();
        player.AddComponent<TargetLock>();
        player.GetComponent<Renderer>().sharedMaterial = MakeMaterial(new Color(0.12f, 0.17f, 0.19f));

        GameObject cameraObject = new GameObject("Main Camera");
        cameraObject.tag = "MainCamera";
        Camera camera = cameraObject.AddComponent<Camera>();
        camera.fieldOfView = 65f;
        cameraObject.AddComponent<AudioListener>();
        ThirdPersonCamera follow = cameraObject.AddComponent<ThirdPersonCamera>();
        SetObjectReference(follow, "target", player.transform);
        SetObjectReference(player.GetComponent<PlayerController>(), "cameraTransform", cameraObject.transform);
        SetObjectReference(player.GetComponent<TargetLock>(), "cameraTransform", cameraObject.transform);

        CreateEnemy("Tombbound_A", new Vector3(-3f, 1f, 3f), player.transform, 80, new Color(0.36f, 0.31f, 0.25f));
        CreateEnemy("Tombbound_B", new Vector3(3f, 1f, 5f), player.transform, 80, new Color(0.36f, 0.31f, 0.25f));

        GameObject boss = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        boss.name = "The Gatekeeper";
        boss.transform.position = new Vector3(0f, 1.2f, 12f);
        boss.transform.localScale = new Vector3(2.2f, 1.2f, 2.2f);
        boss.GetComponent<Renderer>().sharedMaterial = MakeMaterial(new Color(0.16f, 0.14f, 0.13f));
        EnemyHealth bossHealth = boss.AddComponent<EnemyHealth>();
        SetInt(bossHealth, "maxHealth", 600);
        SetBool(bossHealth, "destroyOnDefeat", false);
        GatekeeperBossAI bossAI = boss.AddComponent<GatekeeperBossAI>();
        SetObjectReference(bossAI, "target", player.transform);
        SetLayerMask(bossAI, "playerLayers", ~0);

        GameObject sun = new GameObject("Desert Sun");
        Light light = sun.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1.25f;
        sun.transform.rotation = Quaternion.Euler(48f, -32f, 0f);

        GameObject fill = new GameObject("Blue Shadow Fill");
        Light fillLight = fill.AddComponent<Light>();
        fillLight.type = LightType.Point;
        fillLight.color = new Color(0.12f, 0.55f, 0.62f);
        fillLight.intensity = 5f;
        fillLight.range = 12f;
        fill.transform.position = new Vector3(0f, 3f, 0f);

        EditorSceneManager.MarkSceneDirty(scene);
        EditorSceneManager.SaveScene(scene, ScenePath);
        EditorBuildSettings.scenes = new[] { new EditorBuildSettingsScene(ScenePath, true) };
        AssetDatabase.SaveAssets();
        Debug.Log("BlackGate_Prototype scene generated.");
    }

    private static void CreateEnemy(string name, Vector3 position, Transform player, int hp, Color color)
    {
        GameObject enemy = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        enemy.name = name;
        enemy.transform.position = position;
        enemy.GetComponent<Renderer>().sharedMaterial = MakeMaterial(color);
        Rigidbody body = enemy.AddComponent<Rigidbody>();
        body.mass = 2f;
        body.interpolation = RigidbodyInterpolation.Interpolate;
        body.collisionDetectionMode = CollisionDetectionMode.Continuous;
        body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        EnemyHealth health = enemy.AddComponent<EnemyHealth>();
        SetInt(health, "maxHealth", hp);
        MonsterChaser chaser = enemy.AddComponent<MonsterChaser>();
        SetObjectReference(chaser, "target", player);
        SetFloat(chaser, "moveSpeed", 2f);
    }

    private static void CreatePillar(Vector3 position)
    {
        GameObject pillar = GameObject.CreatePrimitive(PrimitiveType.Cube);
        pillar.name = "Eroded Temple Pillar";
        pillar.transform.position = position;
        pillar.transform.localScale = new Vector3(1.4f, 4f, 1.4f);
        pillar.GetComponent<Renderer>().sharedMaterial = MakeMaterial(new Color(0.42f, 0.34f, 0.26f));
    }

    private static Material MakeMaterial(Color color)
    {
        Shader shader = Shader.Find("Universal Render Pipeline/Lit");
        if (shader == null) shader = Shader.Find("Standard");
        Material material = new Material(shader);
        material.color = color;
        return material;
    }

    private static void SetObjectReference(Object target, string property, Object value)
    {
        SerializedObject serialized = new SerializedObject(target);
        SerializedProperty field = serialized.FindProperty(property);
        if (field != null) { field.objectReferenceValue = value; serialized.ApplyModifiedPropertiesWithoutUndo(); }
    }

    private static void SetInt(Object target, string property, int value)
    {
        SerializedObject serialized = new SerializedObject(target);
        SerializedProperty field = serialized.FindProperty(property);
        if (field != null) { field.intValue = value; serialized.ApplyModifiedPropertiesWithoutUndo(); }
    }

    private static void SetBool(Object target, string property, bool value)
    {
        SerializedObject serialized = new SerializedObject(target);
        SerializedProperty field = serialized.FindProperty(property);
        if (field != null) { field.boolValue = value; serialized.ApplyModifiedPropertiesWithoutUndo(); }
    }

    private static void SetFloat(Object target, string property, float value)
    {
        SerializedObject serialized = new SerializedObject(target);
        SerializedProperty field = serialized.FindProperty(property);
        if (field != null) { field.floatValue = value; serialized.ApplyModifiedPropertiesWithoutUndo(); }
    }

    private static void SetLayerMask(Object target, string property, int value)
    {
        SerializedObject serialized = new SerializedObject(target);
        SerializedProperty field = serialized.FindProperty(property);
        if (field != null) { field.intValue = value; serialized.ApplyModifiedPropertiesWithoutUndo(); }
    }

    [MenuItem("Shadows of the Valley/Build Android APK")]
    public static void BuildAndroid()
    {
        if (!File.Exists(ScenePath)) GeneratePrototypeScene();
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Android, BuildTarget.Android);
        PlayerSettings.companyName = "Zoksh Studio";
        PlayerSettings.productName = "Shadows of the Valley";
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, "com.zoksh.shadowsofthevalley");
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
        PlayerSettings.SetScriptingBackend(NamedBuildTarget.Android, ScriptingImplementation.IL2CPP);

        Directory.CreateDirectory("build/Android");
        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = new[] { ScenePath },
            locationPathName = "build/Android/ShadowsOfTheValley.apk",
            target = BuildTarget.Android,
            options = BuildOptions.None
        };

        BuildReport report = BuildPipeline.BuildPlayer(options);
        if (report.summary.result != BuildResult.Succeeded)
            throw new System.Exception("Android build failed: " + report.summary.result);
        Debug.Log("APK created: " + options.locationPathName);
    }
}
