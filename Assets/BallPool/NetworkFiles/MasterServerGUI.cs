using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class MasterServerGUI : MonoBehaviour 
{
	public string gameName = "Player Name";
	private string skill = "3";

#if UNITY_NETWORK
	[SerializeField]
	private int serverPort = 25013;
	[SerializeField]
	private float lastHostListRequest = -1000.0f;
	[SerializeField]
	private float hostListRefreshTimeout = 10.0f;
	
	private ConnectionTesterStatus natCapable = ConnectionTesterStatus.Undetermined;
	private bool filterNATHosts = false;
	private bool probingPublicIP = false;
	private bool doneTesting = false;
	private float timer = 0.0f;
	private string serverPortStr = "2015";
	private string serverIp = "127.0.0.1";

	private string testMessage = "Undetermined NAT capabilities";
	private bool useNat = false;
	private Vector2 scrollPosition = Vector2.zero;
	private Vector2 listScrollPosition = Vector2.zero;
	
	[System.NonSerialized]
	public NetworkPlayer[] players;
	private List<NetworkPlayer> playersList;
#elif PHOTON_PUN
	public string oldGameName = "Player Name";
	private string roomName = "myRoom";
	private bool connectFailed = false;
	private bool roomIsCreated = false;
	private bool joinededInRoom = false;

	private PhotonPlayer otherPlayer;
	private PhotonView photonView;
	private Vector2 listScrollPosition = Vector2.zero;

    
#elif PHOTON_NETWORK
   //
#endif
	void OnGUI () 
	{
		//ShowServerGUI();
	}

	void Awake ()
	{
		name = "MasterServerGUI";
#if UNITY_NETWORK
		NetworkView nv = gameObject.AddComponent<NetworkView> ();
		nv.stateSynchronization = NetworkStateSynchronization.Off;
		nv.observed = this;
#elif PHOTON_PUN
		photonView = gameObject.AddComponent<PhotonView> ();
		photonView.ObservedComponents = new List<Component>(0);
		photonView.ObservedComponents.Add(this);
		photonView.viewID = 1;

		// this makes sure we can use PhotonNetwork.LoadLevel() on the master client and all clients in the same room sync their level automatically
		PhotonNetwork.automaticallySyncScene = true;
		
		// the following line checks if this client was just created (and not yet online). if so, we connect
		if (PhotonNetwork.connectionStateDetailed == PeerState.PeerCreated)
		{
			// Connect to the photon master-server. We use the settings saved in PhotonServerSettings (a .asset file in this project)
			PhotonNetwork.ConnectUsingSettings("0.1");
		}
		

#elif PHOTON_NETWORK
		//
#endif
	}
	void OnEnable () 
	{
		ServerController.serverController = gameObject.GetComponent<ServerController>();
		ServerController.logicAI = gameObject.GetComponent<LogicAI>();

		string savedMyName = PlayerPrefs.GetString("BallPoolMultyplayerServerTemplateDemoPlayerName");
		if(savedMyName != "")
		{
			gameName = savedMyName;

		}

		name = "MasterServerGUI";
		ServerController.serverController.myName = gameName;
		int coins = Profile.GetUserDate(ServerController.serverController.myName + "_Coins");
		if(coins > 0)
		{
			ServerController.serverController.coins = coins;
		}
#if UNITY_NETWORK
		natCapable = Network.TestConnection();
		// What kind of IP does this machine have? TestConnection also indicates this in the
		// test results
		//if (Network.HavePublicAddress())
			//Debug.Log("This machine has a public IP address");
		//else
			//Debug.Log("This machine has a private IP address");
#elif PHOTON_PUN
		oldGameName = gameName;
		PhotonNetwork.playerName = gameName;
		roomName = "Room" + ", Prize - " + ServerController.serverController.prize;
#elif PHOTON_NETWORK
        
#endif
	}
	
	void Update()
	{
#if UNITY_NETWORK
		// If test is undetermined, keep running
		if (!doneTesting)
		{
			TestConnection();
		}
#endif
	}
	public static void Disconnect ()
	{
#if UNITY_NETWORK
		Network.Disconnect ();
#elif PHOTON_PUN
		PhotonNetwork.LeaveRoom();
#elif PHOTON_NETWORK

#endif
	}

#if UNITY_NETWORK
	void TestConnection() 
	{
		// Start/Poll the connection test, report the results in a label and react to the results accordingly
		natCapable = Network.TestConnection();

		switch (natCapable)
		{
		case ConnectionTesterStatus.Error: 
			testMessage = "Problem determining NAT capabilities";
			doneTesting = true;
			break;
			
		case ConnectionTesterStatus.Undetermined: 
			testMessage = "Undetermined NAT capabilities";
			doneTesting = false;
			break;
		case ConnectionTesterStatus.LimitedNATPunchthroughPortRestricted:
			testMessage = "Limited NAT punchthrough capabilities. Cannot "+
				"connect to all types of NAT servers. Running a server "+
					"is ill advised as not everyone can connect.";
			useNat = true;
			doneTesting = true;
			break;
			
		case ConnectionTesterStatus.LimitedNATPunchthroughSymmetric:
			testMessage = "Limited NAT punchthrough capabilities. Cannot "+
				"connect to all types of NAT servers. Running a server "+
					"is ill advised as not everyone can connect.";
			useNat = true;
			doneTesting = true;
			break;
			
		case ConnectionTesterStatus.NATpunchthroughAddressRestrictedCone:
		case ConnectionTesterStatus.NATpunchthroughFullCone:
			testMessage = "NAT punchthrough capable. Can connect to all "+
				"servers and receive connections from all clients. Enabling "+
					"NAT punchthrough functionality.";
			useNat = true;
			filterNATHosts = true;
			doneTesting = true;
			break;
		case ConnectionTesterStatus.PublicIPIsConnectable:
			testMessage = "Directly connectable public IP address.";
			useNat = false;
			doneTesting = true;
			break;
			
			// This case is a bit special as we now need to check if we can 
			// use the blocking by using NAT punchthrough
		case ConnectionTesterStatus.PublicIPPortBlocked:
			testMessage = "Non-connectble public IP address (port " + serverPort +" blocked),"
				+" running a server is impossible.";
			useNat = false;
			// If no NAT punchthrough test has been performed on this public IP, force a test
			if (!probingPublicIP)
			{
				//Debug.Log("Testing if firewall can be circumnvented");
				natCapable = Network.TestConnectionNAT();
				probingPublicIP = true;
				timer = Time.time + 10;
			}
			// NAT punchthrough test was performed but we still get blocked
			else if (Time.time > timer)
			{
				probingPublicIP = false; // reset
				useNat = true;
				doneTesting = true;
			}
			break;
		case ConnectionTesterStatus.PublicIPNoServerStarted:
			testMessage = "Public IP address but server not initialized,"
				+"it must be started to check server accessibility. Restart connection test when ready.";
			break;
		default: 
			testMessage = "Error in test routine, got " + natCapable;
			break;
		}

	}
#endif

	void ShowGUI ()
	{
		if (!ServerController.serverController)
		{
			return;
		}
		gameName = GUI.TextField(new Rect(0.5f*Screen.width - 50,0.5f*Screen.height - 15,100,20), gameName);

		if (GUI.Button(new Rect(0.5f*Screen.width - 75,50 + 0.5f*Screen.height - 15,150,40),"Play with AI"))
		{
			ServerController.serverController.myName = gameName;
			PlayerPrefs.SetString("BallPoolMultyplayerServerTemplateDemoPlayerName", gameName);
			ServerController.serverController.isMyQueue = Random.Range(0, 2) == 0;
			MenuControllerGenerator.controller.playWithAI = true;
			ServerController.serverController.otherName = "Player AI";
			int otherCoins = Profile.GetUserDate(ServerController.serverController.otherName + "_Coins");
			if(otherCoins > 0)
			{
				ServerController.serverController.otherCoins = otherCoins;
			}
			MenuControllerGenerator.controller.LoadLevel(MenuControllerGenerator.controller.game);
		}
		
		GUI.Label(new Rect(0.5f*Screen.width + 95,60 + 0.5f*Screen.height - 15, 50,40), "AI Skill");
		skill = GUI.TextField(new Rect(0.5f*Screen.width + 165, 60 + 0.5f*Screen.height - 15, 50, 20), skill);
		int skillInt = 3;
		if(int.TryParse(skill, out skillInt))
		{
			MenuControllerGenerator.controller.AISkill = Mathf.Clamp(skillInt, 1, 3);
			skill = MenuControllerGenerator.controller.AISkill.ToString();
		} else
		{
			skill = skillInt.ToString();
		}
		
		if (GUI.Button(new Rect(0.5f*Screen.width - 50,120 + 0.5f*Screen.height - 15,100,50),"Training"))
		{
			ServerController.serverController.myName = gameName;
			PlayerPrefs.SetString("BallPoolMultyplayerServerTemplateDemoPlayerName", gameName);
			MenuControllerGenerator.controller.LoadLevel(MenuControllerGenerator.controller.game);
			ServerController.serverController = null;
			gameObject.SetActive(false);
		}
		if (!ServerController.serverController)
		{
			return;
		}
		GUI.Label(new Rect(330,10,120,20)," Prize ");

		string prize = GUI.TextField(new Rect(370,10,50,20), ServerController.serverController.prize.ToString());
		//GUI.Label(new Rect(330,45,500,20), "Coins - " + ServerController.serverController.coins.ToString());
		GUI.Box(new Rect(330,45,120,20), "Coins - " + ServerController.serverController.coins.ToString());
		int tryPrize;
		if(int.TryParse(prize, out tryPrize))
		{
			ServerController.serverController.prize = Mathf.Clamp( tryPrize , ServerController.serverController.minCoins, 500);
		}
	}
	void ShowServerGUI()
	{
		if(MenuControllerGenerator.controller.playWithAI)
		{
			return;
		}
#if UNITY_NETWORK

		if (Network.peerType == NetworkPeerType.Disconnected)
		{
			if(!ServerController.serverController)
			{
				return;
			}
			ShowGUI ();
			if (GUI.Button (new Rect(10,40,90,30),"Connect"))
			{
				if(gameName == "")
				{
					//Debug.LogError("Empty game name given during host registration");
				}
				else
				{
					Network.Connect(serverIp, serverPort, "VaghoPoolGame");
				}
			}
			if(Network.peerType == NetworkPeerType.Disconnected)
			{
				Debug.Log("Test message: " + testMessage);
			}
			// Start a new server

			if(Network.peerType != NetworkPeerType.Server)
			{
				GUI.Label(new Rect(110,10,90,20)," Server Port ");
				serverPortStr = GUI.TextField(new Rect(200,10,120,20), serverPortStr);
			}




			GUI.Label(new Rect(110,45,90,20)," Connection IP ");
			serverIp = GUI.TextField(new Rect(200,45,120,20), serverIp);

			if(!int.TryParse(serverPortStr, out serverPort))
				return;
			if (GUI.Button(new Rect(10,10,90,30),"Create Server"))
			{
				if(gameName == "")
				{
					//Debug.LogError("Empty game name given during host registration");
				}
				else
				{
					Network.InitializeServer(32, serverPort + ServerController.serverController.prize, useNat);
					Network.incomingPassword = "VaghoPoolGame";
					MasterServer.updateRate = 3;
					MasterServer.RegisterHost("BallPoolMultyplayerServerTemplateDemo", "Server" + ", Prize - " + ServerController.serverController.prize, "testing the 8 ball pool multiplayer game template");
				}
			}
			
			// Refresh hosts
			if (GUI.Button(new Rect(10,70,210,30),"Refresh available Servers") 
			    || Time.realtimeSinceStartup > lastHostListRequest + hostListRefreshTimeout)
			{
				MasterServer.ClearHostList();
				MasterServer.RequestHostList ("BallPoolMultyplayerServerTemplateDemo");
				lastHostListRequest = Time.realtimeSinceStartup;
				//Debug.Log("Refresh Click");
			}
			
			HostData[] data = MasterServer.PollHostList();
			
			int _cnt = 0;
			listScrollPosition = GUI.BeginScrollView (new Rect (20,120,230,310), listScrollPosition, new Rect (20, 120, 180, 230 + 35 * data.Length));

			foreach (HostData element in data)
			{
				// Do not display NAT enabled games if we cannot do NAT punchthrough
				if ( !(filterNATHosts && element.useNat))
				{
					//string name = element.gameName + " " + element.connectedPlayers + " / " + element.playerLimit;
					string hostInfo;
					hostInfo = "[";
					// Here we display all IP addresses, there can be multiple in cases where
					// internal LAN connections are being attempted. In the GUI we could just display
					// the first one in order not confuse the end user, but internally Unity will
					// do a connection check on all IP addresses in the element.ip list, and connect to the
					// first valid one.
					foreach (string host in element.ip)
					{
						hostInfo = hostInfo + host + ":" + element.port + " ";
					}
					hostInfo = hostInfo + "]";
					
					if (GUI.Button(new Rect(20,30 + (_cnt*50)+90,200,40),"Connect to " + element.gameName))
					{
						if(gameName == "")
						{
							//Debug.LogError("Empty game name given during host registration");
						}
						else
						{
							// Enable NAT functionality based on what the hosts if configured to do
							useNat = element.useNat;
							Network.Connect(element.ip, element.port, "VaghoPoolGame");	

						}
						
					}
					_cnt ++;
				}
			}
			GUI.EndScrollView();
		} else
		if (Network.isServer)
		{
			GUI.Label(new Rect(170,10,250,20)," Ip: " + Network.player.ipAddress);
			GUI.Label(new Rect(170,30,250,50)," Connections: " + Network.connections.Length + "\n Prize:            " + ServerController.serverController.prize);

			if (GUI.Button (new Rect(10,10,150,30),"Kill Server"))
			{
				Network.Disconnect();
				MasterServer.UnregisterHost();
				//Debug.Log("Disconnect");
			}
			GUI.Label(new Rect(10,50,150,30)," Players ");
			scrollPosition = GUI.BeginScrollView (new Rect (10,110,550,110), scrollPosition, new Rect (10, 110, 500, 100 + 20 * ServerController.serverController.players.Count));
			int i = 0;

			foreach (KeyValuePair<int, ServerController.Player> playerDictionary in ServerController.serverController.players)
			{
				ServerController.Player player = playerDictionary.Value;
				string otherPlayerName = player.otherPlayer == null? "No player": player.otherPlayer.name;
				GUI.Label(new Rect(10,110 + 30*i,500,20),"     Id: " + player.networkPlayer.ToString() + ", Name: " + player.name + ", Coins: " + player.coins + ", Other Player: " + otherPlayerName);
				i ++;
			}
			GUI.EndScrollView ();
		} else if (Network.isClient && Application.loadedLevel == 0)
		{
			GUI.Label(new Rect(170,10,250,60), " Coins: " + ServerController.serverController.coins);
			GUI.Label(new Rect(170,30,250,60), "Wait for other players or disconnect");
            
			if (GUI.Button (new Rect(10,10,150,30),"Disconnect from server"))
			{
				Network.Disconnect();
                //Debug.Log("Disconnect");
            }
		}
#elif PHOTON_PUN


		if (!PhotonNetwork.connected)
		{
			if (PhotonNetwork.connecting)
			{
				GUILayout.Label("Connecting to: " + PhotonNetwork.ServerAddress);
			}
			else
			{
				GUILayout.Label("Not connected. Check console output. Detailed connection state: " + PhotonNetwork.connectionStateDetailed + " Server: " + PhotonNetwork.ServerAddress);
			}
			
			if (this.connectFailed)
			{
				GUILayout.Label("Connection failed. Check setup and use Setup Wizard to fix configuration.");
				GUILayout.Label(System.String.Format("Server: {0}", new object[] {PhotonNetwork.ServerAddress}));
				//GUILayout.Label("AppId: " + PhotonNetwork.PhotonServerSettings.AppID);
				
				if (GUILayout.Button("Try Again", GUILayout.Width(100)))
				{
					this.connectFailed = false;
					PhotonNetwork.ConnectUsingSettings("0.1");
				}
			}
			ShowGUI ();
			return;
		}
		else
		{
			if(!roomIsCreated && !joinededInRoom)
			{
				if (GUI.Button (new Rect(110,10,210,30),"Disconect from Photon"))
				{
					this.connectFailed = true;
					PhotonNetwork.Disconnect();
	            }
		    }
        }
		if(!roomIsCreated && !joinededInRoom)
		{
			if(!ServerController.serverController)
			{
				return;
            }
            ShowGUI ();

			if(oldGameName != gameName)
			{
				oldGameName = gameName;
				ServerController.serverController.myName = gameName;
				int coins = Profile.GetUserDate(ServerController.serverController.myName + "_Coins");
				if(coins > 0)
				{
					ServerController.serverController.coins = coins;
				}

				PlayerPrefs.SetString("BallPoolMultyplayerServerTemplateDemoPlayerName", gameName);
			}
			if(!ServerController.serverController)
			{
				return;
            }
			roomName = "Room" + ", Prize - " + ServerController.serverController.prize;

			if (ServerController.serverController.coins >= ServerController.serverController.prize && GUI.Button(new Rect(10,10,90,30), "Create Room"))
			{
				ServerController.serverController.isMyQueue = true;

				ServerController.serverController.isFirstPlayer = true;

				PhotonNetwork.CreateRoom(this.roomName, new RoomOptions() { maxPlayers = 2 }, null);
			}
			float y = 0.0f;

			listScrollPosition = GUI.BeginScrollView (new Rect (10,55,200,310), listScrollPosition, new Rect (10, 55, 180, 200 + 35 * PhotonNetwork.GetRoomList().Length));

			foreach (RoomInfo room in PhotonNetwork.GetRoomList())
			{
				int prize = int.Parse( room.name.Replace("Room" + ", Prize - ","") );
				if (room.playerCount == (int)room.maxPlayers)
				{
					if(GUI.Button (new Rect(10,55 + 35 * y,170,30), room.name + " is busy"))
					{
						Debug.Log(room.name + " now is busy");
					}
				}else if (prize <= ServerController.serverController.coins)
				{
					if(GUI.Button (new Rect(10,55 + 35 * y,170,30),"Join " + room.name))
					{
						ServerController.serverController.isMyQueue = false;
						ServerController.serverController.isSecondPlayer = false;
						ServerController.serverController.prize = prize;
						PhotonNetwork.JoinRoom(room.name);
					}
				} else 
				{
					if(GUI.Button (new Rect(10,55 + 35 * y,170,30),"!!! " + room.name))
					{
						Debug.Log("You can not join to the " + room.name + " you have little coins");
					}
				}

				y += 1.0f;
			}
			GUI.EndScrollView();

		} else
		{
			if(roomIsCreated && Application.loadedLevel == 0)
			{
				GUI.Label(new Rect(170,10,250,20)," Ip: " + PhotonNetwork.networkingPeer.ServerAddress);

				if (GUI.Button (new Rect(10,10,150,30),"Left Room"))
				{
					Disconnect();
                }

            } else if(joinededInRoom && Application.loadedLevel == 0)
			{
				GUI.Label(new Rect(170,10,250,60), " Coins: " + ServerController.serverController.coins);
				GUI.Label(new Rect(170,30,250,60), "Wait for other players or disconnect");
				
				if (GUI.Button (new Rect(10,10,150,30),"Disconnect from room"))
				{
					Disconnect();
                }
            }
        }
        #elif PHOTON_NETWORK

#else
		ShowGUI ();
#endif

	}

	public void SendRPCToServer (string message, params object[] args)
	{
		if (MenuControllerGenerator.controller.playWithAI)
			return;
#if UNITY_NETWORK
		if (!ServerController.serverController.GetComponent<NetworkView>())
			return;

		ServerController.serverController.GetComponent<NetworkView>().RPC (message, RPCMode.Server, args);
#elif PHOTON_PUN
		if (!photonView)
			return;
		System.Reflection.MethodInfo methodInfo = ServerController.serverController.GetType().GetMethod(message);
		methodInfo.Invoke(ServerController.serverController, args);
#elif PHOTON_NETWORK

#endif
	}
	//Send message thru the network for some network  player
	public void SendRPCToNetworkPlayer (string message, int player, params object[] args)
	{
		if (MenuControllerGenerator.controller.playWithAI)
			return;
#if UNITY_NETWORK
		if (!ServerController.serverController.GetComponent<NetworkView>())
			return;
		ServerController.serverController.GetComponent<NetworkView>().RPC (message, players[player], args);
#elif PHOTON_PUN
		if (!photonView)
			return;
		photonView.RPC (message, otherPlayer, args);
#elif PHOTON_NETWORK


#endif
	}

	public void SetCoinsToPlayer(int networkPlayer, int coins)
	{
#if UNITY_NETWORK
		ServerController.serverController.FindPlayer(networkPlayer).coins = coins;
#elif PHOTON_PUN

#elif PHOTON_NETWORK
		ServerController.serverController.FindPlayer(networkPlayer).coins = coins;
#endif
	}


#if UNITY_NETWORK


	void OnFailedToConnectToMasterServer(NetworkConnectionError info)
	{
		Debug.Log(info);
	}
	
	void OnFailedToConnect(NetworkConnectionError info) 
	{
		Debug.Log(info);
	}
	void OnDisconnectedFromServer(NetworkDisconnection info)
	{
		if (Network.isServer)
			Debug.Log("Local server connection disconnected");
		else
			if (info == NetworkDisconnection.LostConnection)
				Debug.Log("Lost connection to the server");
		else
			Debug.Log("Successfully diconnected from the server");
		ServerController.serverController.isMyQueue = true;
		MenuControllerGenerator.controller.LoadLevel("GameStart");
		ServerController.serverController.isFirstPlayer = false;
		ServerController.serverController.isSecondPlayer = false;

	}
	void OnPlayerDisconnected(NetworkPlayer player) 
	{
		Debug.Log("Clean up after player " + player);
		ServerController.serverController.DeletePlayer(int.Parse(player.ToString()));

		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
	}
	void OnServerInitialized () 
	{
		//Debug.Log("Server initialized and ready");
		ServerController.serverController.players = new Dictionary<int, ServerController.Player>();
		playersList = new List<NetworkPlayer>(0);
		ServerController.serverController.myNetworkPlayer = 0;
		ServerController.serverController.otherNetworkPlayer = 0;
		playersList.Add (Network.player);
		players = playersList.ToArray ();
	}
	void OnConnectedToServer()
	{
		//Debug.Log("Connected to server");
	}
	void OnPlayerConnected(NetworkPlayer player) 
	{
		Debug.Log("Player " + player + " connected from " + player.ipAddress + ":" + player.port);
		playersList.Add (player);
		players = playersList.ToArray ();
		int playerId = int.Parse (player.ToString ());
		ServerController.serverController.SendRPCToNetworkPlayer ("SetMyNetworkPlayerClient", playerId, playerId);

	}
#elif PHOTON_PUN
	void OnPhotonPlayerConnected ( PhotonPlayer newPlayer )
	{
		Debug.Log("OnPhotonPlayerConnected " + newPlayer.ID);
		otherPlayer = newPlayer;
		ServerController.serverController.otherNetworkPlayer = otherPlayer.ID;
		ServerController.serverController.myName = gameName;
		ServerController.serverController.SendRPCToServer("SendName", ServerController.serverController.otherNetworkPlayer, ServerController.serverController.myName);
		ServerController.serverController.StartGame ();
	}
	void OnPhotonPlayerDisconnected (PhotonPlayer otherPlayer)
	{
		PhotonNetwork.LeaveRoom();
		PhotonNetwork.RemoveRPCs(otherPlayer);
		PhotonNetwork.DestroyPlayerObjects(otherPlayer);
	}
	public void OnCreatedRoom()
	{
		roomIsCreated = true;
		Debug.Log("OnCreatedRoom");
	}
	public void OnJoinedRoom()
	{
		ServerController.serverController.otherNetworkPlayer = 0;
		if(!roomIsCreated)
		{
		    otherPlayer = PhotonNetwork.masterClient;
			ServerController.serverController.otherNetworkPlayer = otherPlayer.ID;
		}
		joinededInRoom = true;
		Debug.Log("OnJoinedRoom");
		ServerController.serverController.myName = gameName;
		int coins = Profile.GetUserDate(ServerController.serverController.myName + "_Coins");
		if(coins > 0)
		{
			ServerController.serverController.coins = coins;
		}
		PlayerPrefs.SetString("BallPoolMultyplayerServerTemplateDemoPlayerName", gameName);

		if (!roomIsCreated)
		{
			ServerController.serverController.SendRPCToServer("SendName", ServerController.serverController.otherNetworkPlayer, ServerController.serverController.myName);
			ServerController.serverController.StartGame ();
		}
	}
	void OnLeftRoom()
	{
		roomIsCreated = false;
		joinededInRoom = false;
		ServerController.serverController.isMyQueue = true;
		MenuControllerGenerator.controller.LoadLevel("GameStart");
		ServerController.serverController.isFirstPlayer = false;
		ServerController.serverController.isSecondPlayer = false;
	}

    public void OnFailedToConnectToPhoton(object parameters)
	{
		this.connectFailed = true;
		Debug.Log("OnFailedToConnectToPhoton. StatusCode: " + parameters + " ServerAddress: " + PhotonNetwork.networkingPeer.ServerAddress);
	}
#elif PHOTON_NETWORK

#endif

    public void onPlayAI () {
        ServerController.serverController.myName = gameName;
        PlayerPrefs.SetString ("BallPoolMultyplayerServerTemplateDemoPlayerName", gameName);
        ServerController.serverController.isMyQueue = Random.Range (0, 2) == 0;
        MenuControllerGenerator.controller.playWithAI = true;
        ServerController.serverController.otherName = "Player AI";
        int otherCoins = Profile.GetUserDate (ServerController.serverController.otherName + "_Coins");
        if(otherCoins > 0) {
            ServerController.serverController.otherCoins = otherCoins;
        }
        MenuControllerGenerator.controller.LoadLevel (MenuControllerGenerator.controller.game);
    }
}
