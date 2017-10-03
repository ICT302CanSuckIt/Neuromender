using UnityEngine;
using System.Collections;

public class Splash : MonoBehaviour
{
    private GUITexture splash;


    // Use this for initialization
    void Update()
    {
        //splash = GetComponent<GUITexture>();

        //splash.pixelInset.x = 0.5f * Screen.width;
        GetComponent<GUITexture>().pixelInset = new Rect(-0.5f * Screen.width, 0.5f * -Screen.height, Screen.width, Screen.height);
    }
}
