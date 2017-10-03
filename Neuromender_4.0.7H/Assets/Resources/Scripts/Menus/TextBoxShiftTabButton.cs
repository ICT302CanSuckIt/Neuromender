﻿using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class TextBoxShiftTabButton : MonoBehaviour {

	public GameObject Target;
	private bool _resolve;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		_resolve = false;
		if(EventSystem.current.currentSelectedGameObject == this.gameObject)
		{
			if (Input.GetKeyDown(KeyCode.Tab) && Input.GetKey(KeyCode.LeftShift))
			{
				_resolve = true;
			}
		}
	}

	void LateUpdate()
	{
		if(_resolve)
		{
			EventSystem.current.SetSelectedGameObject(Target.gameObject);
		}
	}
}
