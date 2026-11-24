using System;
using System.Collections.Generic;
using System.Linq;
public class MilestoneRule
{
    public string Name;
    public Func<int, bool> Matches;
    public Func<int, string> Suffix;
}

#if EXTERNAL_EDITOR
public class Follow_MilestoneNumbers : CPHInlineBase
#else
public class CPHInline
#endif

{
    private const string FiredVarName = "firedMilestones";

    private static readonly Dictionary<int, string> Custom = new Dictionary<int, string>
    {
        // Add your own custom milestones here. The key is the follower count, and the value is the message to send.
        // Example:
        // { 5000, "Halfway to 10k!" },
    };
    private List<MilestoneRule> _rules;
  
    
    public bool Execute()
    {
       if (_rules == null) _rules = BuildRules();

        if (!CPH.TryGetArg("followercount", out string followerCount))
        {
            CPH.LogWarn("Follow_MilestoneNumbers: no 'followercount' argument found.");
            return true;
        }
         if (!int.TryParse(followerCount, out int count))
        {
            CPH.LogWarn($"Follow_MilestoneNumbers: '{followerCount}' is not a number.");
            return true;
        }

        if (!CPH.TryGetArg("user", out string user))
        {
            user = "anonymous";   // fallback if the argument is missing
        }

        string message = $"Thanks for following {user} You are my #{count} follower!";

        MilestoneRule rule = _rules.FirstOrDefault(r => r.Matches(count));

        if (rule != null)
        {
            HashSet<int> fired = LoadFired();

            // Add() returns true only if the number was new, so each number fires once
            if (fired.Add(count))
            {
                SaveFired(fired);
                message += " " + rule.Suffix(count);   // add the rule's text to the end
            }
        }


        return true;
    }
     private List<MilestoneRule> BuildRules()
    {
        return new List<MilestoneRule>
        {
            new MilestoneRule { Name = "Custom value",
                                Matches = n => Custom.ContainsKey(n),
                                Suffix  = n => Custom[n] },

            new MilestoneRule { Name = "Every 1000",
                                Matches = n => n % 1000 == 0,
                                Suffix  = n => "Huge milestone!" },

            new MilestoneRule { Name = "Every 100",
                                Matches = n => n % 100 == 0,
                                Suffix  = n => "Milestone!" },

            new MilestoneRule { Name = "Ends in 69",
                                Matches = n => n % 100 == 69,
                                Suffix  = n => "Nice." },

            new MilestoneRule { Name = "Consecutive digits",
                                Matches = IsConsecutive,
                                Suffix  = n => "Consecutive numbers!" },

            new MilestoneRule { Name = "Palindrome",
                                Matches = IsPalindrome,
                                Suffix  = n => "Palindrome!" },
        };
    }
    private static bool IsPalindrome(int n)
    {
        if (n < 100) return false;   // stops 1-9, 11, 22... counting
        string s = n.ToString();
        return s.SequenceEqual(s.Reverse());   // same forwards and backwards?
    }

    private static bool IsConsecutive(int n)
    {
        string s = n.ToString();
        if (s.Length < 4) return false;   // needs at least 4 digits, e.g. 1234

        bool ascending = true, descending = true;
        for (int i = 1; i < s.Length; i++)
        {
            if (s[i] != s[i - 1] + 1) ascending = false;    // each digit one higher?
            if (s[i] != s[i - 1] - 1) descending = false;   // each digit one lower?
        }
        return ascending || descending;   // 1234 and 4321 both count
    }
    private HashSet<int> LoadFired()
    {
        // The 'true' means persisted: it survives closing Streamer.bot
        string saved = CPH.GetGlobalVar<string>(FiredVarName, true) ?? "";

        var set = new HashSet<int>();
        foreach (string part in saved.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
        {
            if (int.TryParse(part, out int value)) set.Add(value);
        }
        return set;
    }

    private void SaveFired(HashSet<int> fired)
    {
        CPH.SetGlobalVar(FiredVarName, string.Join(",", fired), true);
    }
}  