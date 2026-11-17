using System;

#if EXTERNAL_EDITOR
public class Follow_ChatMessage : CPHInlineBase
#else
public class CPHInline
#endif

{
    public bool Execute()
    {
        if (!CPH.TryGetArg("user", out string user))
        {
            CPH.LogWarn("Follow_ChatMessage: no 'user' argument found.");
            return true;
        }

        // Change this message to whatever you want to say when someone follows your channel.
        string message = $"Thank you for following, {user}! You are now my favourite follower!";
        
        CPH.SendMessage(message, true, true);
        return true;
    }
}