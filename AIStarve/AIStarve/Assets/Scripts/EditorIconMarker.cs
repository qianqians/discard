using UnityEngine;

public class EditorIconMarker : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private Sprite iconSprite;
    [SerializeField] private Vector3 offset = Vector3.zero;
    [SerializeField, Min(0.1f)] private float worldSize = 1f;

    private static Mesh _quad;
    private static Material _mat;

    private static Mesh Quad
    {
        get
        {
            if (_quad == null)
            {
                _quad = new Mesh();
                _quad.vertices = new Vector3[]
                {
                    new Vector3(-0.5f, -0.5f, 0),
                    new Vector3( 0.5f, -0.5f, 0),
                    new Vector3(-0.5f,  0.5f, 0),
                    new Vector3( 0.5f,  0.5f, 0),
                };
                _quad.uv = new Vector2[]
                {
                    new Vector2(0, 0),
                    new Vector2(1, 0),
                    new Vector2(0, 1),
                    new Vector2(1, 1),
                };
                _quad.triangles = new int[] { 0, 2, 1, 1, 2, 3 };
            }
            return _quad;
        }
    }

    private static Material Mat
    {
        get
        {
            if (_mat == null)
            {
                var shader = Shader.Find("Unlit/Texture");
                if (shader == null)
                    shader = Shader.Find("Hidden/Internal-GUITextureClip");
                _mat = new Material(shader);
                _mat.hideFlags = HideFlags.HideAndDontSave;
            }
            return _mat;
        }
    }

    private void OnDrawGizmos()
    {
        DrawIcon();
    }

    private void OnDrawGizmosSelected()
    {
        DrawIcon();
    }

    private void DrawIcon()
    {
        Sprite sprite = iconSprite;
        if (sprite == null)
        {
            Gizmos.DrawIcon(transform.position + offset, "sv_icon_dot0_sml", true);
            return;
        }

        Camera cam = UnityEditor.SceneView.lastActiveSceneView?.camera;
        if (cam == null) return;

        Vector3 pos = transform.position + offset;

        float aspect = sprite.textureRect.width / sprite.textureRect.height;
        float w = worldSize * aspect;
        float h = worldSize;

        Matrix4x4 m = Matrix4x4.TRS(pos, cam.transform.rotation, new Vector3(w, h, 1));

        Rect r = sprite.textureRect;
        Vector2 texSize = new Vector2(sprite.texture.width, sprite.texture.height);
        Mat.mainTexture = sprite.texture;
        Mat.SetTextureScale("_MainTex", new Vector2(r.width / texSize.x, r.height / texSize.y));
        Mat.SetTextureOffset("_MainTex", new Vector2(r.x / texSize.x, r.y / texSize.y));

        Mat.SetPass(0);
        Graphics.DrawMeshNow(Quad, m);
    }
#endif
}
