using UnityEngine;

// Shader help - https://www.youtube.com/watch?v=dFDAwT5iozo
// Capture camera to texture - https://answers.unity.com/questions/576012/create-texture-from-current-camera-view.html
public class Shockwave : MonoBehaviour
{
    [SerializeField]
    private GameObject targetObject;

    [SerializeField]
    private Camera camera;

    [SerializeField]
    private int radius;

    [SerializeField]
    private float startTimePos;

    [SerializeField]
    private float endTime = 1f;

    private Renderer targetRenderer;
    private RenderTexture renderTexture;
    private Texture2D screenShot;
    private float time;
    private bool triggered;
    private Vector3 originalTargetPosition;
    private Quaternion originalRotation;

    private void Awake()
    {
        camera.aspect = 1;
        camera.orthographicSize = radius / 2;

        renderTexture = new RenderTexture(camera.pixelWidth, camera.pixelHeight, 24);
        screenShot = new Texture2D(camera.pixelWidth, camera.pixelHeight, TextureFormat.RGBA32, false);
        targetRenderer = targetObject.GetComponent<Renderer>();

        camera.targetTexture = renderTexture;
        time = startTimePos;
        endTime += startTimePos;
    }

    private void Update()
    {
        if (triggered)
        {
            time += Time.deltaTime;
            targetRenderer.material.SetFloat("_ExternalTime", time);

            if (time > endTime)
            {
                triggered = false;
                time = startTimePos;

                targetRenderer.enabled = false;
                targetObject.transform.position = originalTargetPosition;
                transform.rotation = originalRotation;
                enabled = false;
            }
        }
    }

    public void Trigger()
    {
        triggered = true;

        AlignWithGround();
        camera.Render();
        RenderTexture previousTexture = RenderTexture.active;
        RenderTexture.active = renderTexture;

        screenShot.ReadPixels(new Rect(0, 0, camera.pixelWidth, camera.pixelHeight), 0, 0);
        screenShot.Apply();

        RenderTexture.active = previousTexture;

        targetRenderer.material.mainTexture = screenShot;
        targetRenderer.enabled = true;
        enabled = true;
    }

    private void AlignWithGround()
    {
        originalTargetPosition = targetObject.transform.position;
        originalRotation = transform.rotation;

        var ray = new Ray(targetObject.transform.position, Vector3.down);
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red, 5f, false);

        if (Terrain.activeTerrain.GetComponent<TerrainCollider>().Raycast(ray, out RaycastHit raycastHit, 100f))
        {
            Vector3 newPosition = raycastHit.point;
            newPosition.y -= 1;
            targetObject.transform.position = newPosition;
            transform.rotation = Quaternion.FromToRotation(transform.up, raycastHit.normal);
        }
    }
}
