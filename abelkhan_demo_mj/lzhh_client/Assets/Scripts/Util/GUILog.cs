using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUILog : MonoBehaviour {
	static GUILog instance = null;
	List<string> logs = new List<string>();
	float elapsed = 0.0f;
	// Use this for initialization
	void Start () {
		instance = this;
	}

    // Update is called once per frame
    void Update()
    {
        if (elapsed > 1.0f)
        {
            if (instance.logs.Count > 0)
            {
                instance.logs.RemoveAt(0);
            }
            elapsed = 0.0f;
        }
        elapsed += Time.deltaTime;
    }

    public static void Log(string message)
	{
		#if DEBUG
		if (instance != null) {
			instance.logs.Add (message);
			if(instance.logs.Count > 20)
			{
				instance.logs.RemoveAt(0);
			}
		}
		#endif // DEBUG
	}

	void OnGUI()
	{
		for (int i = 0; i < logs.Count; ++i) {
			var msg = logs [i];
			GUI.TextArea(new Rect(0, i * 30, 500, 30), msg);
		}
	}
}
