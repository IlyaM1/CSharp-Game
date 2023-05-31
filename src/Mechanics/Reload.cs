using Microsoft.Xna.Framework;
using System;

using Dyhar.src.Utils;

namespace Dyhar.src.Mechanics;

public enum ReloadState
{
    NotStarted,
    Reloading,
    Finished
}

public class Reload
{
    public ReloadState State { get; private set; }
    public int ReloadTimeInMilliseconds { get; set; }
    public TimeSpan PassedTimeInMilliseconds => CurrentTimeInMilliseconds - StartTimeInMilliseconds;

    public Reload(int reloadTimeInMilliseconds)
    {
        ReloadTimeInMilliseconds = reloadTimeInMilliseconds;
        State = ReloadState.NotStarted;
        _finishAction = TypesUtils.EmptyFunction;
    }

    public Reload(int reloadTimeInMilliseconds, Action finishAction) 
        : this(reloadTimeInMilliseconds)
    {
        _finishAction = finishAction;
    }

    public void Start()
    {
        _wasStartedWithoutTime = true;
    }

    public void UpdatingEventHandler(GameTime gameTime)
    {
        if (_wasStartedWithoutTime)
        {
            _start(gameTime);
            _wasStartedWithoutTime = false;
        }

        if (State == ReloadState.Reloading)
        {
            CurrentTimeInMilliseconds = gameTime.TotalGameTime;
            if (CurrentTimeInMilliseconds > EndTimeInMilliseconds)
                State = ReloadState.Finished;
        }

        if (State == ReloadState.Finished)
        {
            _finishAction();
            State = ReloadState.NotStarted;
        }
    }

    private TimeSpan StartTimeInMilliseconds { get; set; }
    private TimeSpan CurrentTimeInMilliseconds { get; set; }
    private TimeSpan EndTimeInMilliseconds { get; set; }

    private bool _wasStartedWithoutTime = false;
    private Action _finishAction;

    private void _start(GameTime gameTime)
    {
        if (State != ReloadState.NotStarted)
            return;

        StartTimeInMilliseconds = gameTime.TotalGameTime;
        if (ReloadTimeInMilliseconds == 0)
            throw new ArgumentException();
        EndTimeInMilliseconds = StartTimeInMilliseconds + new TimeSpan(0, 0, 0, ReloadTimeInMilliseconds / 1000, ReloadTimeInMilliseconds % 1000);
        State = ReloadState.Reloading;
    }
}
