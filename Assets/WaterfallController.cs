using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterfallController : MonoBehaviour {

	[SerializeField]
	private Slider _slider;

	[SerializeField]
	private GameObject[] _waterfallBottomParts;
	private Material _waterfallTopMat;
	private List<Material> _waterfallMats = new List<Material>();
	private string _selectedValue;

	// Use this for initialization
	void Start () {
		_waterfallTopMat = GetComponent<SkinnedMeshRenderer> ().material;
		foreach(GameObject WaterfallPart in _waterfallBottomParts) {
			_waterfallMats.Add(WaterfallPart.GetComponent<SkinnedMeshRenderer> ().material);
		}	

		UpdateSlider (0);

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetSelectedValue(float pDropOff){
		_waterfallTopMat.SetFloat (_selectedValue, pDropOff);
		foreach (Material WaterfallMat in _waterfallMats) {
			WaterfallMat.SetFloat (_selectedValue, pDropOff);
		}
	}

	public void UpdateSlider(int pValueID){
		switch (pValueID) {
		case 0:
			_selectedValue = "";
			_slider.minValue = 2;
			_slider.maxValue = 32;
			_selectedValue = "_WaterDropOff";
			_slider.value = _waterfallTopMat.GetFloat (_selectedValue);
			break;
		case 1:
			_selectedValue = "";
			_slider.minValue = 0;
			_slider.maxValue = 2;
			_selectedValue = "_WaterSpeed";
			_slider.value = _waterfallTopMat.GetFloat (_selectedValue);

			break;
		case 2: 
			_selectedValue = "";
			_slider.minValue = 1;
			_slider.maxValue = 20;
			_selectedValue = "_WaterfallLength";
			_slider.value = _waterfallTopMat.GetFloat (_selectedValue);
			break;
		}
	}
}
