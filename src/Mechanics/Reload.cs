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
    public TimeSpan PassedTime => CurrentTime - StartTime;

    public Reload(int reloadTimeInMilliseconds)
    {
        ReloadTimeInMilliseconds = reloadTimeInMilliseconds;
        State = ReloadState.NotStarted;
        finishAction = TypesUtils.EmptyFunction;
    }

    public Reload(int reloadTimeInMilliseconds, Action finishAction) 
        : this(reloadTimeInMilliseconds)
    {
        this.finishAction = finishAction;
    }

    public void Start()
    {
        wasStartedWithoutTime = true;
    }

    public void OnUpdate(GameTime gameTime)
    {
        if (wasStartedWithoutTime)
        {
            Start(gameTime);
            wasStartedWithoutTime = false;
        }

        if (State == ReloadState.Reloading)
        {
            CurrentTime = gameTime.TotalGameTime;
            if (CurrentTime > EndTime)
                State = ReloadState.Finished;
        }

        if (State == ReloadState.Finished)
        {
            finishAction();
            State = ReloadState.NotStarted;
        }
    }

    private TimeSpan StartTime { get; set; }
    private TimeSpan CurrentTime { get; set; }
    private TimeSpan EndTime { get; set; }

    private bool wasStartedWithoutTime = false;
    private Action finishAction;

    private void Start(GameTime gameTime)
    {
        if (State != ReloadState.NotStarted)
            return;

        StartTime = gameTime.TotalGameTime;
        if (ReloadTimeInMilliseconds == 0)
            throw new ArgumentException();
        EndTime = StartTime + new TimeSpan(0, 0, 0, ReloadTimeInMilliseconds / 1000, ReloadTimeInMilliseconds % 1000);
        State = ReloadState.Reloading;
    }
}
