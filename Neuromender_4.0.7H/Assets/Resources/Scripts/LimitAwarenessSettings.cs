using UnityEngine;
using System.Collections;

public static class LimitAwarenessSettings
{
    public const string DAILY_LIMIT_REACHED_MESSAGE = "You have reached the playtime limit for the day for {0}.";
    public const string SESSION_LIMIT_REACHED_MESSAGE = "You have reached the playtime limit for {0} for this session. Please take a break for {1} hours before commencing another session.";
    public const string LAST_SESSION_LIMIT_MESSAGE = "You have reached the session playtime limit for {0} in your previous session. You may commence another session after {1}.";
    public const string ALL_GAMES_PLAYED = "You have played all of the games available to you today.";
    public const string NO_GAMES_SET = "Please contact your clinician as no games are available.";
    public const string COME_BACK_AT = "You have reached the playtime limit for {0} for this session. Please come back at {1} to start another session.";
}
