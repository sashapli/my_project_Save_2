using System;
using System.Collections;
using UnityEngine;

public class ZeusBranch : MonoBehaviour
{
    [Header("Line generation")]
    public int pointsCount;
    public float freq;
    public float lac;
    [Range(0f, 1f)] public float pers;
    public float strength;
    public int octaves;
    public Vector2 offset;
    public Vector2 linePosStart;
    public Vector2 linePosEnd;
    public float posZ;
    public float lineWidth;

    private LineRenderer lineRend;
    private Vector3[] linePoints;

    [Header("Branch")]
    public int seed;
    public int level;

    [Header("Glow / Sky")]
    [Range(0f, 1f)] public float alpha = 1f;
    public Color colorA = Color.white;
    public Color colorB = Color.black;
    [Range(0f, 2f)] public float slide = 0.5f;
    [Range(0f, 20f)] public float multiply = 6f;
    public Texture2D moonTex;

    private Material material;
    private MaterialPropertyBlock propBlock;
    private MaterialPropertyBlock glowPropBlock;
    private Renderer rend;

    public MeshRenderer glowMeshRend;
    public Transform glowTransform;

    private Mesh mesh;
    private Vector3[] meshPoints;
    private int[] triangles;
    private Vector2[] uv;
    private Vector3[] normals;

    private Animator anim;
    private System.Random sysRand;
    private AudioSource audioSource;
    private float audioVolume = 1f;
    private float startOffsetX;

    // Хэши свойств шейдера (быстрее и надёжнее, чем строки каждый кадр)
    private static readonly int AlphaID = Shader.PropertyToID("_Alpha");
    private static readonly int ColorAID = Shader.PropertyToID("_ColorA");
    private static readonly int ColorBID = Shader.PropertyToID("_ColorB");
    private static readonly int SlideID = Shader.PropertyToID("_Slide");
    private static readonly int MultiplyID = Shader.PropertyToID("_Multiply");
    private static readonly int MoonTexID = Shader.PropertyToID("_MoonTex");

    public void Awake()
    {
        lineRend = GetComponent<LineRenderer>();
        rend = GetComponent<MeshRenderer>();
        if (rend != null)
        {
            material = rend.material;
        }
        propBlock = new MaterialPropertyBlock();

        mesh = new Mesh();
        anim = GetComponent<Animator>();

        if (glowMeshRend != null)
        {
            glowPropBlock = new MaterialPropertyBlock();
            glowMeshRend.GetPropertyBlock(glowPropBlock);
        }
    }

    private void Start()
    {
        if (ZeusStrike.instance != null)
        {
            sysRand = ZeusStrike.instance.sysRand;
        }
        else
        {
            sysRand = new System.Random();
        }
    }

    private void Update()
    {
        // Alpha для самой молнии
        if (rend != null)
        {
            propBlock.SetFloat(AlphaID, alpha);
            rend.SetPropertyBlock(propBlock);
        }

        // Alpha и остальные параметры для glow/неба
        if (glowMeshRend != null)
        {
            if (glowPropBlock == null)
            {
                glowPropBlock = new MaterialPropertyBlock();
                glowMeshRend.GetPropertyBlock(glowPropBlock);
            }

            glowPropBlock.SetFloat(AlphaID, alpha);
            glowPropBlock.SetColor(ColorAID, colorA);
            glowPropBlock.SetColor(ColorBID, colorB);
            glowPropBlock.SetFloat(SlideID, slide);
            glowPropBlock.SetFloat(MultiplyID, multiply);

            if (moonTex != null)
            {
                glowPropBlock.SetTexture(MoonTexID, moonTex);
            }

            glowMeshRend.SetPropertyBlock(glowPropBlock);
        }
    }

