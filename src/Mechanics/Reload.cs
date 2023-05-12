using Microsoft.Xna.Framework;
using System;

namespace Dyhar.src.Mechanics;

public enum ReloadState
{
    NotStarted,
    Reloading,
    Finished
}

public class Reload
{
    public string Name { get; private set; }
    public ReloadState State { get; private set; }
    public int ReloadTimeInMilliseconds { get; set; }
    public TimeSpan PassedTime => CurrentTime - StartTime;

    public Reload(string name, int reloadTimeInMilliseconds)
    {
        Name = name;
        ReloadTimeInMilliseconds = reloadTimeInMilliseconds;
        State = ReloadState.NotStarted;
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
    }

    /// <summary> 
	/// Changing state to NotStarted if was maken action that should be called after finishing this reload.
    /// Not necessary but desirable.
	/// </summary>
    public void CompletedFinishedCheck()
    {
        State = ReloadState.NotStarted;
    }

    private TimeSpan StartTime { get; set; }
    private TimeSpan CurrentTime { get; set; }
    private TimeSpan EndTime { get; set; }

    private bool wasStartedWithoutTime = false;

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
