using System;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

[Serializable]
public class Shitter
{
    private static int _currentShitterId;

    public int Id;
    public string Name;
    public float ShitAmmount;
    public Sprite SpriteShitter;
    public SocialPosition SocialPosition;

    public DialogId LastDialogId;

    #region Static members
    
    public static Dictionary<SocialPosition, List<ShitterDialogs>> StoriesBySocialPosition { get; private set; }
    public static Dictionary<DialogId, string> DialogByDialogId { get; private set; }
    public static Dictionary<DialogId, DialogReplyTuple> DialogReplyById { get; private set; }
    public static Dictionary<DialogId, DialogReplyTuple> PlayerReplyById { get; private set; }

    #region Bake

    public static void BakeDialogs()
    {
        BakeDialogBySocialPosition();
        BakeDialogById();
        BakeDialogreplyById();
    }

    private static void BakeDialogBySocialPosition()
    {
        StoriesBySocialPosition = new Dictionary<SocialPosition, List<ShitterDialogs>>();

        var allStories = ScriptableObjectHolder.Instance.GameDatabase.ShitterDialogs;

        for (int i = 0; i < allStories.Count; i++)
        {
            if (!StoriesBySocialPosition.ContainsKey(allStories[i].SocialPosition))
                StoriesBySocialPosition[allStories[i].SocialPosition] = new List<ShitterDialogs>();

            StoriesBySocialPosition[allStories[i].SocialPosition].Add(allStories[i]);
        }
    }

    private static void BakeDialogById()
    {
        DialogByDialogId = new Dictionary<DialogId, string>();

        var allDialogs = ScriptableObjectHolder.Instance.GameDatabase.AllDialogs;

        for (int i = 0; i < allDialogs.Count; i++)
        {
            DialogByDialogId[(DialogId)Enum.Parse(typeof(DialogId), allDialogs[i].Id)] = allDialogs[i].Dialog;
        }
    }

    private static void BakeDialogreplyById()
    {
        DialogReplyById = new Dictionary<DialogId, DialogReplyTuple>();
        PlayerReplyById = new Dictionary<DialogId, DialogReplyTuple>();


        var allDialogsReplies = ScriptableObjectHolder.Instance.GameDatabase.Replies;
        for (int i = 0; i < allDialogsReplies.Count; i++)
        {
            DialogReplyById[allDialogsReplies[i].Dialog] = allDialogsReplies[i];
        }
        

        allDialogsReplies = ScriptableObjectHolder.Instance.GameDatabase.PlayerReplies;
        for (int i = 0; i < allDialogsReplies.Count; i++)
        {
            PlayerReplyById[allDialogsReplies[i].Dialog] = allDialogsReplies[i];
        }
    }

    #endregion

    #endregion

    public float TimeShitting
    {
        get { return ShitAmmount * Random.Range(.7f, 1.2f); }
    }

    public Shitter()
    {
        Id = _currentShitterId++;
    }

    #region Messages

    public string GetMessageForPlayer()
    {
        var messagesOfMySocialPosition = StoriesBySocialPosition[SocialPosition];
        var possibleDialogs = messagesOfMySocialPosition[Random.Range(0, messagesOfMySocialPosition.Count)];
        var dialog = possibleDialogs.Dialogs[Random.Range(0, possibleDialogs.Dialogs.Count)];
        LastDialogId = dialog;
        return DialogByDialogId[LastDialogId];
    }

    public string Accepted()
    {
        var dialogReply = DialogReplyById[LastDialogId];
        return DialogByDialogId[dialogReply.AcceptDialog];

    }

    public string Denied()
    {
        var dialogReply = DialogReplyById[LastDialogId];
        if (dialogReply.ThreatIfDenyed)
        {
            SoundManager.Instance.PlayAudio(AudioId.Threat);
            GameManager.Instance.ThreatCount++;
        }
        return DialogByDialogId[dialogReply.DenyDialog];
    }

    #endregion
}
