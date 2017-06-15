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

	[SerializeField]
	private GameObject[] _waterfallParts;
	[SerializeField]
	private GameObject _ocean;
	[SerializeField]
	private GameObject _foam;
	[SerializeField]
	private GameObject _fog;
	[SerializeField]
	public List<Property> properties;

	private List<Material> _waterfallMats = new List<Material> ();
	private Material _oceanMat;

	private GameObject _activeInput;
	private Property _selectedProperty;

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
		FLOAT,
		COLOR
	}

	[System.Serializable]
	public class Property
	{
		public PropertyType type;
		public string propertyName;
		public RangeProperty range;
	}

	//store the materials we want to change and set the slider to the default value
	void Start ()
	{
		_oceanMat = _ocean.GetComponent<MeshRenderer> ().material;
		foreach (GameObject WaterfallPart in _waterfallParts) {
			_waterfallMats.Add (WaterfallPart.GetComponent<SkinnedMeshRenderer> ().material);
		}	

		UpdateInput (0);

	}

	//update the position of the foam and fog particle emiters
	void Update ()
	{
		float waterDropOff = _waterfallMats [0].GetFloat ("_WaterDropOff");
		float waterfallLength = _waterfallMats [0].GetFloat ("_WaterfallLength");

		//aim the fog emiter towards the base of the waterfall
		_fog.transform.LookAt (transform.position);

		//aproximate location of where the waterfall hits the ocean
		float x = (waterfallLength / ((waterDropOff * 0.1f))) - 8;
		Vector3 pos = _foam.transform.position;
		pos.z = Mathf.Lerp (0, x, (transform.position.y - pos.y) * 0.05f);
		_foam.transform.position = pos;

		
	}

	//set the float value of the currently selected property of the materials
	public void SetSelectedProperty (float pValue)
	{
		if (_selectedProperty == null)
			return;
		foreach (Material WaterfallMat in _waterfallMats) {
			WaterfallMat.SetFloat (_selectedProperty.propertyName, pValue);
		}
	}

	//converts the string value to float or color depending on the currently selected property
	public void SetSelectedProperty (string pValue)
	{
		switch (_selectedProperty.type) {
		case PropertyType.FLOAT:
			float value;
			if (float.TryParse (pValue, out value)) {
				foreach (Material WaterfallMat in _waterfallMats) {
					WaterfallMat.SetFloat (_selectedProperty.propertyName, value);
				}
			} else {
				Debug.LogWarning ("invalid float");
			}
			break;
		case PropertyType.COLOR:
			Color colorvalue;
			if (ColorUtility.TryParseHtmlString (pValue, out colorvalue)) {
				_oceanMat.SetColor (_selectedProperty.propertyName, colorvalue);
				foreach (Material WaterfallMat in _waterfallMats) {
					WaterfallMat.SetColor (_selectedProperty.propertyName, colorvalue);
				}
			}else {
				Debug.LogWarning ("invalid color");
			}
			break;
		}
		


	}

	//select a property and set the value of the slider/text field
	public void UpdateInput (int pValueID)
	{
		_selectedProperty = null;
		if (_activeInput != null)
			_activeInput.SetActive (false);
	
		switch (properties [pValueID].type) {
		case PropertyType.RANGE:
			_activeInput = _slider.gameObject;
			_slider.minValue = properties [pValueID].range.min;
			_slider.maxValue = properties [pValueID].range.max;
			_selectedProperty = properties [pValueID];
			_slider.value = _waterfallMats [0].GetFloat (_selectedProperty.propertyName);
			break;
		case PropertyType.FLOAT:
			_activeInput = _inputField.gameObject;
			_inputField.contentType = InputField.ContentType.DecimalNumber;
			_selectedProperty = properties [pValueID];
			_inputField.text = _waterfallMats [0].GetFloat (_selectedProperty.propertyName).ToString ();
			break;
		
		case PropertyType.COLOR:
			_activeInput = _inputField.gameObject;
			_inputField.contentType = InputField.ContentType.Standard;
			_selectedProperty = properties [pValueID];
			_inputField.text = "#" + ColorUtility.ToHtmlStringRGB (_waterfallMats [0].GetColor (_selectedProperty.propertyName));
			break;
		}
		_activeInput.SetActive (true);

	}
}
