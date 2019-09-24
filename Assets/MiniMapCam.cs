using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapCam : MonoBehaviour
{
    public Transform player = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetPlayer(Transform p)
    {
        player = p;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(player)
        {
            Vector3 newPos = player.position;
            newPos += new Vector3(0f, 500f, 0f);
            transform.position = newPos;

            transform.rotation = Quaternion.Euler(90f, player.eulerAngles.y, 0f);
        }
    }
}
