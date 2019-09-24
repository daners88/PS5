using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum KeyRequired
{
    Quad1toQuad2 = 12,
    Quad1toQuad3 = 13,
    Quad2toQuad4 = 24,
    Quad3toQuad4 = 34
}
public class Door : MonoBehaviour
{
    public List<Material> mats = null;
    public KeyRequired keyReq;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetMat()
    {
        if(keyReq == KeyRequired.Quad1toQuad2)
        {
            GetComponent<Renderer>().material = mats[0]; 
        }
        else if (keyReq == KeyRequired.Quad1toQuad3)
        {
            GetComponent<Renderer>().material = mats[1];
        }
        else if (keyReq == KeyRequired.Quad2toQuad4)
        {
            GetComponent<Renderer>().material = mats[2];
        }
        else if (keyReq == KeyRequired.Quad3toQuad4)
        {
            GetComponent<Renderer>().material = mats[3];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
