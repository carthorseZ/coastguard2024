var periodicTaskProcessor = new TimedTaskProcessor();

function StartEventPolling(PollCount) {
    if (typeof (PollCount) == "undefined" || PollCount == null || isNaN(PollCount) || PollCount < 0) {
        PollCount = -1;
    }
    
    periodicTaskProcessor.Tasks =
		[
			{ Name: "EventPoller", Quanta: 8, Handler: BEGetEvents, Count: 0, MaxCount: PollCount}
		];
    periodicTaskProcessor.Start();

}

function StopEventPolling() {
    periodicTaskProcessor.Stop();
    periodicTaskProcessor.Tasks = null;
}



function TimedTaskProcessor() {
    /// <summary>
    /// A class that controls a central timer which invokes tasks that have been registered with it
    /// </summary>

    this.Tasks = null; //  a collection of the records of tasks to be performed.
    this.Quantum = 250; // the base frequency of the timer (all tasks are run in multiples of these units)
    this.TimerIsRunning = false;
    this.Timer = null;

    this.Start = function() {
        /// <summary>
        /// starts the timer ticking
        /// </summary>
        this.Timer = window.setInterval(function() { periodicTaskProcessor.ProcessTasks(); }, this.Quantum);
    }

    this.StartTimerTask = function(timerName) {
        for (var idx in this.Tasks) {
            var task = this.Tasks[idx];
            if (task && task.TimerTracker && task.TimerTracker.id == timerName) {
                task.TimerTracker.StartTracking();
            }
        }
    }

    this.StopTimerTask = function(timerName) {
        for (var idx in this.Tasks) {
            var task = this.Tasks[idx];
            if (task && task.TimerTracker && task.TimerTracker.id == timerName) {
                task.TimerTracker.StopTracking();
            }
        }
    }

    this.GetTimerHitCount = function(timerName) {
        for (var idx in this.Tasks) {
            var task = this.Tasks[idx];
            if (task && task.Name == timerName) {
                return task.Count;
            }
        }
    }

    this.GetTimerQuanta = function(timerName) {
        for (var idx in this.Tasks) {
            var task = this.Tasks[idx];
            if (task && task.Name == timerName) {
                return task.Quanta;
            }
        }
        return null;
    }

    this.SetTimerQuanta = function(timerName, newQuanta) {
        for (var idx in this.Tasks) {
            var task = this.Tasks[idx];
            if (task && task.Name == timerName) {
                task.Quanta = newQuanta;
            }
        }
    }

    this.GetTimerTracker = function(timerName) {
        var result = null;

        for (var idx in this.Tasks) {
            var task = this.Tasks[idx];
            if (task && task.TimerTracker && task.TimerTracker.id == timerName) {
                result = task.TimerTracker;
            }
        }

        return result;
    }

    this.Stop = function() {
        /// <summary>
        /// stops the timer ticking
        /// </summary>
        window.clearInterval(this.Timer);
        this.Timer = null;
    }

    this.ProcessTasks = function() {
        /// <summary>
        /// processes each of the tasks in the task list (if their turn has come round)
        /// </summary>
        for (var index in this.Tasks) {
            var item = this.Tasks[index];
            if ((item.Count % item.Quanta) == 0 && (item.Count < (item.MaxCount*item.Quanta)) || item.MaxCount < 0) {
                try {
                    item.Handler(item);
                }
                catch (ex) { } // catch everything - nothing should be allowed to break this timer.
            }
            item.Count++; // would need to poll for 34 years at 250ms for this to overflow.
        }
    }
}
