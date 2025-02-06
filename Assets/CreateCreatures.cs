using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class BezierSurfaceOfRevolution : MonoBehaviour
{
    // game objects and public variables
    public int seed;
    public float[] controlPoints;
    public GameObject leg1;
    public GameObject leg2;
    public GameObject leg3;

    public GameObject wing1;
    public GameObject wing2;
    public GameObject wing3;

    public GameObject eye1;
    public GameObject eye2;
    public GameObject eye3;

    private bool touchedLeg = true;
    private int count = 0;

    // 4 points for cubic bezier curve
    public Vector3 p0 = new Vector3(0, 0, 0);
    public Vector3 p1 = new Vector3(0.5f, 1, 0);
    public Vector3 p2 = new Vector3(0.5f, 3, 0);
    public Vector3 p3 = new Vector3(0, 5, 0);

    // customize parameters for surface of revolution
    public int radiusAmount = 50;
    public int heightAmount = 10;
    public float height = 5.0f; // height and width to randomize the scales
    public float width = 1.0f;

    // rotate the random angle as well
    public float xRotationAngle = 30f;

    // materials for color
    public Material surfaceMaterial;
    public Material baseMaterial;
    public Material haloMaterial;

    private void Start()
    {
        Random.InitState(seed); // random seed to provide procedural pseudorandomness
        controlPoints = setControlPoints(); // set the control points each time before generating new surface of revolution
        createMatcha();
        controlPoints = setControlPoints();
        createMatcha();
        controlPoints = setControlPoints();
        createMatcha();
        controlPoints = setControlPoints();
        createMatcha();
        controlPoints = setControlPoints();
        createMatcha();
    }

    float[] setControlPoints()
    {
        // also set the random values for width and height here
        height = 5 + Random.value * 5;
        width = 2 + Random.value * 1;

        float point2 = Random.value * 5; // random number between 0 and 5, we don't want the creature too fat :)
        float point3 = Random.value * 5; // another random number between 0 and 5
        float[] returnArr = new float[] { 0, point2, point3, 0 }; // 0 on start and end so the mesh is closed
        return returnArr;
    }

    // make the matcha monsters
    void createMatcha()
    {
        // Create a new mesh
        Mesh matchaMesh = new Mesh();
        int numVerts = (heightAmount + 1) * (radiusAmount + 1);
        Vector3[] vertices = new Vector3[numVerts];
        int[] triangles = new int[heightAmount * radiusAmount * 6];

        int vertIndex = 0;
        int triangleIndex = 0;

        xRotationAngle = Random.value * 20; // random value between 0 and 20 degree rotation
        if (Random.value < 0.5)
        {
            xRotationAngle = -xRotationAngle; // 50% chance to be rotated the other way
        }
        Quaternion xRotation = Quaternion.Euler(xRotationAngle, 0, 0);

        // generates the vertices
        for (int i = 0; i <= heightAmount; i++)
        {
            float t = (float)i / heightAmount; // gets a number between 0 and 1

            // linearly interpolate between layers
            float currentHeight = Mathf.Lerp(0, height, t);

            Debug.Log(width * (controlPoints[1] + controlPoints[2]) / 2);
            // calculate bezier control points
            Vector3 bezierPoint = bezier(t);
            bezierPoint *= width; // random width factor
            bezierPoint.y = currentHeight;

            for (int j = 0; j <= radiusAmount; j++)
            {
                // angle calculation
                float angle = 2 * Mathf.PI * j / radiusAmount;
                float cos = Mathf.Cos(angle);
                float sin = Mathf.Sin(angle);

                // rotate
                Vector3 rotatedPoint = new Vector3(cos * bezierPoint.x, bezierPoint.y, sin * bezierPoint.x);
                rotatedPoint = xRotation * rotatedPoint;

                vertices[vertIndex] = rotatedPoint;
                vertIndex++;
            }
        }

        // generate the triangles by looping through height and width
        for (int i = 0; i < heightAmount; i++)
        {
            for (int j = 0; j < radiusAmount; j++)
            {
                int curr = i * (radiusAmount + 1) + j;
                int next = curr + 1;
                int currUp = (i + 1) * (radiusAmount + 1) + j;
                int currUpNext = currUp + 1;

                // generate the quad
                // first triangle
                triangles[triangleIndex] = curr;
                triangleIndex++;
                triangles[triangleIndex] = currUp;
                triangleIndex++;
                triangles[triangleIndex] = currUpNext;
                triangleIndex++;

                // second triangle
                triangles[triangleIndex] = curr;
                triangleIndex++;
                triangles[triangleIndex] = currUpNext;
                triangleIndex++;
                triangles[triangleIndex] = next;
                triangleIndex++;
            }
        }

        matchaMesh.vertices = vertices;
        matchaMesh.triangles = triangles;
        matchaMesh.RecalculateNormals();

        // make a new gameobject to attach mesh to so that changes can be made individually
        GameObject matchaBody = new GameObject("MatchaBody");
        MeshFilter meshFilter = matchaBody.AddComponent<MeshFilter>();
        meshFilter.mesh = matchaMesh;
        MeshRenderer meshRenderer = matchaBody.AddComponent<MeshRenderer>();
        meshRenderer.material = surfaceMaterial;
        matchaBody.AddComponent<MeshRenderer>();

        // make the base body
        GameObject baseObject = Instantiate(matchaBody);
        baseObject.transform.localScale = new Vector3(1.1f, 0.6f, 1.1f);
        baseObject.GetComponent<MeshRenderer>().material = baseMaterial;
        
        baseObject.AddComponent<CollisionDetector>();
        CollisionDetector triggerScript = baseObject.GetComponent<CollisionDetector>();
        triggerScript.onTriggerEvent.AddListener(handleTriggerEvent);

        MeshCollider meshCollider = baseObject.AddComponent<MeshCollider>();
        meshCollider.isTrigger = true;
        meshCollider.convex = true;

        // move the matcha monster a fixed distance to spread them out
        matchaBody.transform.position = matchaBody.transform.position + new Vector3((count * 40), 0f, 0f);
        baseObject.transform.position = baseObject.transform.position + new Vector3((count * 40), 0f, 0f);

        // time to make the limbs and swappable body parts?!
        /*int legStyle = Random.Range(0, 3);
        float armLength = Random.Range(0.5f, 2.0f);
        float startRadius = Random.Range(1f, 1f);
        float endRadius = Random.Range(1f, 1f);*/

        Vector3 P0 = new Vector3(0, 0, 0);
        Vector3 P1 = new Vector3(1, 2, 0);
        Vector3 P2 = new Vector3(2, 2, 0);
        Vector3 P3 = new Vector3(3, 0, 0);


        int randomLeg = Random.Range(0, 3);
        // generate claw legs
        if (randomLeg == 0)
        {
            // only make 1 leg if it's too small, otherwise make 2 legs
            if ((width * (controlPoints[1] + controlPoints[2]) / 2) < 4)
            {
                bool touchedLeg = false;
                GameObject newObject = Instantiate(leg1);
                newObject.transform.position = baseObject.transform.position + new Vector3(0.7f, -3.5f, 9.5f);

                // the legs change scale and size, and are moved up/down to account for this scale
                float yScale = Random.Range(0.6f, 2f);
                float yOffset = Mathf.Lerp(2f, -5f, (yScale - 0.6f) / 1.4f);
                Vector3 currentPosition = newObject.transform.position;
                newObject.transform.position = new Vector3(currentPosition.x, currentPosition.y + yOffset, currentPosition.z);
                newObject.transform.localScale = new Vector3(newObject.transform.localScale.x, yScale, newObject.transform.localScale.z);
            }
            else
            {
                // make 2 legs if big enough
                GameObject newObject = Instantiate(leg1);
                GameObject newObject2 = Instantiate(leg1);

                newObject.transform.position = baseObject.transform.position + new Vector3(-1.8f, -3.5f, 9.5f);
                newObject2.transform.position = baseObject.transform.position + new Vector3(3.2f, -3.5f, 9.5f);

                // the legs change scale and size, and are moved up/down to account for this scale
                float yScale = Random.Range(0.6f, 2f);
                float yOffset = Mathf.Lerp(1f, -5f, (yScale - 0.6f) / 1.4f);
                Vector3 currentPosition = newObject.transform.position;
                Vector3 currentPosition2 = newObject2.transform.position;
                newObject.transform.position = new Vector3(currentPosition.x, currentPosition.y + yOffset, currentPosition.z);
                newObject2.transform.position = new Vector3(currentPosition2.x, currentPosition2.y + yOffset, currentPosition2.z);
                newObject.transform.localScale = new Vector3(newObject.transform.localScale.x, yScale, newObject.transform.localScale.z);
                newObject2.transform.localScale = new Vector3(newObject2.transform.localScale.x, yScale, newObject2.transform.localScale.z);
            }
        }
        // generate the quad legs instead
        else if (randomLeg == 1)
        {
            // the randomized positions are handled within the quad leg script
            GameObject newObject = Instantiate(leg2);
            newObject.transform.position = new Vector3((count * 40), 0f, 0f);
        }
        // generate the wheel leg instead
        else if (randomLeg == 2)
        {
            // the wheel leg randomized angle and wheel randomized size is handled within the wheel leg script
            GameObject newObject = Instantiate(leg3);
            newObject.transform.position = new Vector3((count * 40), 0f, 0f);
        }

        // generate the wings/propellors
        int randomWing = Random.Range(0, 3);
        // generates the angel wings
        if (randomWing == 0)
        {
            GameObject wing = Instantiate(wing1);
            wing.transform.position = new Vector3((count * 40), 0f, 0f);

            Transform wingComponent = wing.transform.GetChild(0);
            // procedural random x and z scalings
            float randomX = Random.Range(0.5f, 3f);
            float randomZ = Random.Range(0.5f, 3f);
            wingComponent.localScale = new Vector3(randomX, wingComponent.localScale.y, randomZ);

            // if angel wings, there's a 80% chance to generate a halo as well (angelic!)
            if (Random.value < 0.8)
            {
                GameObject halo = createHalo(4f, 0.2f, 30, 10);
                halo.transform.position = new Vector3((count * 40), 10f, 0f);
            }
        }
        // generate the dark angel wings (different color, and feathers are fallen off)
        else if (randomWing == 1)
        {
            GameObject wing = Instantiate(wing2);
            wing.transform.position = new Vector3((count * 40), 0f, 0f);

            Transform wingComponent = wing.transform.GetChild(0);
            // random x and z scalings
            float randomX = Random.Range(0.5f, 3f);
            float randomZ = Random.Range(0.5f, 3f);
            wingComponent.localScale = new Vector3(randomX, wingComponent.localScale.y, randomZ);
        }
        // generate the back propellor
        else if (randomWing == 2)
        {
            GameObject wing = Instantiate(wing3);
            wing.transform.position = new Vector3((count * 40), 0f, 0f);

            Transform wingComponent = wing.transform.GetChild(0);
            // random x and z scaling
            float randomX = Random.Range(0.5f, 3f);
            float randomZ = randomX;
            wingComponent.localScale = new Vector3(randomX, wingComponent.localScale.y, randomZ);
        }

        // generate the eyes (Naruto style)
        int randomEye = Random.Range(0, 3);
        // generate the regular eyes
        if (randomEye == 0)
        {
            GameObject eye = Instantiate(eye1);
            eye.transform.position = new Vector3((count * 40), 0f, 0f);

            Transform eyeBeam1 = eye.transform.GetChild(0);
            Transform eyeBeam2 = eye.transform.GetChild(1);
            Transform rotate1 = eye.transform.GetChild(2);
            Transform rotate2 = eye.transform.GetChild(3);

            // random rotations to move the eyes
            float random1 = Random.Range(-40f, 40f);
            float random2 = Random.Range(-40f, 40f);
            eyeBeam1.transform.RotateAround(rotate1.position, rotate1.up, random1);
            eyeBeam2.transform.RotateAround(rotate2.position, rotate2.up, random2);
        }
        // generate the eyes of the Sage of Six Paths (Yellow and horizontal iris)
        else if (randomEye == 1)
        {
            GameObject eye = Instantiate(eye2);
            eye.transform.position = new Vector3((count * 40), 0f, 0f);

            Transform eyeBeam1 = eye.transform.GetChild(0);
            Transform eyeBeam2 = eye.transform.GetChild(1);
            Transform rotate1 = eye.transform.GetChild(2);
            Transform rotate2 = eye.transform.GetChild(3);

            // random angles that can only move towards each other
            float random1 = Random.Range(-40f, 0f);
            float random2 = Random.Range(0f, 40f);
            eyeBeam1.transform.RotateAround(rotate1.position, rotate1.up, random1);
            eyeBeam2.transform.RotateAround(rotate2.position, rotate2.up, random2);
        }
        // generate the Sharingan (red and black iris)
        else if (randomEye == 2)
        {
            GameObject eye = Instantiate(eye3);
            eye.transform.position = new Vector3((count * 40), 0f, 0f);

            Transform eyeBeam1 = eye.transform.GetChild(0);
            Transform eyeBeam2 = eye.transform.GetChild(1);
            Transform rotate1 = eye.transform.GetChild(2);
            Transform rotate2 = eye.transform.GetChild(3);

            // random angle towards each eye same amount
            float random1 = Random.Range(-40f, 0f);
            float random2 = -random1;
            eyeBeam1.transform.RotateAround(rotate1.position, rotate1.up, random1);
            eyeBeam2.transform.RotateAround(rotate2.position, rotate2.up, random2);
        }

        /*MeshFilter meshFilter = GetComponent<MeshFilter>();
        if (meshFilter != null)
        {
            meshFilter.mesh = mesh;
        }

        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null && surfaceMaterial != null)
        {
            meshRenderer.material = surfaceMaterial;
        }*/

        count++;
    }

    // use the cubic bezier formula to calculate the points
    Vector3 bezier(float t)
    {
        Vector3 pos0 = new Vector3(controlPoints[0], 0f, 10f);
        Vector3 pos1 = new Vector3(controlPoints[1], 1f, 9f);
        Vector3 pos2 = new Vector3(controlPoints[2], 2f, 8f);
        Vector3 pos3 = new Vector3(controlPoints[3], 3f, 7f);
        return Mathf.Pow(1 - t, 3) * pos0 + 3 * Mathf.Pow(1 - t, 2) * t * pos1 + 3 * (1 - t) * Mathf.Pow(t, 2) * pos2 + Mathf.Pow(t, 3) * pos3;
    }

    public void handleTriggerEvent()
    {
        touchedLeg = true;
    }

    GameObject createHalo(float outerRadius, float innerRadius, int radiusAmount, int segments)
    {
        Mesh haloMesh = new Mesh();

        Vector3[] vertices = new Vector3[radiusAmount * segments];
        int[] triangles = new int[radiusAmount * segments * 6];
        float radialStep = 2 * Mathf.PI / radiusAmount;
        float widthStep = 2 * Mathf.PI / segments;

        // generate the vertices
        int vertexIndex = 0;
        for (int i = 0; i < radiusAmount; i++)
        {
            float angle = i * radialStep;
            float cos = Mathf.Cos(angle);
            float sin = Mathf.Sin(angle);

            for (int j = 0; j < segments; j++)
            {
                float tubeAngle = j * widthStep;
                float cosTube = Mathf.Cos(tubeAngle);
                float sinTube = Mathf.Sin(tubeAngle);

                // vertex positions
                float x = (outerRadius + innerRadius * cosTube) * cos;
                float z = (outerRadius + innerRadius * cosTube) * sin;
                float y = innerRadius * sinTube;

                vertices[vertexIndex] = new Vector3(x, y, z);
                vertexIndex++;
            }
        }

        // generate triangles from vertex list
        int triangleIndex = 0;
        for (int i = 0; i < radiusAmount; i++)
        {
            for (int j = 0; j < segments; j++)
            {
                int nextSegment = ((i + 1) % radiusAmount) * segments;
                int nextSegmentIndex = (j + 1) % segments;

                int current = i * segments + j;
                int next = nextSegment + j;
                int currTube = i * segments + nextSegmentIndex;
                int currTubeNext = nextSegment + nextSegmentIndex;

                triangles[triangleIndex++] = current;
                triangles[triangleIndex++] = currTube;
                triangles[triangleIndex++] = next;

                triangles[triangleIndex++] = currTube;
                triangles[triangleIndex++] = currTubeNext;
                triangles[triangleIndex++] = next;
            }
        }

        haloMesh.vertices = vertices;
        haloMesh.triangles = triangles;
        haloMesh.RecalculateNormals();
        haloMesh.RecalculateBounds();

        // make a new gameobject to attach mesh to so that changes can be made individually
        GameObject haloObject = new GameObject("Halo");
        MeshFilter meshFilter = haloObject.AddComponent<MeshFilter>();
        meshFilter.mesh = haloMesh;
        MeshRenderer meshRenderer = haloObject.AddComponent<MeshRenderer>();
        meshRenderer.material = haloMaterial;
        haloObject.AddComponent<MeshRenderer>();

        return haloObject;
    }
}