using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameDatabaseScriptableObject : ScriptableObject
{
    public List<string> Names;
    public List<string> Stories;
    
    public List<string> AcceptedReplies;
    public List<string> DenyedReplies;

    public List<string> TwiceADayMessages;

    public List<string> WakeupMessages;

    public string InitialMessage;

    public List<SocialPositionSpriteTuple> SpriteBySocialPosition = new List<SocialPositionSpriteTuple>();
}
