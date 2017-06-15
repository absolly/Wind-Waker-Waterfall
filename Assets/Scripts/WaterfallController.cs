using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterfallController : MonoBehaviour
{

	[SerializeField]
	private Slider _slider;
	[SerializeField]
	private InputField _inputField;
	private GameObject _activeInput;

	[SerializeField]
	private GameObject[] _waterfallParts;
	private List<Material> _waterfallMats = new List<Material> ();
	private string _selectedProperty;
	[SerializeField]
	private GameObject _temp;
	[SerializeField]
	private GameObject _temp2;
	[SerializeField]
	public List<Property> properties;

	[System.Serializable]
	public class RangeProperty
	{
		public float min;
		public float max;
	}

	[SerializeField]
	public enum PropertyType
	{
		RANGE,
		FLOAT
	}

	[System.Serializable]
	public class Property
	{
		public PropertyType type;
		public string propertyName;
		public RangeProperty range;
	}

	// Use this for initialization
	void Start ()
	{
		//_waterfallTopMat = GetComponent<SkinnedMeshRenderer> ().material;
		foreach (GameObject WaterfallPart in _waterfallParts) {
			_waterfallMats.Add (WaterfallPart.GetComponent<SkinnedMeshRenderer> ().material);
		}	

		UpdateSlider (0);

	}

	// Update is called once per frame
	void Update ()
	{
		float value = _waterfallMats[0].GetFloat ("_WaterDropOff");
		float value2 = _waterfallMats[0].GetFloat ("_WaterfallLength");
		float x = (value2 / ((value * 0.1f))) - 8;

		_temp2.transform.LookAt (transform.position);
		Vector3 pos = _temp.transform.position;
		pos.z = Mathf.Lerp (0, x, (transform.position.y - pos.y) * 0.05f);
		_temp.transform.position = pos;

		
	}

	public void SetSelectedProperty (float pValue)
	{
		foreach (Material WaterfallMat in _waterfallMats) {
			WaterfallMat.SetFloat (_selectedProperty, pValue);
		}
	}

	public void SetSelectedProperty (string pValue){
		float value;
		if (float.TryParse (pValue, out value)) {
			foreach (Material WaterfallMat in _waterfallMats) {
				WaterfallMat.SetFloat (_selectedProperty, value);
			}
		} else {
			Debug.LogWarning ("invalid float");
		}
	}

	public void UpdateSlider (int pValueID)
	{
		_selectedProperty = "";
		if(_activeInput != null)
			_activeInput.SetActive (false);
	
		switch (properties [pValueID].type) {
		case PropertyType.RANGE:
			_activeInput = _slider.gameObject;
			_slider.minValue = properties [pValueID].range.min;
			_slider.maxValue = properties [pValueID].range.max;
			_selectedProperty = properties [pValueID].propertyName;
			_slider.value = _waterfallMats[0].GetFloat (_selectedProperty);
			break;
		case PropertyType.FLOAT:
			_activeInput = _inputField.gameObject;
			_selectedProperty = properties [pValueID].propertyName;
			_inputField.text = _waterfallMats[0].GetFloat (_selectedProperty).ToString();
			break;
		}
		_activeInput.SetActive (true);

	}
}
