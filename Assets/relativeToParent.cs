using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class relativeToParent : MonoBehaviour {
	void Start () {
        GetComponent<RectTransform>().sizeDelta = transform.parent.GetComponent<RectTransform>().sizeDelta;
	}
}
