using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vuforia;
using System.IO;


public class Obb : MonoBehaviour {

	private string nextScene = "Main";//cargamos la scena por medio de un string mas adelante
	private bool obbisok = false; //variable de verificacion de status de expantion file .obb
	private bool loading = false;
	private bool replacefiles = false; //true if you wish to over copy each time

		private string[] paths ={
			"Vuforia/ASTA_Media.dat",   //aqui van los archivos de Vuforia entontrados en project/Assets/Streaming assets
			"Vuforia/ASTA_Media.xml",
		};

		void Update()
		{
			if (Application.platform == RuntimePlatform.Android) // mientras la paltaforma se encuentre en android 
			{
				if (Application.dataPath.Contains(".obb") && !obbisok) // y si la aplicacion contiene un obb  y oobisok=false
				{
					StartCoroutine(CheckSetUp());                     //inicializa la busqueda de el archivo.obb
					obbisok = true;									  //
				}
			}
			else
			{
				if (!loading) /// si la carga es falsa 
				{
					StartApp(); /// inicia la app y carga la siuiente escena
				}
			}
		}


		public void StartApp()               //funcion de inicio de carga de la siguiente scena
		{
			loading = true;                   
			SceneManager.LoadScene(nextScene);
		}

		public IEnumerator CheckSetUp()
		{
			//Check and install!
			for (int i = 0; i < paths.Length; ++i)
			{
				yield return StartCoroutine(PullStreamingAssetFromObb(paths[i]));

			}
			yield return new WaitForSeconds(3f);
			StartApp();
		}

		//Alternatively with movie files these could be extracted on demand and destroyed or written over
		//saving device storage space, but creating a small wait time.
		public IEnumerator PullStreamingAssetFromObb(string sapath)
		{
			if (!File.Exists(Application.persistentDataPath + sapath) || replacefiles)
			{
				WWW unpackerWWW = new WWW(Application.streamingAssetsPath + "/" + sapath);
				while (!unpackerWWW.isDone)
				{
					yield return null;
				}
				if (!string.IsNullOrEmpty(unpackerWWW.error))
				{
					Debug.Log("Error unpacking:" + unpackerWWW.error + " path: " + unpackerWWW.url);

					yield break; //skip it
				}
				else
				{
					Debug.Log("Extracting " + sapath + " to Persistant Data");

					if (!Directory.Exists(Path.GetDirectoryName(Application.persistentDataPath + "/" + sapath)))
					{
						Directory.CreateDirectory(Path.GetDirectoryName(Application.persistentDataPath + "/" + sapath));
					}
					File.WriteAllBytes(Application.persistentDataPath + "/" + sapath, unpackerWWW.bytes);
					//could add to some kind of uninstall list?
				}
			}
			yield return 0;
		}
}
