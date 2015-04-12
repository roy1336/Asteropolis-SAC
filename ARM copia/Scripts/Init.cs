﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Init : TouchLogicalOneTouch {
	public int noBombas;
	public Button inicio;
	public GameObject newBomb;
	public Text textBomb;
	public Image messageBackground;
	public Image messageContent;

	private Ray ray1;
	private RaycastHit hit1;
	private GameObject aster;
	private bool moverBomba;
	private Vector3 posicion;
	private int currentBomb;
	private const int TOTAL_BOMBS = 7;
	private List<GameObject> bombs;
	private GameObject cameraMain;

	// Use this for initialization
	void Start () {
		noBombas = (int) TOTAL_BOMBS;
		textBomb.text = "0"+noBombas.ToString ();
		inicio.onClick.AddListener(()=> showMsg());
		bombs= new List<GameObject>();
		cameraMain = GameObject.Find ("Main Camera");
		aster = GameObject.Find ("Main Camera/Aster");
		aster.transform.position.Set(cameraMain.camera.transform.position.x ,cameraMain.camera.transform.position.y, -cameraMain.camera.transform.position.z);
		moverBomba = false;

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		CheckTouches ();
	}
	private void showMsg(){

		if (noBombas != TOTAL_BOMBS) {
			Main script;
	
			inicio.onClick.RemoveListener (()=>showMsg());

			script = GetComponent<Main> ();

			script.enabled = true;
		}
	}
	

	public override void OnTouchBeganAnywhere(){
		ray1 = cameraMain.camera.ScreenPointToRay (touch.position);
		if (Physics.Raycast (ray1, out hit1) && hit1.transform.name == "Bomb(Clone)") {
			currentBomb = bombs.FindIndex(x => x.transform.position.Equals(hit1.transform.position));
			moverBomba=true;
		}
		else if(noBombas>0){
			posicion = touch.position;
			posicion.z = -cameraMain.camera.transform.position.z + 2;
			posicion = cameraMain.camera.ScreenToWorldPoint (posicion);
			bombs.Add ((GameObject)Instantiate (newBomb, posicion, Quaternion.identity));
			currentBomb=TOTAL_BOMBS - noBombas;
			bombs[currentBomb].transform.SetParent(cameraMain.transform);
			noBombas-= 1;
			textBomb.text = "0"+noBombas.ToString();
			moverBomba=true;
		}
	}
	public override void OnTouchEndedAnywhere(){
		ray1 = new Ray(bombs[currentBomb].transform.position,new Vector3(0,0,1));
		if (!(Physics.Raycast (ray1,out hit1) && hit1.transform.name == "Aster")) {
			GameObject bombToDelete = bombs[currentBomb];
			noBombas+= 1;
			textBomb.text = "0"+noBombas.ToString();
			bombs.RemoveAt(currentBomb);
			Object.DestroyObject(bombToDelete);
		} 
		moverBomba = false;
		
	}
	public override void OnTouchMovedAnywhere(){
		if(moverBomba){
			posicion = touch.position;
			posicion.z = -cameraMain.camera.transform.position.z;
			posicion = cameraMain.camera.ScreenToWorldPoint (posicion);
			bombs[currentBomb].transform.position=posicion;
		}
	}
	public override void OnTouchStationaryAnywhere(){

	}

	public List<GameObject> getBombs(){
		return bombs;
	}

}