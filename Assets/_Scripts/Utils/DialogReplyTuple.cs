using System;

[Serializable]
public class DialogReplyTuple
{
    public DialogId Dialog;
    public DialogId AcceptDialog;
    public DialogId DenyDialog;

    public bool ThreatIfDenyed;
}