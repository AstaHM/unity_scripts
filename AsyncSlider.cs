using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MyActivityClass:Monoeahviour{///Este script debe ser asignado a la camara
	public GameObject LodingScreenObj; //el objeto donde se encuentra el slider que se quiere usar
	public Slider slider;              //la slider que cambiara dependiendo del la sincronizacion de la escena

	AsyncOperation async;

	public	 void LoadScreenExample(int LVL)
	{

		StartCoroutine(LoadingScreen(LVL));//inicializacion de funcion de carga asincrona de scena
	}

	IEnumerator LoadingScreen(int lvl)
	{
		LoadingScreenObj.SetActive(true);         // se permite la vizualizacion del slider
		async = SceneManager.LoadSceneAsync(lvl); //el administrador de scenas busca cargar una scena de forma asincorna dentro de una operacion
		async.allowsSceneActivation=false;      //de momento au  si la carga de la scena es exitosa debido al bajo peso de la misma se 
												//mantiene es background

		while(async.isDone==false)   //si async.isDone == true se intercambia a la scena posterior mientras no
		{
			slider.value=async.progress; //el valor de slider= al valor del progreso asincrono
			if(async.progress==0.9f)     //si el vlor del progreso se encuentre en .9 que es el indicativo de listo en ejecucion para la sigueinte escena
			{
				slider.value =1f;                  //se avanza el slider a uno indicando que la carga de la scena esta lista
				async.allowsSceneActivation= true; //el modo asyncrono de carga de la scena libera la scena siguiente
			}
			yield return null;
		}
	}
}
