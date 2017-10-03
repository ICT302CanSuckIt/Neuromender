using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public enum eOrientationMode { NODE = 0, TANGENT }

[AddComponentMenu("Splines/Spline Controller")]
[RequireComponent(typeof(SplineInterpolator))]
public class SplineController : MonoBehaviour
{
	public GameObject SplineRoot;
	public float Duration = 10;
	public eOrientationMode OrientationMode = eOrientationMode.NODE;
	public eWrapMode WrapMode = eWrapMode.ONCE;
	public bool AutoStart = true;
	public bool AutoClose = true;
	public bool HideOnExecute = true;

    public int CountDownTimer;

    public Text VirtPhysioText;

    SplineInterpolator mSplineInterp;
	Transform[] mTransforms;

	void OnDrawGizmos()
	{
		Transform[] trans = GetTransforms();
		if (trans.Length < 2)
			return;

        if (AutoStart)      // This is a hack job ok?
        {
            SplineInterpolator interp = GetComponent(typeof(SplineInterpolator)) as SplineInterpolator;
            SetupSplineInterpolator(interp, trans);
            interp.StartInterpolation(null, false, WrapMode);


            Vector3 prevPos = trans[0].position;
            for (int c = 1; c <= 100; c++)
            {
                float currTime = c * Duration / 100;
                Vector3 currPos = interp.GetHermiteAtTime(currTime);
                float mag = (currPos - prevPos).magnitude * 2;
                Gizmos.color = new Color(mag, 0, 0, 1);
                Gizmos.DrawLine(prevPos, currPos);
                prevPos = currPos;
            }
        }
	}


	void Start()
	{
		mSplineInterp = GetComponent(typeof(SplineInterpolator)) as SplineInterpolator;

		mTransforms = GetTransforms();

		if (HideOnExecute)
			DisableTransforms();

        if (AutoStart)
            FollowSpline();
        else
            StartCoroutine(CountDownStart());
	}

	void SetupSplineInterpolator(SplineInterpolator interp, Transform[] trans)
	{
		interp.Reset();

		float step = (AutoClose) ? Duration / trans.Length :
			Duration / (trans.Length - 1);

		int c;
		for (c = 0; c < trans.Length; c++)
		{
			if (OrientationMode == eOrientationMode.NODE)
			{
				interp.AddPoint(trans[c].position, trans[c].rotation, step * c, new Vector2(0, 1));
			}
			else if (OrientationMode == eOrientationMode.TANGENT)
			{
				Quaternion rot;
				if (c != trans.Length - 1)
					rot = Quaternion.LookRotation(trans[c + 1].position - trans[c].position, trans[c].up);
				else if (AutoClose)
					rot = Quaternion.LookRotation(trans[0].position - trans[c].position, trans[c].up);
				else
					rot = trans[c].rotation;

				interp.AddPoint(trans[c].position, rot, step * c, new Vector2(0, 1));
			}
		}

		if (AutoClose)
			interp.SetAutoCloseMode(step * c);
	}


	/// <summary>
	/// Returns children transforms, sorted by name.
	/// </summary>
	Transform[] GetTransforms()
	{
		if (SplineRoot != null)
		{
			List<Component> components = new List<Component>(SplineRoot.GetComponentsInChildren(typeof(Transform)));
			List<Transform> transforms = components.ConvertAll(c => (Transform)c);

			//if(SplineRoot.CompareTag ("Path1"))


			transforms.Remove(SplineRoot.transform);
			transforms.Sort(delegate(Transform a, Transform b)
			{
				return a.name.CompareTo(b.name);
			});

			return transforms.ToArray();
		}

		return null;
	}

	/// <summary>
	/// Disables the spline objects, we don't need them outside design-time.
	/// </summary>
	void DisableTransforms()
	{
		if (SplineRoot != null)
		{
			SplineRoot.SetActiveRecursively(false);
		}
	}


	/// <summary>
	/// Starts the interpolation
	/// </summary>
	void FollowSpline()
	{
		if (mTransforms.Length > 0)
		{
			SetupSplineInterpolator(mSplineInterp, mTransforms);
			mSplineInterp.StartInterpolation(null, true, WrapMode);
		}
	}

    IEnumerator CountDownStart()
    {
        for (int i = CountDownTimer; i >= 0; i--)
        {
            if (i > 0) // 3... 2... 1...
            {
                VirtPhysioText.text = "Starting in: " + i.ToString();

                yield return new WaitForSeconds(1.0f);
            }
            else if (i <= 0) // START!
            {
                VirtPhysioText.enabled = false;
                AutoStart = true;
                FollowSpline();
            }
        }
        yield break;
    }
}