using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class relativeToParent : MonoBehaviour {
    public bool update = false;
	void Start () { setRelative(); }
    void Update() { if (update) setRelative(); }
    private void setRelative() { GetComponent<RectTransform>().sizeDelta = transform.parent.GetComponent<RectTransform>().sizeDelta; }
}
