using UnityEngine;

public class TutorialHandler : MonoBehaviour {
    private float _disappearTime;

    public void SetActiveFor(int sec)
    {
        gameObject.SetActive(true);
        _disappearTime = Time.timeSinceLevelLoad + sec;
    }

    private void Update ()
    {
		if(Time.timeSinceLevelLoad > _disappearTime)
        {
            gameObject.SetActive(false);
        }
	}
}
