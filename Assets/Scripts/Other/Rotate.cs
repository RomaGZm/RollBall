
using UnityEngine;

public class Rotate : MonoBehaviour
{
    public float Speed = 1;
    public Vector3 Direction = Vector3.up;

    private void Update()
    {
        transform.Rotate(Direction * Speed * Time.deltaTime);
    }
}
