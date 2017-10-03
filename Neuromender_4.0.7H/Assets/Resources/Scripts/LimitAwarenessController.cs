using UnityEngine;
using System.Collections;

public class LimitAwarenessController : MonoBehaviour {

    private UserConfig _userConfig;

    // Use this for initialization
    void Start ()
    {
        _userConfig = GameObject.Find("DatabaseController").GetComponent<LoginControl>().config;
        var limitTextObject = this.GetComponent<UnityEngine.UI.Text>();
        limitTextObject.text = _userConfig.LimitWarning;
    }

    // Update is called once per frame
    void Update ()
    {
	
	}
}
