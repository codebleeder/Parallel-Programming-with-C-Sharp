# Parallel Programming with C#

* Task is a unit of work that takes a function.
* Tasks can be passed as an object
* Tasks can return values
* Tasks can report their states

## Creating and starting Tasks

Tasks get executed on separate threads.

## Canceling tasks
The recommended approach:
- Instantiate a CancelationTokenSource and pass it's token in a thread. 
- Cancel the token source
- Throw OperationCanceledException inside the thread when the token is cancelled. 

Throwing OperationCanceledException inside the thread won't affect the main thread. 

## Waiting in tasks
* We waste CPU cycles in SpinWait() and SpinUntil()

# Data sharing and synchronization
An operation is **Atomic** if it cannot be interrupted.
This is atomic:
```cs
x = 1;
```
This is not atomic as it is made up of two operations:
```cs
x++;
```
Non-atomic operations are vulnerable to race conditions

## Critical sections
A **critical section** is a piece of code which only one thread can execute. 

```cs
lock(object)
```
lock is a short-hand for:
```cs
Monitor.Enter(); // and
Monitor.Exit();
```

**SpinLock** does not support lock recursion. Recursion is dangerous. Watch out for recursive logic. 

**Mutex** controls access to a resource.

## Mutex vs lock, interlocked, spinlock
Mutex, Lock, Interlocked, and SpinLock are all synchronization mechanisms in C# that are used to manage concurrent access to shared resources in multi-threaded applications. Each of these mechanisms has its advantages and is suitable for different scenarios. Here's a comparison of the advantages of Mutex over Lock, Interlocked, and SpinLock:

**Advantages of Mutex:**

1. **Cross-Process Synchronization**: Mutex can be used for inter-process synchronization, making it suitable for coordinating access to resources shared across different processes. Lock, Interlocked, and SpinLock are primarily for intra-process synchronization.

2. **Wait Timeout**: Mutex allows you to specify a timeout for waiting to acquire the mutex. This can help prevent deadlocks and allows a thread to proceed if it can't acquire the mutex within a specified time.

3. **Release from Different Thread**: A Mutex can be released by a different thread than the one that acquired it. This can be useful in scenarios where a thread acquires the mutex and another thread needs to release it based on certain conditions.

4. **Named Mutex**: Mutexes can have names, making them easier to coordinate among multiple parts of an application or across different applications.

**Advantages of Lock:**

1. **Simplicity**: Lock is straightforward to use and is the simplest synchronization mechanism. It is often used for protecting critical sections of code within the same process.

2. **Implicit Monitor**: Lock is built on top of the monitor (or "lock") built into the .NET runtime, making it easy to use and efficient for most in-process synchronization scenarios.

**Advantages of Interlocked:**

1. **Atomic Operations**: Interlocked provides atomic operations, such as increment, decrement, compare-and-swap, etc. These operations are very efficient and are useful for low-level synchronization and performance-critical scenarios.

2. **Low Overhead**: Interlocked operations have low overhead and are generally faster than Mutex for simple atomic operations.

**Advantages of SpinLock:**

1. **Low Contention Scenarios**: SpinLock is designed for low-contention scenarios where contention for a resource is infrequent. It is efficient when lock acquisition times are expected to be very short.

2. **Lightweight**: SpinLock is lightweight and can be faster than Mutex in cases where contention is minimal.

**Considerations**:

- The choice between these synchronization mechanisms depends on the specific requirements of your application and the characteristics of the concurrent access you're dealing with.
- Mutex is the most versatile but may introduce more overhead than Lock, Interlocked, or SpinLock, so it's often used in scenarios that require cross-process synchronization.
- Lock is typically used for simple in-process synchronization and is the easiest to work with.
- Interlocked and SpinLock are best suited for low-level, performance-critical operations where atomicity is essential, but they require careful handling to avoid issues like deadlocks.

It's important to choose the synchronization mechanism that best matches your application's needs, taking into account factors like performance, simplicity, and the type of resource you're protecting.

## ReaderWriterLock
* We can make them support lock recursion.
* Note: Lock recursion is not recommended. 
* We can create UpgradableReadLock(). Using this, Depending on the condition we can upgrade a read-lock to a write-lock.

## Task Coordination
**Barrier**  
- Barrier lets us define stages in our operation
- For example, we can define all worker threads must finish "stage 1" before beginning "stage 2" using Barrier