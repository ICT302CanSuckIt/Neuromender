using UnityEngine;
using Tobii.EyeX.Framework;
using System.Collections;

public class TobiiEyeX : IEyeTrackerData
{
    private EyeXHost _eyexHost;
    private IEyeXDataProvider<EyeXGazePoint> _dataProvider;
    private GazePointDataMode gazePointDataMode = GazePointDataMode.LightlyFiltered;

    // Use this for initialization
    public TobiiEyeX()
    {
        _eyexHost = EyeXHost.GetInstance();
        _dataProvider = _eyexHost.GetGazePointDataProvider(gazePointDataMode);
        _dataProvider.Start();

        Debug.Log(_eyexHost);
    }
	
	public Vector2 GetEyeGazePosition()
    {
        return _dataProvider.Last.Viewport;
    }
}
