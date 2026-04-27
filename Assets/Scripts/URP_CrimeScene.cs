using UnityEngine;

public class URP_CrimeScene_Fixed : MonoBehaviour
{
    [Header("Avatar Settings")]
    public GameObject playerAvatar;  // Drag your avatar here in Inspector
    
    [Header("Adjustment Settings")]
    public float avatarHeight = 1.6f;  // Adjust based on your avatar's height
    public float neckOffset = 0.5f;    // Distance from feet to neck
    public float ropeHeight = 2.4f;    // Height of ceiling rope
    
    void Start()
    {
        // DON'T create cylinder body - use your avatar instead
        // CreateHangingBody();  ← COMMENTED OUT
        
        SetupAvatar();      // NEW: Setup your avatar
        CreateURPRope();    // Creates rope around avatar's neck
        AddURPLighting();   // Adds dramatic lighting
    }
    
    void SetupAvatar()
    {
        if (playerAvatar == null)
        {
            Debug.LogError("❌ No avatar assigned! Drag your avatar to the script in Inspector.");
            return;
        }
        
        // Position avatar correctly for hanging
        playerAvatar.transform.position = new Vector3(0, 0.8f, 0);
        
        // Freeze avatar so it doesn't move/fall
        Rigidbody rb = playerAvatar.GetComponent<Rigidbody>();
        if (rb == null)
            rb = playerAvatar.AddComponent<Rigidbody>();
        
        rb.isKinematic = true;   // Stops physics from moving it
        rb.useGravity = false;    // Prevents falling
        
        // Optional: Add slight rotation for realism
        playerAvatar.transform.rotation = Quaternion.Euler(0, 0, 2);
        
        Debug.Log("✅ Avatar setup complete!");
    }
    
    void CreateURPRope()
    {
        GameObject ropeGroup = new GameObject("RopeGroup");
        
        // Calculate neck position (where rope should go)
        float neckY = 0.8f + neckOffset;  // Base position + neck offset
        
        // If avatar is assigned, use its position
        if (playerAvatar != null)
        {
            neckY = playerAvatar.transform.position.y + neckOffset;
        }
        
        // Main vertical rope (from ceiling to neck)
        float ropeLength = ropeHeight - neckY;
        float ropeMidY = (ropeHeight + neckY) / 2f;
        
        GameObject verticalRope = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        verticalRope.name = "VerticalRope";
        verticalRope.transform.parent = ropeGroup.transform;
        verticalRope.transform.position = new Vector3(0, ropeMidY, 0);
        verticalRope.transform.localScale = new Vector3(0.06f, ropeLength / 2f, 0.06f);
        
        // Rope material (URP compatible)
        Renderer rend = verticalRope.GetComponent<Renderer>();
        Material ropeMat = new Material(Shader.Find("Universal Render Pipeline/Lit"));
        ropeMat.color = new Color(0.55f, 0.35f, 0.15f); // Brown rope color
        ropeMat.SetFloat("_Metallic", 0.1f);
        ropeMat.SetFloat("_Smoothness", 0.3f);
        rend.material = ropeMat;
        
        // Loop around neck (horizontal)
        GameObject neckLoop = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        neckLoop.name = "NeckLoop";
        neckLoop.transform.parent = ropeGroup.transform;
        neckLoop.transform.position = new Vector3(0, neckY, 0);
        neckLoop.transform.localScale = new Vector3(0.3f, 0.05f, 0.3f);
        neckLoop.GetComponent<Renderer>().material = ropeMat;
        
        // Rope knot
        GameObject knot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        knot.name = "Knot";
        knot.transform.parent = ropeGroup.transform;
        knot.transform.position = new Vector3(0.1f, neckY + 0.05f, 0.15f);
        knot.transform.localScale = new Vector3(0.1f, 0.08f, 0.1f);
        knot.GetComponent<Renderer>().material = ropeMat;
        
        // Add hanging rope end (dangling piece)
        GameObject danglingEnd = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
        danglingEnd.name = "DanglingEnd";
        danglingEnd.transform.parent = ropeGroup.transform;
        danglingEnd.transform.position = new Vector3(0.12f, neckY - 0.15f, 0.12f);
        danglingEnd.transform.localScale = new Vector3(0.05f, 0.2f, 0.05f);
        danglingEnd.GetComponent<Renderer>().material = ropeMat;
        
        RemoveAllRigidbodies(ropeGroup);
        
        Debug.Log("✅ Rope created at neck height: " + neckY);
    }
    
    void RemoveAllRigidbodies(GameObject obj)
    {
        Rigidbody rb = obj.GetComponent<Rigidbody>();
        if (rb != null) Destroy(rb);
        
        foreach (Transform child in obj.transform)
        {
            rb = child.GetComponent<Rigidbody>();
            if (rb != null) Destroy(rb);
        }
    }
    
    void AddURPLighting()
    {
        // Main directional light
        Light mainLight = FindFirstObjectByType<Light>();
        if (mainLight == null)
        {
            GameObject lightObj = new GameObject("Directional Light");
            mainLight = lightObj.AddComponent<Light>();
            mainLight.type = LightType.Directional;
        }
        
        mainLight.intensity = 0.7f;
        mainLight.color = new Color(1f, 0.9f, 0.8f);
        mainLight.shadows = LightShadows.Soft;
        
        // Dramatic fill light from below (creates eerie effect)
        GameObject fillLightObj = new GameObject("Fill Light");
        fillLightObj.transform.position = new Vector3(0, 0.5f, 0);
        Light fillLight = fillLightObj.AddComponent<Light>();
        fillLight.type = LightType.Point;
        fillLight.intensity = 0.4f;
        fillLight.color = new Color(0.6f, 0.5f, 0.7f);
        
        // Optional: Add back light for rim lighting
        GameObject backLightObj = new GameObject("Back Light");
        backLightObj.transform.position = new Vector3(0, 1.2f, -2f);
        Light backLight = backLightObj.AddComponent<Light>();
        backLight.type = LightType.Point;
        backLight.intensity = 0.3f;
        backLight.color = new Color(0.4f, 0.5f, 0.8f);
        
        Debug.Log("✅ Lighting added!");
    }
    
    // Optional: Add this for debugging
    void OnDrawGizmos()
    {
        if (playerAvatar != null)
        {
            // Draw neck position in Scene view (for adjustment)
            float neckY = playerAvatar.transform.position.y + neckOffset;
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(new Vector3(0, neckY, 0), 0.2f);
            
            // Draw rope line
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(new Vector3(0, ropeHeight, 0), new Vector3(0, neckY, 0));
        }
    }
}