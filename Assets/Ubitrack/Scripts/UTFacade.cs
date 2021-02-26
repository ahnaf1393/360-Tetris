using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Globalization;
using System.Collections.Generic;

namespace FAR{

	public class UTFacade : MonoBehaviour {
		
		
		public string ubitrackComponentsPath="";	
		public string logConfig="";	
		public TextAsset dataflowFile=null;
		public bool DoNotDestroyOnLoad = false;
        public float UbiToUnityScaleFactor = 1.0f;
	    
		protected SimpleFacade m_facade = null;
        protected DFGParser dfgParser = null;
        protected string m_dfgString;
	    protected HashSet<UbiTrackComponent> appComponents = new HashSet<UbiTrackComponent>();
	    protected bool initialized = false;
		protected HashSet<UbiTrackComponent> appComponentsToStart = new HashSet<UbiTrackComponent>();

        protected bool m_UbitrackError = false;
		
		// Use this for initialization
		
		void Awake(){
			if(DoNotDestroyOnLoad)
				DontDestroyOnLoad(this);
            UbiMeasurementUtils.UbiToUnityScaleFactor = this.UbiToUnityScaleFactor;
		}

        void OnLevelWasLoaded() 
        {
            Debug.Log("LEVEL LOADED");
            UbiTrack_Allocator myAlloc = FindObjectOfType<UbiTrack_Allocator>();
            if (myAlloc != null) 
            {
                this.UbiToUnityScaleFactor = myAlloc.UbiScaleFactor;
                UbiMeasurementUtils.UbiToUnityScaleFactor = this.UbiToUnityScaleFactor;
            }
        }
		
	    void OnEnable()
	    {
	        if (initialized) return;

			string dfgString="";	  		
	        if (ubitrackComponentsPath == null )
	        {
	            throw new MissingMemberException("ubitrack component path is empty, ubitrack can't work");
	        }
			
			#if !UNITY_ANDROID || UNITY_EDITOR		
	
			string dir = Path.GetFullPath(Application.dataPath+Path.DirectorySeparatorChar+"..")+Path.DirectorySeparatorChar;
			
			DirectoryInfo dirInfo = new DirectoryInfo(dir);
			FileInfo[] filesInDir = dirInfo.GetFiles();
			
			foreach(FileInfo f in filesInDir) 
			{			
				if(f.Extension.Equals(".dfg")) 
				{
					Debug.Log("Found dfg in project folder:"+f.FullName);
					StreamReader reader = f.OpenText();				
					dfgString = reader.ReadToEnd();
					reader.Close();
					break;
				}
					
			}
			
	       
			
			if(logConfig.Length > 0) {			
				string logfile = dir+logConfig;
                
				if(File.Exists(logfile))
				{
					Debug.Log("Init UbiTrack Logging with file:" + 	logfile);
	                ubitrack.initLogging(logfile);
				} else {
					Debug.LogError("UbiTrack log file does not exist: "+logfile);
				}
				
			}
			#else
			var unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
			var activity = unity.GetStatic<AndroidJavaObject>("currentActivity");
			// Retrieve ApplicationInfo and nativeLibraryDir (N.B. API-9 or newer only!)
			var info = activity.Call<AndroidJavaObject>("getApplicationInfo");
			ubitrackComponentsPath = info.Get<String>("nativeLibraryDir");		
			
			Ubitrack.ubitrack.initLogging("TRACE");
			#endif
			
			
			
			if (dfgString.Length == 0)
	        {
				if(dataflowFile != null) 
				{
					dfgString = dataflowFile.text;
				} 
				else 
				{
	            	throw new MissingMemberException("ubitrack dataflow is empty, ubitrack can't work");
				}
	        }



            
                

            m_facade = new SimpleFacade(ubitrackComponentsPath);
            m_UbitrackError = m_facade.getLastError() != null;

            if (m_UbitrackError)
            {
                Debug.LogError("Error creating ubitrack simple facade, aborint starting ubitrack: " + m_facade.getLastError());
            }
            Debug.Log("sad" + dfgString);
            m_dfgString = dfgString;
	        //dfgParser = new DFGParser(dfgString);
		
			
			
	
	        
			
	        
	        
		}
	
	    void Start()
	    {
	        if (initialized || m_UbitrackError) return;


            String dfgString = m_dfgString;// dfgParser.getDFG();
            Debug.Log("Loading dfg file: " + dfgString);
            m_facade.loadDataflowString(dfgString);

            m_UbitrackError = m_facade.getLastError() != null;
            if (m_UbitrackError)
            {
                Debug.LogError("Error loading ubitrack dfg file: " + m_facade.getLastError());
                return;
            }                                        
            
            startUbiTrack();
            
            m_UbitrackError = m_facade.getLastError() != null;
            if (m_UbitrackError)
            {                
                Debug.Log("Error starting ubitrack: " + m_facade.getLastError());
                return;
            }

            initialized = true;

        }
		
		void Update(){
			if(!initialized) return;
			if(appComponentsToStart.Count == 0) return;
			
			foreach (UbiTrackComponent comp in appComponentsToStart)
	        {
	            comp.utStart();
	        }
			
			appComponentsToStart.Clear();
			
		}
		
		public void startUbiTrack(){
			foreach (UbiTrackComponent comp in appComponents)
	        {
	                comp.utInit(m_facade);                                        
	        }
	           
	
	        m_facade.startDataflow();
	
	        foreach (UbiTrackComponent comp in appComponents)
	        {
	            comp.utStart();
	        }
		}
		
		public void stopUbiTrack(){
			if (m_facade == null || !initialized)
	            return;
	    
	        Debug.Log("stopUbiTrack, Shutting down ubitrack, components:" + appComponents.Count);
	
	        foreach (UbiTrackComponent comp in appComponents)
	        {
	            if (comp != null)
	            {
	                comp.utStop();
	            }
	
	        }
	
	        m_facade.stopDataflow();
	        
	        
	        
	
	        foreach (UbiTrackComponent comp in appComponents)
	        {
	            if (comp != null)
	            {
	                comp.utDestroy();
	            }
	
	        }
	
	        m_facade.clearDataflow();
	
	
	        //m_facade.killEverything();
	    	m_facade = null;
			initialized = false;
			Debug.Log("Ubitrack shut down");
		}
	    
	    void OnApplicationQuit()
	    {
			stopUbiTrack();               
	    }	
	
	    internal void registerUbitrackComponent(UbiTrackComponent ubiTrackComponent)
	    {        
	        appComponents.Add(ubiTrackComponent);
			if(initialized){
				ubiTrackComponent.utInit(m_facade);
				appComponentsToStart.Add(ubiTrackComponent);			
			}
	    }
			
	
	    internal System.Xml.XmlDocument getDFG()
	    {
            return null;// dfgParser.getRoot();
	    }
	}

}

