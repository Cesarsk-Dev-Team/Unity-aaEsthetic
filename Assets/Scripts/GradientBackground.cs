using UnityEngine;

public class GradientBackground : MonoBehaviour
{
    public static GradientBackground instance;
    public Color topColor = Color.blue;
    public Color bottomColor = Color.white;
    public int gradientLayer = 7;
    Mesh mesh;
    public Material mat;

    private void Update()
    {
        mesh.colors = new Color[4] { topColor, topColor, bottomColor, bottomColor };
    }

    private void Awake()
    {
        //Singleton D.P.
        if (instance != null) Destroy(gameObject);
        else instance = this;
    }

    void Start()
    {
        gradientLayer = Mathf.Clamp(gradientLayer, 0, 31);
        if (!GetComponent<Camera>())
        {
            Debug.LogError("Must attach GradientBackground script to the camera");
            return;
        }

        GetComponent<Camera>().clearFlags = CameraClearFlags.Depth;
        GetComponent<Camera>().cullingMask = GetComponent<Camera>().cullingMask & ~(1 << gradientLayer);
        Camera gradientCam = new GameObject("Gradient Cam", typeof(Camera)).GetComponent<Camera>();
        gradientCam.depth = GetComponent<Camera>().depth - 1;
        gradientCam.cullingMask = 1 << gradientLayer;

        mesh = new Mesh();
        mesh.vertices = new Vector3[4]
                        {new Vector3(-100f, .577f, 1f), new Vector3(100f, .577f, 1f), new Vector3(-100f, -.577f, 1f), new Vector3(100f, -.577f, 1f)};

        mesh.colors = new Color[4] { topColor, topColor, bottomColor, bottomColor };

        mesh.triangles = new int[6] { 0, 1, 2, 1, 3, 2 };
       // Material mat = new Material(Shader.Find("Vertex Color Only"));
        GameObject gradientPlane = new GameObject("Gradient Plane", typeof(MeshFilter), typeof(MeshRenderer));

        ((MeshFilter)gradientPlane.GetComponent(typeof(MeshFilter))).mesh = mesh;
        gradientPlane.GetComponent<Renderer>().material = mat;
        gradientPlane.layer = gradientLayer;
    }

}
