using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameDatabaseScriptableObject : ScriptableObject
{
    public List<string> Names;
    public List<string> Stories;

    public List<string> AcceptedStories;
    public List<string> DenyiedStories;

    public List<string> AcceptedReplies;
    public List<string> DenyiesReplies;

    public List<string> TwiceADayMessages;

    public List<string> WakeupMessages;
}
