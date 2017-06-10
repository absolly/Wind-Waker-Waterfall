using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanTide : MonoBehaviour {

    [SerializeField]
    private float _tideDelay;
    [SerializeField]
    private float _tideChangeSpeed;
    [SerializeField]
    private float _tideHeigth;

    private float _tideTimer;
    private bool _highTide;
    private float _tideStartHeigth;
    private float _tideChangeTimer;

	void Start () {
        _tideTimer = _tideDelay;
        _tideStartHeigth = transform.position.y;

    }
	
	// Update is called once per frame
	void Update () {
        _tideTimer -= Time.deltaTime;
        if (_tideTimer <= 0) {
            _highTide = !_highTide;
            if (_highTide)
            {
                transform.position = new Vector3(0, Mathf.Lerp(transform.position.y, _tideStartHeigth + _tideHeigth, -_tideTimer), 0);
            } else {
                transform.position = new Vector3(0, Mathf.Lerp(_tideStartHeigth, transform.position.y, -_tideTimer), 0);
            }
           
                _tideTimer = _tideDelay;
        }
	}
}
