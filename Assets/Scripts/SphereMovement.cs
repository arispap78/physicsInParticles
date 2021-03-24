using UnityEngine;

//class for the movement of the spheres
public class SphereMovement : MonoBehaviour
{
    //multiplies the forces
    public float forceMultiplier = 10;
    //the limit of every axis
    public float axisLimit = 4;
    //the radius of the sphere
    public float radius = 5;
    //the point in space where the velocity of each sphere with go to
    Vector3 pointA;
    //the rigidbody of the sphere
    public Rigidbody myRb;
    //the boolean if the limits are of a cube or of a sphere
    public bool isBox = true;


    void Start()
    {
        //get the rigidbody of the sphere
        myRb = this.gameObject.GetComponent<Rigidbody>();
        //the random variables of the pointA
        float randomX = Random.Range(-12, 12);
        float randomY = Random.Range(-12, 12);
        float randomZ = Random.Range(-12, 12);
        //the point for the direction of the velocity
        pointA = new Vector3(randomX, randomY, randomZ);
        //the velocity of the sphere
        myRb.velocity = pointA;
    }

    void FixedUpdate()
    {
        //if it is a cube
        if (isBox) 
        {
            BoxCollision();
        }
        //if it is a sphere
        else 
        {
            SphereCollision();
        }

    }

    //the collision of the spheres inside the cube
    public void BoxCollision() 
    {
        //if the sphere is off the limits in the x axis
        if (Mathf.Abs(transform.position.x) >= axisLimit)
        {
            //reverse the value of x 
            pointA = new Vector3(pointA.x * -1, pointA.y, pointA.z);
            //put the sphere inside the cube in case it is outside
            transform.position = new Vector3(transform.position.x * 3.99f / Mathf.Abs(transform.position.x)
                , transform.position.y, transform.position.z);
        }
        //if the sphere is off the limits in the y axis
        if (Mathf.Abs(transform.position.y) >= axisLimit)
        {
            //reverse the value of y 
            pointA = new Vector3(pointA.x, pointA.y * -1, pointA.z);
            //put the sphere inside the cube in case it is outside
            transform.position = new Vector3(transform.position.x, transform.position.y * 3.99f / Mathf.Abs(transform.position.y)
                , transform.position.z);
        }
        //if the sphere is off the limits in the z axis
        if (Mathf.Abs(transform.position.z) >= axisLimit)
        {
            //reverse the value of z
            pointA = new Vector3(pointA.x, pointA.y, pointA.z * -1);
            //put the sphere inside the cube in case it is outside
            transform.position = new Vector3(transform.position.x, transform.position.y,
                transform.position.z * 3.99f / Mathf.Abs(transform.position.z));
        }
        //multiply the velocity to the pointA with 0.05 coefficient of depreciation
        myRb.velocity = pointA * forceMultiplier * 0.95f;
    }

    //the collision of the spheres inside the sphere
    public void SphereCollision() 
    {
        //if a sphere pass the limits of the sphere(x*x+y*y+z*z=radius*radius)
        if (System.Math.Pow(transform.position.x, 2) + System.Math.Pow(transform.position.y, 2) + System.Math.Pow(transform.position.z, 2) >= System.Math.Pow(radius, 2))
        {
            //reverse the values of the coordinates 
            pointA = new Vector3(pointA.x * -1, pointA.y * -1, pointA.z * -1);
            //put the sphere inside the sphere in case it is outside
            transform.position = new Vector3(transform.position.x * 0.9f, transform.position.y * 0.9f, transform.position.z * 0.9f);
        }
        //multiply the velocity to the pointA with 0.05 coefficient of depreciation
        myRb.velocity = pointA * forceMultiplier * 0.95f;
    } 
}

