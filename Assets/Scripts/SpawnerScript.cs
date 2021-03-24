using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//the check box for the shape of the collision area 
public enum CollisionType
{
    Sphere,
    Box
}

//the Spawner
public class SpawnerScript : MonoBehaviour 
{
    //the variable for the choice of the check box
    public CollisionType type;
    //the time for the creation of a sphere in the beginning
    [Range(0.1f, 5)]
	public float spawnTime = 3f;
    //the number of spheres that will be created
    public float max_num_particles = 20;
    //the multiplier of the force of the wind
    [Range(-500, 500)]
    public float constantForce = 20;
    //the multiplier for the forces of attraction and repulsion
    [Range(0, 10)]
    public float pullForceMultiplier = 100;
    //the boolean for the gravity
    public bool sphereGravity = true;
    //the multiplier for the gravity
    public float gravityMultiplier = 100;
    //the object that with make the attraction or the repulsion
    public GameObject pull0;
    public GameObject pull1;
    //the object for each sphere
	private GameObject generatedSphere;
    //the distance of each sphere from the objects with the attraction and repulsion forces
    float distance1;
    float distance2;
    //the list of the spheres
    public List<SphereMovement> myShperes = new List<SphereMovement>();

	void Start () 
    {
        //find the objects with the attraction and repulsion forces
        pull0 = GameObject.Find("Pull0");
        pull1 = GameObject.Find("Pull1");
        //the routine to create a sphere
        StartCoroutine(WaitAndSpawn(spawnTime));
    }
    //the creation of the spheres
    private IEnumerator WaitAndSpawn(float waitTime)
    {
        while (myShperes.Count < max_num_particles)
        {
            //wait for a while for the next sphere
            yield return new WaitForSeconds(waitTime);
            //the shape of each sphere is taken from the prefab
            generatedSphere = Instantiate(Resources.Load("Prefabs/Sphere") as GameObject);
            //the position of the sphere
            generatedSphere.transform.position = transform.position;
            //add to the list
            myShperes.Add(generatedSphere.GetComponent<SphereMovement>());
        }
    }

    private void FixedUpdate()
    {
        foreach (SphereMovement sphere in myShperes) 
        {
            //the horizontal force of the wind 
            sphere.myRb.AddForce(Vector3.right * constantForce);
            //the gravity(with a choice and a multiplier)
            if (sphereGravity) 
                sphere.myRb.AddForce(Vector3.down * gravityMultiplier);
            //the choice of the check box
            if (type == CollisionType.Box && !sphere.isBox) 
            {
                sphere.isBox = true;
            }

            else if (type == CollisionType.Sphere && sphere.isBox) 
            {
                sphere.isBox = false;
            }
            //if the collision area is a sphere
            if (!sphere.isBox)
            {
                Vector3 dir1 = sphere.transform.position - pull0.transform.position;
                Vector3 dir2 = sphere.transform.position - pull1.transform.position;
                distance1 = dir1.magnitude;
                distance2 = dir2.magnitude;

                sphere.transform.position =
                    Vector3.MoveTowards(sphere.transform.position, pull0.transform.position, -pullForceMultiplier * Time.deltaTime / distance1);
                sphere.transform.position =
                    Vector3.MoveTowards(sphere.transform.position, pull1.transform.position, -pullForceMultiplier * Time.deltaTime / distance2);
            }
            //if the collision area is a cube
            if (sphere.isBox)
                sphere.transform.position =
                    Vector3.MoveTowards(sphere.transform.position, pull0.transform.position, pullForceMultiplier * Time.deltaTime);
                sphere.transform.position =
                    Vector3.MoveTowards(sphere.transform.position, pull1.transform.position, pullForceMultiplier * Time.deltaTime);
        }
    }
}