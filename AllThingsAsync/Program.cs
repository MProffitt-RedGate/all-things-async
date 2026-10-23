using AllThingsAsync;

BadThreadingDemo.RunBad();
BadThreadingDemo.RunSlightlyBetter();
BadThreadingDemo.RunSafelyWithLock();
BadThreadingDemo.RunATimedLock();
_ = Task.Run(BadThreadingDemo.RunALikelyDeadlock);

// Thread Sync

ThreadSynchronization.RunBadWaitingThread();
ThreadSynchronization.RunGoodWaitingThread();