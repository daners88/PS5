using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum KeyType
{
    Quad12 = 12,
    Quad13 = 13,
    Quad24 = 24
}
public class Key : MonoBehaviour
{
    public KeyType keytype;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(keytype == KeyType.Quad12)
        {
            other.transform.parent.gameObject.GetComponent<PlayerController>().quad12Enabled = true;
        }
        else if(keytype == KeyType.Quad13)
        {
            other.transform.parent.gameObject.GetComponent<PlayerController>().quad13Enabled = true;
        }
        else if(keytype == KeyType.Quad24)
        {
            other.transform.parent.gameObject.GetComponent<PlayerController>().quad24Enabled = true;
        }
        gameObject.SetActive(false);
    }
}
