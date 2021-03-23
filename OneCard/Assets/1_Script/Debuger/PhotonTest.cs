using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PhotonTest : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    [PunRPC]
    void NetworkSetParent(string name)
    {
        photonView.GetInstanceID();
    }
    // ���ÿ��� ���� �˾Ƽ� �����ϰ� �ϴ� ���
    GameObject fire;
    GameObject myGuy;
    GameObject flameBaby;  //Drag "flamebaby" here in the Inspector
    void asdf()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            photonView.RPC("InstantiateFlame", RpcTarget.All);
        }
        if (Input.GetKeyUp(KeyCode.F))
        {
            photonView.RPC("DestroyFlame ", RpcTarget.All);
        }
    }
    [PunRPC]
    void InstantiateFlame()
    {
        fire = (GameObject)Instantiate(flameBaby, transform.position + transform.forward, transform.rotation);

        //this is the part that is giving me trouble.
        fire.transform.parent = myGuy.transform;
    }
    [PunRPC]
    void DestroyFlame()
    {
        if (fire)
        {
            Destroy(fire);
        }

    }
          


}
 
         



