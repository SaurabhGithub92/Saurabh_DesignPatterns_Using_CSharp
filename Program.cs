public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Hello to Design Patterns");

        Console.WriteLine("Singleton Design Pattern - NotificationManager");
        var notificationManager = NotificationManager.Instance;
        notificationManager.SendNotification("Hello, Singleton");

        Console.WriteLine("Factory Design Pattern - NotificationFactory");
        var factory = new NotificationFactory();
        INotification notification = factory.CreateNotification("Email");
        notification.SendMessage("Hello, Factory!");

        Console.WriteLine("Observer Design Pattern - NotificationObserver");
        var service = new NotificationObserver();
        var emailSubscriber = new EmailNotificationSubscriber();
        var smsSubscriber = new SmsNotificationSubscriber();

        service.Subscribe(emailSubscriber);
        service.Subscribe(smsSubscriber);

        service.NotifySubscribers("Hello, Observe!");

        Console.WriteLine("Strategy Design Pattern - NotificationStrategyManager");
        var strategyContext = new NotificationStrategyManager();

        strategyContext.SetStrategy(new EmailNotificationStrategy());
        strategyContext.SendStrategyNotification("Hello email Strategy!");

        strategyContext.SetStrategy(new SmsNotificationStrategy());
        strategyContext.SendStrategyNotification("Hello SMS Strategy!");

        Console.WriteLine("Decorator Design Pattern - BasicNotificationDecorator");
        INotificationDecorator notification1 = new BasicNotification();
        Console.WriteLine(notification1.SendMessage());

        notification1 = new EmailDecorator(notification1);
        Console.WriteLine(notification1.SendMessage());

        notification1 = new SmsDecorator(notification1);
        Console.WriteLine(notification1.SendMessage());
        Console.ReadKey();
    }
}

/*
 Singleton - Ensures a class has only one instance and provides a global point of access
 */

public class NotificationManager
{
    private static NotificationManager? instance = null;
    private static readonly object checklock = new object();

    private NotificationManager() { }

    public static NotificationManager Instance
    {
        get
        {
            lock (checklock)
            {
                if(instance == null)
                {
                    instance = new NotificationManager();
                }
                return instance;
            }
        }
    }

    public void SendNotification(string message)
    {
        Console.WriteLine($"Sending notification: {message}");
    }
}

/*
 Factory - Provides a method to create objects without specifying the exact class
of the object that will be created.
 */

public interface INotification
{
    void SendMessage(string message);
}

public class EmailNotification : INotification
{
    public void SendMessage(string message)
    {
        Console.WriteLine($"Email Notification: {message}");
    }
}

public class SmsNotification : INotification
{
    public void SendMessage(string message)
    {
        Console.WriteLine($"SMS Notification: {message}");
    }
}

public class NotificationFactory
{
    public INotification CreateNotification(string type)
    {
        if (type == "Email")
            return new EmailNotification();
        else if (type == "SMS")
            return new SmsNotification();
        else
            throw new ArgumentException("Unknown notification type.");
    }
}

/*
 Observer Pattern - Defines a one-to-many relationship where one object changes
state, all its dependents are notified and updated automatically
 */

public interface INotificationObserver
{
    void UpdateMessage(string message);
}

public class EmailNotificationSubscriber : INotificationObserver
{
    public void UpdateMessage(string message)
    {
        Console.WriteLine($"Email Subscriber received: {message}");
    }
}

public class SmsNotificationSubscriber : INotificationObserver
{
    public void UpdateMessage(string message)
    {
        Console.WriteLine($"SMS Subscriber received: {message}");
    }
}

public class NotificationObserver
{
    private List<INotificationObserver> observers = new List<INotificationObserver>();

    public void Subscribe(INotificationObserver observer)
    {
        observers.Add(observer);
    }

    public void UnSubscribe(INotificationObserver observer)
    {
        observers.Remove(observer);
    }

    public void NotifySubscribers(string message)
    {
        foreach(var observer in observers)
        {
            observer.UpdateMessage(message);
        }
    }
}

/*
 Strategy Pattern - Allows a method to be defined on a family of algorithms,
encapsulate each one, and make them interchangeable
 */

public interface INotificationStrategy
{
    void SendMessage(string message);
}

public class EmailNotificationStrategy : INotificationStrategy
{
    public void SendMessage(string message)
    {
        Console.WriteLine($"Email Strategy: {message}");
    }
}

public class SmsNotificationStrategy : INotificationStrategy
{
    public void SendMessage(string message)
    {
        Console.WriteLine($"SMS Strategy: {message}");
    }
}

public class NotificationStrategyManager
{
    private INotificationStrategy? _strategy;

    public void SetStrategy(INotificationStrategy strategy)
    {
        _strategy = strategy;
    }

    public void SendStrategyNotification(string message)
    {
        _strategy?.SendMessage(message);
    }
}

/*
 Decorator pattern - Allows behavior to be added to an individual object,
either statically or dynamically, without affecting the behavior of other object
from the same class
 */

public interface INotificationDecorator
{
    string SendMessage();
}

public class BasicNotification : INotificationDecorator
{
    public string SendMessage()
    {
        return "Basic Notification";
    }
}

public class BasicNotificationDecorator : INotificationDecorator
{
    protected INotificationDecorator _notification;

    public BasicNotificationDecorator(INotificationDecorator notification)
    {
        _notification = notification;
    }

    public virtual string SendMessage()
    {
        return _notification.SendMessage();
    }
}

public class EmailDecorator : BasicNotificationDecorator
{
    public EmailDecorator(INotificationDecorator notification): base(notification) { }

    public override string SendMessage()
    {
        return $"EmailDecorator({base.SendMessage()})";
    }
}

public class SmsDecorator : BasicNotificationDecorator
{
    public SmsDecorator(INotificationDecorator notification) : base(notification) { }

    public override string SendMessage()
    {
        return $"SmsDecorator({base.SendMessage()})";
    }
}
