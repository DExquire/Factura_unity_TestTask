using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCoinText : MonoBehaviour
{
    public Vector3 startPosition;
    public float maxDistance;
    public float speed;

    public void OnEnable()
    {
        startPosition = transform.localPosition;
        FlyUp();
    }

    public void FlyUp()
    {
        if (Vector3.Distance(startPosition, transform.position) < maxDistance)
        {
            transform.localPosition = speed * Time.deltaTime * Vector3.up;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
}