    public void GenerateLine(int _level, Vector3 _startPos, Vector3 _endPos)
    {
        if (level == 0)
        {
            if (audioSource == null)
            {
                audioSource = base.gameObject.AddComponent<AudioSource>();
                if (AmbienceAudioManager.instance != null)
                {
                    audioSource.volume = audioVolume * AmbienceAudioManager.instance.effectsVolume;
                }
                else
                {
                    audioSource.volume = audioVolume;
                }
            }

            if (ZeusStrike.instance != null &&
                ZeusStrike.instance.thunderAudio != null &&
                ZeusStrike.instance.thunderAudio.Length > 0)
            {
                int idx = UnityEngine.Random.Range(0, ZeusStrike.instance.thunderAudio.Length);
                audioSource.clip = ZeusStrike.instance.thunderAudio[idx];
                audioSource.PlayDelayed(1.2f);
            }
        }

        if (glowTransform != null)
        {
            glowTransform.position = new Vector3(_startPos.x, _startPos.y, posZ);
        }

        if (anim != null)
        {
            anim.Play("zeusBranch");
        }

        // Наследуем параметры от префаба
        if (ZeusStrike.instance != null && ZeusStrike.instance.zeusBranchPrefab != null)
        {
            ZeusBranch prefabBranch = ZeusStrike.instance.zeusBranchPrefab.GetComponent<ZeusBranch>();
            if (prefabBranch != null)
            {
                strength = prefabBranch.strength;
                lineWidth = prefabBranch.lineWidth;

                // Наследуем параметры шейдера, если они не заданы вручную
                colorA = prefabBranch.colorA;
                colorB = prefabBranch.colorB;
                slide = prefabBranch.slide;
                multiply = prefabBranch.multiply;
                moonTex = prefabBranch.moonTex;
            }
        }

        level = _level;
        lineWidth /= Mathf.Pow(2f, _level);
        lineRend.startWidth = lineWidth;
        lineRend.endWidth = lineWidth;
        lineRend.positionCount = pointsCount;

        linePoints = new Vector3[pointsCount];

        if (ZeusStrike.instance != null && ZeusStrike.instance.sysRand != null)
        {
            offset = new Vector2(
                (float)ZeusStrike.instance.sysRand.NextDouble() * 1000f,
                (float)ZeusStrike.instance.sysRand.NextDouble() * 1000f);
        }
        else
        {
            offset = new Vector2(
                (float)UnityEngine.Random.value * 1000f,
                (float)UnityEngine.Random.value * 1000f);
        }

        Vector2 point = offset;
        float num = 1f / (float)pointsCount;

        for (int i = 0; i < pointsCount; i++)
        {
            linePoints[i] = default(Vector3);
            linePoints[i].x = Mathf.Lerp(_startPos.x, _endPos.x, (float)i * num);

            float num2 = Noise.Sum(point, freq, octaves, lac, pers, NoiseType.Ridged) * strength;
            linePoints[i].x += num2;

            if (i == 0)
            {
                startOffsetX = num2;
            }
            linePoints[i].x -= startOffsetX;

            linePoints[i].y = Mathf.Lerp(_startPos.y, _endPos.y, (float)i * num);
            linePoints[i].z = posZ;

            point.x += 0.1f;
        }

        lineRend.SetPositions(linePoints);
        GenerateMesh();

        linePosStart = _startPos;
        linePosEnd = _endPos;

        if (ZeusStrike.instance != null && _level != ZeusStrike.instance.zeusLevels.Length)
        {
            StartCoroutine(InvokeMethod(ZeusStrike.instance.branchGenerationDelay + UnityEngine.Random.Range(0f, 0.01f)));
        }
    }

    private void DefineBranches()
    {
        if (ZeusStrike.instance == null) return;

        for (int i = 0; i < ZeusStrike.instance.zeusLevels[level].branchesCount; i++)
        {
            GameObject go = ObjectPool.instance.ReuseObject(ZeusStrike.instance.zeusBranchPrefab).go;
            ZeusBranch component = go.GetComponent<ZeusBranch>();
            component.strength /= Mathf.Pow(4f, level + 1);

            System.Random random = ZeusStrike.instance.sysRand;
            int num = random.Next(0, linePoints.Length - 1);
            Vector3 vector = linePoints[num];
            Vector3 endPos = vector + new Vector3(
                (float)random.NextDouble() * 400f - 200f,
                (float)random.NextDouble() * 0f - 200f);

            component.GenerateLine(level + 1, vector, endPos);
        }
    }

    private IEnumerator InvokeMethod(float _time)
    {
        yield return new WaitForSeconds(_time);
        DefineBranches();
    }

    private void GenerateMesh()
    {
        meshPoints = new Vector3[pointsCount * 2];
        triangles = new int[(pointsCount - 1) * 6];
        uv = new Vector2[meshPoints.Length];
        normals = new Vector3[pointsCount];

        int i = 0;
        int num = 0;
        for (; i < pointsCount; i++)
        {
            int num2 = 0;
            while (num2 < 2)
            {
                meshPoints[num] = linePoints[i];
                float num3 = Mathf.Lerp(lineWidth, lineWidth / 2f, (float)i * (1f / (float)pointsCount));
                meshPoints[num].x += (0f - num3) / 2f + num3 * (float)num2;
                uv[num] = new Vector2(num2, (float)i * (1f / (float)pointsCount));
                num2++;
                num++;
            }
        }

        normals[0] = Vector3.zero;
        for (int j = 1; j < pointsCount; j++)
        {
            normals[j] = (meshPoints[j * 2] - meshPoints[j * 2 - 2]).normalized;
        }

        int num4 = 0;
        int num5 = 0;
        int num6 = 0;
        while (num4 < pointsCount - 1)
        {
            triangles[num5] = num6;
            triangles[num5 + 1] = num6 + 1;
            triangles[num5 + 2] = num6 + 2;
            triangles[num5 + 3] = num6 + 2;
            triangles[num5 + 4] = num6 + 1;
            triangles[num5 + 5] = num6 + 2 + 1;

            num4++;
            num5 += 6;
            num6 += 2;
        }

        mesh.vertices = meshPoints;
        mesh.triangles = triangles;
        mesh.uv = uv;

        MeshFilter mf = GetComponent<MeshFilter>();
        if (mf != null)
        {
            mf.mesh = mesh;
        }
    }
}