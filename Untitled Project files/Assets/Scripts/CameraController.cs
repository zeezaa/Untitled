using UnityEngine;
using System.Collections;
using UnityStandardAssets.ImageEffects;

[System.Serializable]
public class Boundary
{
    public float minX, maxX, minY, maxY, minZ, maxZ;
}

public class CameraController : MonoBehaviour 
{
    public Boundary boundary;
    public GameObject player;
    public GameObject Camera;
    public bool fade;
    public Grayscale gray;
    public float fadeAmount;

    Vector3 offset;

	void Start () 
    {
        Vector2 aspect = AspectRatio.GetAspectRatio(Screen.width, Screen.height, true);
        if (aspect.y == 10)
        {
            boundary.minX = -2.55f;
            boundary.maxX = 2.55f;
        }
        else if (aspect.y == 9)
        {
            boundary.minX = -2.3f;
            boundary.maxX = 2.3f;
        }
        else if (aspect.y < 3)
        {
            boundary.minX = -2.7f;
            boundary.maxX = 2.7f;
        }
        else if (aspect.y < 5)
        {
            boundary.minX = -3f;
            boundary.maxX = 3f;
        }
        else if (aspect.y < 6)
        {
            boundary.minX = -3f;
            boundary.maxX = 3f;
        }
        else if (aspect.y < 7)
        {
            boundary.minX = -2.3f;
            boundary.maxX = 2.3f;
        }
        else
        {
            boundary.minX = -2.3f;
            boundary.maxX = 2.3f;
        }
        gray = GetComponent<Grayscale>();
        fade = false;
        Camera = gameObject;
        offset = transform.position - player.transform.position;
	}
	

    void Update()
    {
        // Follows the player if the player is in the boundaries.
        if (player.transform.position.x < boundary.maxX && player.transform.position.x > boundary.minX)
        {
            transform.position = player.transform.position + offset;   
        }

        // Clamps the cameras movement. 
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, boundary.minX, boundary.maxX), Mathf.Clamp(transform.position.y, boundary.minY, boundary.maxY), Mathf.Clamp(transform.position.z, boundary.minZ, boundary.maxZ));
        if (gray.effectAmount < 1 && fade) { gray.effectAmount = gray.effectAmount + fadeAmount; }
    }

    public void BW()
    {
        fade = true;
    }
}