using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour 
{
	public bool useNetwork = true;
	[System.NonSerialized]
	public bool playWithAI = false;
	[System.NonSerialized]
	public int AISkill = 1;
	[System.NonSerialized]
	public bool canRotateCue = true;
	public delegate void LoadLevelHandler(MenuController menuController);
	public event LoadLevelHandler OnLoadLevel;
	public GameObject masterServerGUI;
	[SerializeField]
	private GameObject masterServerGUIPrefab;
	public Preloader preloader;
	public string loader = "Loader";
	public string game = "Game";
	public Camera loaderCamera;
	public Camera guiCamera;
	[System.NonSerialized]
	public bool IsFirstTimeStarted = false;
	[System.NonSerialized]
	public bool isPaused = false;
	[System.NonSerialized]
	public string levelName = "";
	[System.NonSerialized]
	public int levelNumber = -1;
	
	[System.NonSerialized]
	public bool LoaderIsDoneUnload = false;

	[System.NonSerialized]
	public float progress;

	[System.NonSerialized]
	public bool canControlCue = true;

	[System.NonSerialized]
	public bool loadIsComplite = false;
	public bool isTouchScreen = false;
	//public GameObject layerHelp;
	private float fps = 25.0f;
	private float fixedDeltaTime = 0.01f;
	private float oldFixedDeltaTime = 0.01f;

	public GameObject root;

	public void Pause (bool pause)
	{
		isPaused = pause;
		if(isPaused)
		Time.timeScale = 0.0f;
		else
		Time.timeScale = 1.0f;
	}
	void Awake ()
	{
		preloader.controller = this;
	}
	public void OnGoBack ()
	{
		MenuControllerGenerator.controller.LoadLevel("GameStart");
		root.SetActive (true);
		StartCoroutine(WaitAndActivateMasterServerGUI ());
	}
	IEnumerator WaitAndActivateMasterServerGUI ()
	{
		while(!loadIsComplite)
		{
			yield return null;
		}
		masterServerGUI.SetActive(true);
	}
	public void OnStart ()
	{
		if(useNetwork)
		{
			masterServerGUI = GameObject.Instantiate(masterServerGUIPrefab) as GameObject;
			masterServerGUI.transform.parent = transform;
		}
		else
		{
			LoadLevel(game);
		}
	}
	void OnEnable ()
	{

	}
	void FixedUpdate ()
	{
		if(fps < 15.0f)
		{
			fixedDeltaTime = 0.02f;
		}
		else
		{
			fixedDeltaTime = 0.01f;
		}
		if(oldFixedDeltaTime != fixedDeltaTime)
		{
			oldFixedDeltaTime = fixedDeltaTime;
			Time.fixedDeltaTime = fixedDeltaTime;
		}
	}
	void Update ()
	{
#if !UNITY_EDITOR && UNITY_ANDROID 
		if(Input.GetKeyDown(KeyCode.Escape))
			Application.Quit();
#endif
		fps = 1.0f/Time.deltaTime;
		if(Input.GetKeyDown(KeyCode.P))
		{
			if(!isPaused)
			{
				Pause (true);
			}
			else
			if(isPaused)
			{
				Pause (false);
			}
		}
	}
	public Menu CreatedMenuFromResources (string path)
	{
		Menu menuRes = Resources.Load(path, typeof(Menu)) as Menu;
		return CreatedMenu(menuRes);
	}
	public Menu CreatedMenu (Menu menuRes)
	{
		Menu menu = Menu.Instantiate(menuRes) as Menu;
		menu.guiCamera = guiCamera;
		if(guiCamera)
		menu.transform.parent = guiCamera.transform;
		return menu;
	}
	public void LoadLevel (string levelName)
	{
		this.levelName = levelName;
		this.levelNumber = -1;
		StartCoroutine( Load () );
	}
	public void LoadLevel (int levelNumber)
	{
		this.levelName = "";
		this.levelNumber = levelNumber;
		StartCoroutine( Load () );
	}
	IEnumerator Load ()
	{
		preloader.SetState(true);

		if(OnLoadLevel != null)
			OnLoadLevel(this);
		loadIsComplite = false;
		if(loaderCamera)
		loaderCamera.enabled = true;
		
		if(guiCamera)
		guiCamera.enabled = false;
		
		LoaderIsDoneUnload = false;
		progress = 0.0f;
		preloader.UpdateLoader( 0.0f );

		Application.LoadLevel(loader);

		yield return StartCoroutine(UpdateLoader ());
		yield return new WaitForEndOfFrame();
		
		preloader.UpdateLoader(1.0f);
	
		yield return null;
		loadIsComplite = true;
	}
	IEnumerator UpdateLoader ()	
	{
		while(Application.loadedLevelName != loader)
		{
			yield return null;
		}
		if(levelName != "")
		{
			while(levelName != Application.loadedLevelName)
			{
				if(LoaderIsDoneUnload)
				{
					progress = 0.8f;
					preloader.UpdateLoader( 0.8f );

				}
				yield return null;
			}
		}
		else
		{
			while(levelNumber != Application.loadedLevel)
			{
				if(LoaderIsDoneUnload)
				{
					progress = 0.8f;
					preloader.UpdateLoader( 0.8f );
				}
					yield return null;
			}
		}
		if(Application.loadedLevel != 0)
		{
			if(guiCamera)
				guiCamera.enabled = true;
			
			if(loaderCamera)
				loaderCamera.enabled = false;

			preloader.SetState(false);
		}

	}

    /*===========================================================================================================================*/

	public void onClickCN () {
		root.SetActive (false);
        masterServerGUIPrefab.GetComponent<MasterServerGUI> ().onPlayAI ();
    }
}
