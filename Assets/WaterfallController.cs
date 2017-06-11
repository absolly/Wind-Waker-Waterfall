using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterfallController : MonoBehaviour {


	[SerializeField]
	private GameObject[] WaterfallBottomParts;
	private Material WaterfallTopMat;
	private List<Material> WaterfallMats = new List<Material>();

	// Use this for initialization
	void Start () {
		WaterfallTopMat = GetComponent<SkinnedMeshRenderer> ().material;
		foreach(GameObject WaterfallPart in WaterfallBottomParts) {
			WaterfallMats.Add(WaterfallPart.GetComponent<SkinnedMeshRenderer> ().material);
		}	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetDropOffValue(float pDropOff){
		WaterfallTopMat.SetFloat ("_WaterDropOff", pDropOff);
		foreach (Material WaterfallMat in WaterfallMats) {
			WaterfallMat.SetFloat ("_WaterDropOff", pDropOff);
		}
	}
}
