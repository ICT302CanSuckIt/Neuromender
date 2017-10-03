using UnityEngine;
using System.Collections;

public class RingCollider : MonoBehaviour {

	private bool hasPlayed = false;

    HeadsUpDisplay hud;

	// Use this for initialization
	void Start () {
        hud = GameObject.Find("HUD").GetComponent<HeadsUpDisplay>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other) {
        int maxPoints = 10;

		if (!hasPlayed) {
			this.GetComponent<AudioSource>().Play ();
			//print ("BLAAAAHHH!!11");
			//Cameron put your point scoring here
            StrokeRehabLevelController blah = GameObject.Find("NeuromendController").GetComponent<StrokeRehabLevelController>();

            float diff = 0;
            if(blah.getAngle() > blah.getAngleThreshold())
            {
                //angle is greater
                diff = blah.getAngle() - blah.getAngleThreshold();
            }
            else
            {
                diff = blah.getAngleThreshold() - blah.getAngle();
            }

            int points = 0;

            if (diff <= 1)
                points = maxPoints;
            else if (diff <= 2)
                points = maxPoints - (maxPoints/10);
            else if (diff <= 3)
                points = maxPoints - ((maxPoints / 10) * 2);
            else if (diff <= 4)
                points = maxPoints - ((maxPoints / 10) * 3);
            else if (diff <= 5)
                points = maxPoints - ((maxPoints / 10) * 4);
            else if (diff <= 6)
                points = maxPoints - ((maxPoints / 10) * 5);
            else if (diff <= 7)
                points = maxPoints - ((maxPoints / 10) * 6);
            else if (diff <= 8)
                points = maxPoints - ((maxPoints / 10) * 7);
            else if (diff <= 9)
                points = maxPoints - ((maxPoints / 10) * 8);
            else if (diff <= 10)
                points = maxPoints - ((maxPoints / 10) * 9);
            else
                points = 0;


            hud.GotRing(points,blah.getAngle());
            //hud.score += points;

            //print("" + blah.getAngle());
            //print("" + (points));
			//print("" + blah.getAngleThreshold());

            //hud.rings++;
			hasPlayed = true;
		}
	}
}
