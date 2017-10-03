using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class TextBoxEnterButton : MonoBehaviour {
	
	public GameObject Target;
	
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		var pointer = new PointerEventData(EventSystem.current); 
		if (Input.GetKeyDown(KeyCode.Return))
		{
			ExecuteEvents.Execute(Target, pointer, ExecuteEvents.submitHandler);
		}
	}
}