using UnityEngine;
using System.Collections.Generic;

public class GameDatabaseScriptableObject : ScriptableObject
{
    public string InitialMessage;
    public string LetterFromCleric;
    public string MessageForHouseThrashed;
    public string TitleMessage;

    public List<string> Names;
    public List<DialogIdTuple> AllDialogs;
    public List<ShitterDialogs> ShitterDialogs;
    public List<DialogReplyTuple> Replies;
    public List<ShitterDialogs> PlayerAcceptReplies;
    public List<ShitterDialogs> PlayerDeniesReplies;

    public List<EndOptionMessageTuple> MessageForEndGame;
    public List<string> WakeupMessages;

    public List<SocialPositionSpriteTuple> SpriteBySocialPosition;
}
