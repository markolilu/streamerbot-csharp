using System;

#if EXTERNAL_EDITOR
public class Follow_MostRecentFollowerTimer : CPHInlineBase
#else
public class CPHInline
#endif

{
    public bool Execute()
    {
        
        long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        
        // 1. check if the global variable "mostRecentFollowerUserId" exists. If it doesn't, set it to the current follower's userId and set their "userFollowedAt" variable to the current time. Then reurn true to exit function.
        string mostRecentFollowerUserId = CPH.GetGlobalVar<string>("mostRecentFollowerUserId", false);

        if (string.IsNullOrEmpty(mostRecentFollowerUserId))
        {
            if (!CPH.TryGetArg("userId", out string userId))
            {
                CPH.LogWarn("Follow_MostRecentTimer: no 'userId' argument found.");
                return true;
            }

            CPH.SetGlobalVar("mostRecentFollowerUserId", userId, false);
            CPH.SetTwitchUserVarById(userId, "userFollowedAt", currentTime, true);
            return true;
        }



        // 2. compare the CURRENT TIME to the assigned value of WHEN the last follower followed.
        // this is how LONG that follower was the most recent follower.

        long userFollowedAt = CPH.GetTwitchUserVarById<long>(mostRecentFollowerUserId, "userFollowedAt", true);

        if (userFollowedAt > 0)
        {
            CPH.SetTwitchUserVarById(mostRecentFollowerUserId, "mostRecentFollowerDuration", currentTime - userFollowedAt, true);
        }

        // 3. set the global variable "mostRecentFollowerUserId" to the current follower's userId and set their "userFollowedAt" variable to the current time.
        if (!CPH.TryGetArg("userId", out string UserId))
        {
            CPH.LogWarn("Follow_MostRecentTimer: no 'userId' argument found.");
            return true;
        }

        CPH.SetGlobalVar("mostRecentFollowerUserId", UserId, false);
        CPH.SetTwitchUserVarById(UserId, "userFollowedAt", currentTime, true);

        return true;
    }
}

