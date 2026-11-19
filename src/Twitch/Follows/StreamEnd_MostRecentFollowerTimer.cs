using System;

/*----- Class name should match <filename>.cs -----*/
#if EXTERNAL_EDITOR
public class StreamEnd_MostRecentFollowerTimer : CPHInlineBase
#else
public class CPHInline
#endif
/*--------------------------------------------*/
{
    public bool Execute()
    {
        string mostRecentFollowerUserId = CPH.GetGlobalVar<string>("mostRecentFollowerUserId", false);

        if (string.IsNullOrEmpty(mostRecentFollowerUserId))
        {
            return true; // nobody followed this stream
        }

        long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        long followedAt = CPH.GetTwitchUserVarById<long>(mostRecentFollowerUserId, "userFollowedAt", true);

        if (followedAt > 0)
        {
            CPH.SetTwitchUserVarById(mostRecentFollowerUserId, "mostRecentFollowerDuration", currentTime - followedAt, true);
        }

        // Clear it so the next stream starts fresh
        CPH.UnsetGlobalVar("mostRecentFollowerUserId", false);

        return true;    
        
    }
}

